using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayApp.Models
{
    public class DayToDayTrainRoute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Depature Day")]
        public DateTime Day { get; set; }
        [Required]

        [Display(Name = "Depature Time")]
        public string DepatureTime { get; set; }

        public string ArrivalTime { get; set; }
        [Required]
        [Display(Name = "Train")]
        public int TrainId { get; set; }
        [ForeignKey("TrainId")]
        public Train Train { get; set; }
        [Required]
        [Display(Name = "Route")]
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route Route { get; set; }

    }
}