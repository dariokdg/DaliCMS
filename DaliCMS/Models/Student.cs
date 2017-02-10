using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del alumno es obligatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El DNI del alumno es obligatorio")]
        public int DNI { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Dateofbirth { get; set; }
        public decimal Debt { get; set; }
        public int LevelId { get; set; }
        public virtual Level LevelModel { get; set; }
        public int SchoolId { get; set; }
        public virtual School SchoolModel { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<StudentRespAdultRel> StudentRespAdultRels { get; set; }
    }
}