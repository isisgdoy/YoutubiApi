﻿using System.ComponentModel.DataAnnotations;

namespace YoutubiApi.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
