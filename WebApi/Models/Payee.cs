using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
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

        public AustralianState State { get; set; }

        [StringLength(10), RegularExpression("^[0-9]{4}$", ErrorMessage = "Postcode must be 4 digits")]
        public string PostCode { get; set; }

        [Required, StringLength(15), RegularExpression(@"^\(61\)- ([0-9]{4}) ([0-9]{4})$", ErrorMessage = "Phone number must be (61)- XXXX XXXX")]
        public string Phone { get; set; }

        public virtual List<BillPay> BillPays { get; set; }
    }
}
