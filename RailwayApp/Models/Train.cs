using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RailwayApp.Models
{
    public class Train
    {
        [Column(Order = 1)]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column(Order = 10)]
        [Display(Name = "Train Name")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "The FirstName field should consist of characters only")]
        public string Name { get; set; }

        [Column(Order = 11)]
        [Display(Name = "Max Number of Passengers")]
        public int MaxNoOfPassengers { get; set; }

        [NotMapped]
        public string Data { get; set; }

    }
}