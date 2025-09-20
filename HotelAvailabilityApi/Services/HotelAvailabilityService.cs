using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace HotelAvailabilityApi.Services
{
    public class HotelAvailability
    {
        public string HotelName { get; set; }
        public string City { get; set; }
        public DateTime AvailableDate { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class HotelAvailabilityService
    {
        private readonly List<HotelAvailability> _hotels;

        public HotelAvailabilityService(string excelPath)
        {
            _hotels = new List<HotelAvailability>();
            LoadFromExcel(excelPath);
        }

        private void LoadFromExcel(string excelPath)
        {
            if (!File.Exists(excelPath))
                throw new FileNotFoundException($"Excel file not found: {excelPath}");

            if (Path.GetExtension(excelPath).ToLower() == ".csv")
            {
                // Simple CSV loader for demo
                var lines = File.ReadAllLines(excelPath).Skip(1);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        _hotels.Add(new HotelAvailability
                        {
                            HotelName = parts[0],
                            City = parts[1],
                            AvailableDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            IsAvailable = parts[3].Trim().ToUpper() == "TRUE"
                        });
                    }
                }
                return;
            }

            using var workbook = new XLWorkbook(excelPath);
            var ws = workbook.Worksheet(1);
            var rows = ws.RangeUsed().RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                _hotels.Add(new HotelAvailability
                {
                    HotelName = row.Cell(1).GetString(),
                    City = row.Cell(2).GetString(),
                    AvailableDate = row.Cell(3).GetDateTime(),
                    IsAvailable = row.Cell(4).GetBoolean()
                });
            }
        }

        public IEnumerable<HotelAvailability> GetAvailableHotels(string city, DateTime date)
        {
            return _hotels.Where(h => h.City.Equals(city, StringComparison.OrdinalIgnoreCase) && h.AvailableDate.Date == date.Date);
        }
    }
}
