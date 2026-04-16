using System;

namespace PETC.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public int ServiceId { get; set; }

        public int DoctorId { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }

        public string Note { get; set; }
    }
}