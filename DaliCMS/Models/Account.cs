using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(250)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Por favor repita la contraseña")]
        [StringLength(250)]
        public string PasswordCheck { get; set; }
    }
}