using Microsoft.AspNetCore.Mvc;
using StockMaster.Dtos;

namespace StockMaster.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAll();
        Task<InvoiceDto> GetById(int id);
        Task<InvoiceDto> Create(CreateInvoiceDto createInvoiceDto);
        Task Update(int id, InvoiceDto updatedInvoice);
        Task Delete(int id);
    }
}
