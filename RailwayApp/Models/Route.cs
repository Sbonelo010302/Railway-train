using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RailwayApp.Models
{
    public class Route
    {
        [Column(Order = 1)]
        [Display(Name = "Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public string To { get; set; }

        [Column(Order = 3)]
        public string From { get; set; }

        [Column(Order = 4)]
        [Range(0,int.MaxValue)]
        [DataType(DataType.Currency)]
        [Display(Name ="Cost")]
        public decimal Rate { get; set; }

        [Column(Order = 5)]
        [Range(0, int.MaxValue)]
        public int? Sequence { get; set; }

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
        public int Hours { get; set; }
        [Required]
        public int Minutes { get; set; }

        //public int? TrainId { get; set; }
        //[ForeignKey("TrainId")]
        //public Train Train { get; set; }


        [Display(Name = "Route Name")]
        public string RouteName
        {
            get { return string.Format("{0} - {1}", From , To); }
        }        
        
        //[Display(Name = "Route Name")]
        //public string RouteNameDrp
        //{
        //    get { return string.Format("{0} , {1} - {2}", To , From, Train.Name); }
        //}
    }
}