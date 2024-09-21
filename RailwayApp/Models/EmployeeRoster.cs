using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RailwayApp.Models
{
    public class EmployeeRoster
    {
        [Column(Order = 1)]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }


        [Column(Order = 3)]
        public int? RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route Route { get; set; }


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