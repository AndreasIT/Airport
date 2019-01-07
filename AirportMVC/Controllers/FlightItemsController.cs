using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirportMVC.Models;

namespace AirportMVC.Controllers
{
    public class FlightItemsController : Controller
    {
        private readonly AirportContext _context;

        public FlightItemsController(AirportContext context)
        {
            _context = context;
        }

        // GET: FlightItems
        public async Task<IActionResult> Index(string FromString, string ToString)
        {
            var flights = from f in _context.flightItems
                         select f;

            if (!String.IsNullOrEmpty(FromString))
            {
                flights = flights.Where(s => s.FromLocation.Contains(FromString));
            }

            if (!String.IsNullOrEmpty(ToString))
            {
                flights = flights.Where(s => s.ToLocation.Contains(ToString));
            }

            return View(await flights.ToListAsync());
        }

        // GET: FlightItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlightItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime")] FlightItems flightItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flightItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flightItems);
        }

        // GET: FlightItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightItems = await _context.flightItems.FindAsync(id);
            if (flightItems == null)
            {
                return NotFound();
            }
            return View(flightItems);
        }

        // POST: FlightItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime")] FlightItems flightItems)
        {
            if (id != flightItems.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flightItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightItemsExists(flightItems.FlightId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flightItems);
        }


        private bool FlightItemsExists(int id)
        {
            return _context.flightItems.Any(e => e.FlightId == id);
        }
    }
}
