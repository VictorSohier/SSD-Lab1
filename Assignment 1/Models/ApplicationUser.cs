using Assignment_1.Attributes;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_1.Models
{
    public class ApplicationUser : IdentityUser
    {
        // new keyword in this context overwrites the property in the base class
        [Key]
        [Required]
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName { get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public new string PhoneNumber;

        [DateRange("now", false)]
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }
    }
}
