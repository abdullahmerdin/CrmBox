﻿using CrmBox.Core.Domain.Base;

namespace CrmBox.Core.Domain;

public class Customer : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string CompanyName { get; set; }
    public string JobTitle { get; set; }
    public DateTime BirthDate { get; set; }
}