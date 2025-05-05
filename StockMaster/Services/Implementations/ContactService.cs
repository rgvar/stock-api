using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StockMaster.Data;
using StockMaster.Dtos;
using StockMaster.Entities;
using StockMaster.Exceptions;
using StockMaster.Models;
using StockMaster.Services.Interfaces;

namespace StockMaster.Services
{
    public class ContactService<T> : IContactService<T> where T : Contact
    {
        private readonly StockDbContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<ContactService<T>> _logger;
        public ContactService(StockDbContext context, IMapper mapper, ILogger<ContactService<T>> logger)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public async Task<IEnumerable<ContactDto>> GetContacts()
        {
            _logger.LogInformation("Retrieving Contacts. ");
            var contacts = await _dbSet
                .AsNoTracking()
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation("Retrieved {total} Contacts successfully.", contacts.Count);

            return contacts;
        }
        public async Task<ContactDto> GetContactById(int id)
        {
            _logger.LogInformation("Retrieving Contact ID: {id}. ", id);

            var contact = await _dbSet.FindAsync(id);
            if (contact is null)
            {
                _logger.LogError("Contact ID: {id} does not exist. ", id);
                throw new NotFoundException($"Contact ID: {id} does not exist. ");
            }

            if (contact is Client client)
            {
                _logger.LogInformation("Retrieving Orders for Client ID: {id}.", id);
                await _context.Clients
                    .Entry(client)
                    .Collection(c => c.SalesOrders)
                    .LoadAsync();
            }
            else if (contact is Supplier supplier)
            {
                _logger.LogInformation("Retrieving Orders for Supplier ID: {id}.", id);
                await _context.Suppliers
                    .Entry(supplier)
                    .Collection(s => s.PurchaseOrders)
                    .LoadAsync();
            }

            var contactDto = _mapper.Map<ContactDto>(contact);
            _logger.LogInformation("Contact ID: {id} \"{name}\" retrieved successfully. ", id, contact.Name);

            return contactDto;
        }

        public async Task<ContactDto> CreateContact(CreateContactDto newContact)
        {
            _logger.LogInformation("Creating Contact \"{name}\". ", newContact.Name);

            var contact = _mapper.Map<T>(newContact);
            var savedContact = await _dbSet.AddAsync(contact);
            await _context.SaveChangesAsync();
            var savedContactDto = _mapper.Map<ContactDto>(savedContact.Entity);

            _logger.LogInformation("Contact ID: {id} \"{name}\" created successfully. ", savedContactDto.Id , savedContactDto.Name);

            return savedContactDto;
        }

        public async Task RemoveContact(int id)
        {
            _logger.LogInformation("Removing Contact \"{id}\". ", id);
            var contact = await _dbSet.FindAsync(id);
            if (contact is null)
            {
                _logger.LogError("Contact ID: {id} does not exist. ", id);
                throw new NotFoundException($"Contact ID: {id} does not exist. ");
            }
            _dbSet.Remove(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Contact ID: {id} \"{name}\" removed successfully. ", contact.Id, contact.Name);
        }

        public async Task UpdateContact(int id, ContactDto updatedContact)
        {
            if (id != updatedContact.Id) throw new Exception();

            var updatedContactEntity = _mapper.Map<T>(updatedContact);
            _dbSet.Update(updatedContactEntity);
            await _context.SaveChangesAsync();

        }
    }
}
