using System;
using System.ComponentModel.DataAnnotations;

namespace RestAPINet6Base.DTOs
{
    public class UserCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
