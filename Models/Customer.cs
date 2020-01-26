using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(1, 9999, ErrorMessage = "CustomerID must be 4 digits")]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(11)]
        public string TFN { get; set; }

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

        public virtual List<Account> Accounts { get; set; }
    }
}
