﻿using System.ComponentModel.DataAnnotations;

namespace Profiles;

public class ReceptionistProfile
{
    [Key] public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? OfficeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
}