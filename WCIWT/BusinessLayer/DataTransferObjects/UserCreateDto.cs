using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DataTransferObjects
{
    public class UserCreateDto : DtoBase
    {
        [Required(ErrorMessage = "Username is required!")]
        [MaxLength(64, ErrorMessage = "Username name is too long!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "This is not valid email address!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required!")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 30")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required!")]
        public Gender Gender { get; set; }
        
        [Required(ErrorMessage = "Birthdate is required!")]
        [UserDateAttributeValidation(ErrorMessage = "You must be at least 15 years old to register")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }

}
