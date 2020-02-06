using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminApi.Models
{
    public enum LoginStatus
    {
        Locked = 1,
        Normal = 2
    }

    public class Login
    {
        [Key, Required, Range(0, 9999)]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime ModifyDate { get; set; }

        [Required]
        public int NumOfFailedLoginAttempt { get; set; }

        public DateTime LockUntilTime { get; set; }

        [Required]
        public LoginStatus Status { get; set; }

        public void AttemptLoginSuccessful()
        {
            NumOfFailedLoginAttempt = 0;
        }

        public void AttemptLoginFailed()
        {
            NumOfFailedLoginAttempt += 1;
            if (NumOfFailedLoginAttempt >= 3)
            {
                LockTemp();
            }
        }

        public void Unlock()
        {
            NumOfFailedLoginAttempt = 0;
            Status = LoginStatus.Normal;
        }

        public void LockTemp()
        {
            LockUntilTime = DateTime.UtcNow.AddMinutes(1);
            Status = LoginStatus.Locked;
        }
    }
}
