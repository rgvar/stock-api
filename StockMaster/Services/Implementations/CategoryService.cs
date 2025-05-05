using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StockMaster.Data;
using StockMaster.Dtos;
using StockMaster.Exceptions;
using StockMaster.Models;
using StockMaster.Services.Interfaces;

namespace StockMaster.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly StockDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(StockDbContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CategorySimpleDto> Create(CreateCategoryDto newCategoryDto)
        {
            _logger.LogInformation("Creating Category \"{name}\" ... ", newCategoryDto.Name);

            var newCategory = _mapper.Map<Category>(newCategoryDto);

            var result = (await _context.Categories.AddAsync(newCategory)).Entity;

            await _context.SaveChangesAsync();


            _logger.LogInformation("Category ID: {id} \"{name}\" successfully created. ", result.Id, result.Name);

            return _mapper.Map<CategorySimpleDto>(result);
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Removing Category ID: {id} ... ", id);

            var cat = await _context.Categories.FindAsync(id);
            
            if (cat is null)
            {
                _logger.LogError("Category ID: {id} was not found. ", id);
                throw new NotFoundException($"Category ID: {id} was not found. ");
            }

            _context.Categories.Remove(cat);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Category ID: {id} \"{name}\" successfully deleted. ", cat.Id, cat.Name);
        }

        public async Task<IEnumerable<CategorySimpleDto>> GetAll()
        {
            _logger.LogInformation("Retrieving categories ... ");

            var result = await _context.Categories
                .ProjectTo<CategorySimpleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation("Categories retrieved. ");

            return result;
        }

        public async Task<CategorySimpleDto> GetById(int id)
        {
            _logger.LogInformation("Searching for Category ID: {id} ... ", id);
            var result = await _context.Categories.FindAsync(id);
            if (result is null)
            {
                _logger.LogError("Category ID: {id} was not found. ", id);
                throw new NotFoundException($"Category ID: {id} was not found. ");
            } 
            _logger.LogInformation("Category ID: {id} \"{name}\" found. ", id, result.Name);
            return _mapper.Map<CategorySimpleDto>(result);
        }

        public async Task<CategoryDto> GetWithProductsById(int id)
        {
            _logger.LogInformation("Searching for Category ID: {id} ... ", id);
            var result = await _context.Categories.FindAsync(id);
            if (result is null)
            {
                _logger.LogError("Category ID: {id} was not found. ", id);
                throw new NotFoundException($"Category ID: {id} was not found. ");
            }

            _logger.LogInformation("Category ID: {id} \"{name}\" found with {productCount}. ", result.Id, result.Name, result.Products);

            return _mapper.Map<CategoryDto>(result);
        }

        public async Task Update(int id, CategorySimpleDto updatedCategoryDto)
        {
            _logger.LogInformation("Updating Category ID: {id} ... ", id);

            if (id != updatedCategoryDto.Id)
            {
                _logger.LogError("Category ID mismatch. Given ID: {givenId}, Expected ID: {expectedId}. ", updatedCategoryDto.Id, id);
                throw new BadHttpRequestException("Category ID mismatch. ");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                _logger.LogError("Category ID: {id} was not found. ", id);
                throw new NotFoundException($"Category ID: {id} was not found. ");
            }

            category.Name = updatedCategoryDto.Name;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Category ID: {id} successfully updated. ", id);

        }
    }
}
