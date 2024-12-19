﻿using System.ComponentModel.DataAnnotations;

namespace Profiles;

public class DoctorProfile
{
    [Key] public Guid Id { get; set; }
    [Required] public Guid AccountId { get; set; }
    [Required] public Guid SpecializationId { get; set; }
    [Required] public Guid OfficeId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string MiddleName { get; set; }
    [Required] public string Status { get; set; }
    [Required] public DateTime DateOfBirth { get; set; }
    [Required] public DateTime CareerStartDate { get; set; }
}
