using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RailwayApp.Models
{
    public class Customer
    {
        [Column(Order = 1)]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column(Order = 10)]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "The FirstName field should consist of characters only")]
        public string FirstName { get; set; }

        [Column(Order = 11)]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage =  "The LastName field should consist of characters only")]
        public string LastName { get; set; }

        [Column(Order = 15)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Column(Order = 17)]
        public string StatusId { get; set; }

        [Column(Order = 22)]
        [Display(Name = "ID/ Passport No")]
        [MaxLength(100)]
        public string IdentificationNumber { get; set; }

        [Column(Order = 20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Column(Order = 21)]
        public int? RailwayUserId { get; set; }
        [ForeignKey("RailwayUserId")]
        public RailwayUser RailwayUser { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [Column(Order = 100)]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created On")]
        [Column(Order = 104)]
        public DateTime? CreatedDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modified On")]
        [Column(Order = 106)]
        public DateTime? ModifiedDateTime { get; set; }
    }
}