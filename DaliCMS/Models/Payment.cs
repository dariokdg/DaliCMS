using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaliCMS.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El monto del pago es obligatorio")]
        public decimal Amount { get; set; }
        public decimal CancellationAmount { get; set; }
        public bool HasDiscount { get; set; }
        public bool HasPenalty { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha del pago es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }
        [Required(ErrorMessage = "El mes-año asociado a este pago es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}")]
        public DateTime MonthYearPaid { get; set; }
        public int StudentId { get; set; }
        public virtual Student StudentModel { get; set; }
    }
}