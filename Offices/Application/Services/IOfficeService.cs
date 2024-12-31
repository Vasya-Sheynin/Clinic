using Application.Dto;
using Offices;

namespace Application
{
    public interface IOfficeService
    {
        Task<IEnumerable<OfficeDto>?> GetOffices();
        Task<OfficeDto?> GetOffice(Guid id);
        Task<OfficeDto> CreateOffice(CreateOfficeDto createOfficeDto);
        Task UpdateOffice(Guid id, UpdateOfficeDto updateOfficeDto);
        Task DeleteOffice(Guid id);
    }
}
