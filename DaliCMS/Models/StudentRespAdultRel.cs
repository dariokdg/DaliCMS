using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class StudentRespAdultRel
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student StudentModel { get; set; }
        public int ResponsibleAdultId { get; set; }
        public virtual ResponsibleAdult ResponsibleAdultModel { get; set; }
    }
}