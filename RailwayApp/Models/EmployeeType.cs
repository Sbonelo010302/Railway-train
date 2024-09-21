using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RailwayApp.Models
{
    public class EmployeeType
    {
        [Column(Order = 1)]
        [Display(Name = "Id")]
        public int Id { get; set; }

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

        [Required]
        [Column(Order = 10)]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column(Order = 11)]
        [Display(Name = "Description")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Column(Order = 12)]
        [Display(Name = "Key")]
        [MaxLength(100)]
        public string Key { get; set; }
    }
}