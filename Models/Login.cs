using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Login
    {
        [Required, Range(1000, 9999)]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Key, Required, StringLength(50)]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
    }
}
