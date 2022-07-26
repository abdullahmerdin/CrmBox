﻿using System.ComponentModel.DataAnnotations;

namespace CrmBox.WebUI.Models
{
    public class AddRoleVM
    {
        [Required(ErrorMessage = "Lütfen rol adını giriniz.")]
        public string Name { get; set; }
        public List<PolicyWithIsSelectedVM> Policies { get; set; }
    }
}
