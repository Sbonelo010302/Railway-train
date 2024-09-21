using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RailwayApp.Models
{
    public class Reservation 
    {
        [Column(Order = 1)]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }


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

        public bool? Booked { get; set; }

        [Display(Name ="Booking Expiry")]
        public DateTime? Expiry { get; set; }

        //[NotMapped]
        [Range(0 ,int.MaxValue,ErrorMessage ="Please select a valid number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Number must be a positive number")]
        public int NoOfReservations { get; set; }

        public string SingleReferenceNumber { get; set; }
        public string GroupReferenceNumber { get; set; }

        public int? DayToDayTrainRouteId { get; set; }
        [ForeignKey("DayToDayTrainRouteId")]
        public DayToDayTrainRoute DayToDayTrainRoute { get; set; }

        [NotMapped]
        public string Data { get; set; }
    }
    public class RefGen
    {
        public static string ResReferenceGenerator(bool SingleRes)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                Random random = new Random();
                int randNumber = random.Next(10000, 99999);
                string RefPrefix = SingleRes ? "RESS" : "RESG";

                switch (SingleRes)
                {
                    case true:
                        string refnos = string.Format("{0}{1}", RefPrefix, randNumber);
                        _ = (context.Reservations.Select(x => x.SingleReferenceNumber).Contains(refnos)) ? ResReferenceGenerator(SingleRes) : refnos;
                        return refnos;

                    default:
                        string refnog = string.Format("{0}{1}", RefPrefix, randNumber);
                        _ = (context.Reservations.Select(x => x.SingleReferenceNumber).Contains(refnog)) ? ResReferenceGenerator(SingleRes) : refnog;
                        return refnog;
                }
            }
            catch (Exception ex)
            {
                return ResReferenceGenerator(SingleRes);
            }
        }
    }
}