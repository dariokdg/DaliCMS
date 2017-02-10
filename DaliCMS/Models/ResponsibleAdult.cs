using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class ResponsibleAdult
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string JobName { get; set; }
        public virtual ICollection<StudentRespAdultRel> StudentRespAdultRels { get; set; }
    }
}