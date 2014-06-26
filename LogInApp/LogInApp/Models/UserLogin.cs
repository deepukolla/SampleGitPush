using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LogInApp.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please provide UserName", AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please provide Password", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]        
        public string Password { get; set; }  
    }
}