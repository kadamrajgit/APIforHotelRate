using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using HotelAvailabilityApi.Services;

namespace HotelAvailabilityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly HotelAvailabilityService _service;

        public HotelsController(HotelAvailabilityService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string city, [FromQuery] string date)
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(date))
                return BadRequest("city and date are required");

            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                return BadRequest("date must be in yyyy-MM-dd format");

            var hotels = _service.GetAvailableHotels(city, parsedDate);
            var result = new List<HotelDto>();
            foreach (var h in hotels)
            {
                result.Add(new HotelDto
                {
                    HotelName = h.HotelName,
                    City = h.City,
                    AvailableDate = h.AvailableDate,
                    IsAvailable = h.IsAvailable
                });
            }
            return Ok(result);
        }
    }
}
