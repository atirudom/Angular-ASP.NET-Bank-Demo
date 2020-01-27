using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class BillPay
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, 9999)]
        public int BillPayID { get; set; }

        [Range(1, 9999), Required]
        public int AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual Account Account { get; set; }

        [Range(1, 9999), Required]
        public int PayeeID { get; set; }

        public virtual Payee Payee { get; set; }


        [Column(TypeName = "money"), Range(1, 99999999), Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ScheduleDate { get; set; }

        [Required]
        public string Period { get; set; }

        [Required]
        public DateTime ModifyDate { get; set; }
    }
}
