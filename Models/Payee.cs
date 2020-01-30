using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Payee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(0, 9999)]
        public int PayeeID { get; set; }

        [Required, StringLength(50)]
        public string PayeeName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        // incomplete format
        [StringLength(20)]
        public string State { get; set; }

        // incomplete format
        [StringLength(10)]
        public string PostCode { get; set; }

        // incomplete format
        [Required, StringLength(15)]
        public string Phone { get; set; }

        public virtual List<BillPay> BillPays { get; set; }
    }
}
