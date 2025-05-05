using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMaster.Data;
using StockMaster.Models;
using StockMaster.Services.Interfaces;
using StockMaster.Dtos;
using StockMaster.Mapping;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using StockMaster.Exceptions;

namespace StockMaster.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly StockDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(StockDbContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            
        }

        public async Task<IEnumerable<ProductDto>> GetAll(string? search)
        {
            bool hasSearch = !string.IsNullOrWhiteSpace(search);
            _logger.LogInformation("Retrieving products. ");

            var query = _context.Products.AsNoTracking();

            if (hasSearch)
            {
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{search}%"));
            }

            var products = await query
                .OrderBy(p => p.Id)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation("Retrieved {count} products. ", products.Count);

            return products;
        }

        public async Task<ProductDto> GetById(int id)
        {
            _logger.LogInformation("Retrieving product ID: {id} ... ", id);

            var product = await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                _logger.LogError("Product ID: {id} not found. ", id);
                throw new NotFoundException($"Product ID: {id} not found. ");
            }

            var dtoProduct = _mapper.Map<ProductDto>(product);

            _logger.LogInformation("Product ID: {id} \"{name}\" retrieved. ", id, product.Name);

            return dtoProduct;
        }

        public async Task<ProductDto> Create(CreateProductDto newProductDto)
        {
            _logger.LogInformation("Creating Product \"{name}\" ... ", newProductDto.Name);
            var product = _mapper.Map<Product>(newProductDto);

            var existingCategories = await _context.Categories
                .Where(c => newProductDto.CategoriesId.Contains(c.Id))
                .ToListAsync();

            if (newProductDto.CategoriesId.Any() && existingCategories.Count != newProductDto.CategoriesId.Count)
            {
                _logger.LogError("Some categories could not be mapped for Product \"{name}\". ", product.Name);
                throw new BadHttpRequestException("Some categories could not be mapped. ");
            }

            product.Categories = existingCategories;

            var addedProduct = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(addedProduct.Entity);

            _logger.LogInformation("Product ID: {id} \"{name}\" created successfully. ", productDto.Id, productDto.Name);

            return productDto;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Deleting Product ID: {id} ... ", id);

            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                _logger.LogError("Product ID: {id} not found. ", id);
                throw new NotFoundException($"Product ID: {id} not found. ");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product ID: {id} \"{name}\" removed successfully. ", id, product.Name);

        }

        public async Task Update(int id, UpdateProductDto toUpdateProduct)
        {
            _logger.LogInformation("Updating Product ID: {id} ... ", id);

            if (id != toUpdateProduct.Id)
            {
                _logger.LogError("Product ID mismatch. Given ID: {givenId}, Expected ID: {expectedId}. ", toUpdateProduct.Id, id);
                throw new BadHttpRequestException($"Product ID mismatch. Given ID: {toUpdateProduct.Id}, Expected ID: {id}. ");
            }

            var product = await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                _logger.LogError("Product ID: {id} not found. ", id);
                throw new NotFoundException($"Product ID: {id} not found. ");
            }

            _mapper.Map(toUpdateProduct, product);

            var categoryIds = toUpdateProduct.Categories.Select(c => c.Id).ToList();
            var existingCategories = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            if (toUpdateProduct.Categories.Any() && existingCategories.Count != toUpdateProduct.Categories.Count)
            {
                _logger.LogError("Some categories could not be mapped for Product ID: {id} \"{name}\". ", id, product.Name);
                throw new BadHttpRequestException("Some categories could not be mapped. ");
            }

            product.Categories = existingCategories;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Product ID: {id} \"{name}\" updated successfully. ", product.Id, product.Name);

        }

    }
}
