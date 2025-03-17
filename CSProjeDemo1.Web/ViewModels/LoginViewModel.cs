﻿using System.ComponentModel.DataAnnotations;

namespace CSProjeDemo1.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
