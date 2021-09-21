using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CIty1.Models;
using CIty1.DTOs;

namespace CIty1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesTryController : ControllerBase
    {
        private readonly ModelContext _context;

        public CitiesTryController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetCities()
        {
            //return await _context.Cities.OrderByDescending(s => s.Population)
            //                     .ToListAsync();

            List<City> cities= await _context.Cities.OrderBy(s => s.Name)
                                 .ToListAsync();

            if (cities.Count>0) 
            {
               return StatusCode(StatusCodes.Status200OK, new ResponseDto
                {
                    Message = "this is 4th times",
                    Success = true,
                    Payload = cities
                });
            }
            else 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "this is 4th times",
                    Success = false,
                    Payload = null
                });
            }

            //return await _context.Cities.OrderBy(s => s.Name)
            //                     .ToListAsync();
            
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(decimal id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(decimal id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            _context.Cities.Add(city);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CityExists(city.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(decimal id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(decimal id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
