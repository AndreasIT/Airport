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
//Get with location
        [HttpGet("location/{FlightId}")]
        public async Task<ActionResult<FlightItems>> GetFlightItemWL(string FromLocation, string ToLocation)
        {
            var FlightItem = await _context.flightItems.FindAsync(FromLocation, ToLocation);

            if (FlightItem == null)
            {
                return NotFound();
            }

            return FlightItem;
        }
//Post
        [HttpPost]
        public async Task<ActionResult<FlightItems>> PostFlightItem(FlightItems flightItem)
        {
            _context.flightItems.Add(flightItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlightItem", new
            { aircraftType = flightItem.AircraftType, fromLocation = flightItem.FromLocation, toLocation = flightItem.ToLocation,
                departureTime = flightItem.DepartureTime, arrivalTime = flightItem.ArrivalTime }, flightItem);
        }
//put
        [HttpPut("{FlightId}")]
        public async Task<IActionResult> PutFlightItem(int FlightId, FlightItems flightItem)
        {
            if (FlightId != flightItem.FlightId)
            {
                return BadRequest();
            }

            _context.Entry(flightItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}