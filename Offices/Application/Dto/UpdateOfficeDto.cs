using System.ComponentModel.DataAnnotations;

namespace Application.Dto;

public record UpdateOfficeDto (
    Guid? PhotoId,
    [Required] string Address,
    [Required] string RegistryPhoneNumber,
    [Required] bool IsActive
    );
