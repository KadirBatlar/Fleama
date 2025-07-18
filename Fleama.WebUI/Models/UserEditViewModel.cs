﻿using System.ComponentModel.DataAnnotations;

namespace Fleama.WebUI.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; } 
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}