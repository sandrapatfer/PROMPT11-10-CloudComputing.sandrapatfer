using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Server.MvcModel
{
    public class UserClaims
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        public string Provider{ get; set; }
        public string NameIdentifier { get; set; }
    }
}