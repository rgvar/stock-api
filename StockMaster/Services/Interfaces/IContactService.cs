using StockMaster.Dtos;
using StockMaster.Entities;

namespace StockMaster.Services.Interfaces
{
    public interface IContactService<T> where T : Contact
    {
        Task<IEnumerable<ContactDto>> GetContacts();
        Task<ContactDto> GetContactById(int id);
        Task<ContactDto> CreateContact(CreateContactDto newContact);
        Task UpdateContact(int id, ContactDto updatedContact);
        Task RemoveContact(int id);
    }
}
