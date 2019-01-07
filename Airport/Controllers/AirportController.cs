using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Airport.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Airport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportContext _context;

        public AirportController(AirportContext context)
        {
            _context = context;

            if (_context.flightItems.Count() == 0)
            {
                _context.flightItems.Add(new FlightItems { AircraftType = "Plane1" });
                _context.SaveChanges();
            }
        }
//Get all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightItems>>> GetFlightItems()
        {
            return await _context.flightItems.ToListAsync();
        }
//Get With id
        [HttpGet("{FlightId}")]
        public async Task<ActionResult<FlightItems>> GetFlightItem(int FlightId)
        {
            var FlightItem = await _context.flightItems.FindAsync(FlightId);

            if (FlightItem == null)
            {
                return NotFound();
            }

            return FlightItem;
        }
    }
}