using System.ComponentModel.DataAnnotations;

namespace Offices;

public class Office
{
    [Key]
    public Guid Id { get; set; }

    public Guid? PhotoId { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string RegistryPhoneNumber { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Office o)
        {
            return PhotoId == o.PhotoId &&
                Address == o.Address &&
                RegistryPhoneNumber == o.RegistryPhoneNumber &&
                IsActive == o.IsActive;
        }

        return false;
    }
}
