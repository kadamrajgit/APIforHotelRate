using System;

namespace HotelAvailabilityApi
{
    public class HotelDto
    {
        public string HotelName { get; set; }
        public string City { get; set; }
        public DateTime AvailableDate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
