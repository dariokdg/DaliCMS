using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class Level
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del nivel es obligatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El precio base es obligatorio")]
        public decimal BasePrice { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}