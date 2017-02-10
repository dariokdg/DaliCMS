using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class DaliCMSContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<ResponsibleAdult> ResponsibleAdults { get; set; }
        public DbSet<StudentRespAdultRel> StudentsRespAdultsRel { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}