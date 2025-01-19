using System.ComponentModel.DataAnnotations;

namespace Application.Dto;

public record OfficeDto (
    [Required] Guid Id,
    Guid? PhotoId,
    [Required] string Address,
    [Required] string RegistryPhoneNumber,
    [Required] bool IsActive
    );
