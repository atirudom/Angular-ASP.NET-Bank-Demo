using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    public class AdminLoginDto
    {
        public string UserID { get; set; }

        public string Password { get; set; }
    }
}
