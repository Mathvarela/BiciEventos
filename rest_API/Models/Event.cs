using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rest_API.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public double start_Latitude { get; set; }
        public double end_Latitude { get; set; }
        public double start_Longitude { get; set; }
        public double end_Longitude{ get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public string Username { get; set; }
    }
}