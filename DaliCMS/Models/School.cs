using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class School
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la escuela es obligatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La dirección de la escuela es obligatoria")]
        public string Address { get; set; }
        [Required(ErrorMessage = "La ciudad de la escuela es obligatoria")]
        public string City { get; set; }
        [Required(ErrorMessage = "El teléfono de la escuela es obligatorio")]
        public string Phone { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}