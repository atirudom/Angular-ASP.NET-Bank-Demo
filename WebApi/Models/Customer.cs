using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public enum AustralianState
    {
        NSW = 1,
        QLD = 2,
        SA = 3,
        TAS = 4,
        VIC = 5,
        WA = 6
    }

    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(1000, 9999, ErrorMessage = "CustomerID must be less than 4 digits")]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(11)]
        public string TFN { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        public AustralianState State { get; set; }

        [StringLength(10), RegularExpression("^[0-9]{4}$", ErrorMessage = "Postcode must be 4 digits")]
        public string PostCode { get; set; }

        [Required, StringLength(15), RegularExpression(@"^\(61\)- ([0-9]{4}) ([0-9]{4})$", ErrorMessage = "Phone number must be (61)- XXXX XXXX")]
        public string Phone { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}
