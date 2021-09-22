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
    public class SelfClass 
    {
        public decimal Id { get; set; }
    }

    public class SelfClass2
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Countrycode { get; set; }
        public decimal? Population { get; set; }
    }
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

            //return await _context.Cities.OrderBy(s => s.Name)
            //                     .ToListAsync();

            List<City> cities= await _context.Cities.OrderBy(s => s.Id)
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
        }

        // GET: api/Cities/5
        [HttpPost("GetCityById")]
        public async Task<ActionResult<ResponseDto>> GetCity([FromBody] SelfClass input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto 
                {
                    Message ="input error",
                    Success =false,
                    Payload = null
                });
            }
           
            var city = await _context.Cities.Where(c => c.Id == input.Id).FirstOrDefaultAsync();

            if (city == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Data Base has no data",
                    Success = false,
                    Payload = null
                });
            }

            else 
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseDto
                {
                    Message = "Data Found",
                    Success = true,
                    Payload = city
                });
            }
            
            
        }

        // PUT: api/Cities/5
        [HttpPost("UpdateCity")]
        public async Task<ActionResult<ResponseDto>> PutCity([FromBody] City input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto 
                {
                    Message="You have to type the ID",
                    Success=false,
                    Payload=null
                });
            }

            if (input.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Name Cant be Null!",
                    Success = false,
                    Payload = null
                });
            }

            if (input.Countrycode == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Countrycode Cant be Null!",
                    Success = false,
                    Payload = null
                });
            }
            if (input.Population == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "You have to type the Population Number!",
                    Success = false,
                    Payload = null
                });
            }


            City city = await _context.Cities.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
            if (city==null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto 
                {
                    Message= "DATA IS NOT FOUND!!",
                    Success= false,
                    Payload= null
                });
            }

            city.Name = input.Name;
            city.Countrycode = input.Countrycode;
            city.Population = input.Population;
            _context.Cities.Update(city);
            bool isSaved = await _context.SaveChangesAsync() > 0;

            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "update cant be execute server error!!",
                    Success = false,
                    Payload = null
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseDto 
                {
                    Message = "Update Complete",
                    Success = true,
                    Payload = null
                });
            }
        }

        // POST: api/Cities
        [HttpPost("InsertingNewCity")]
        public async Task<ActionResult<ResponseDto>> PostCity([FromBody] City input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "You have to type the ID",
                    Success = false,
                    Payload = null
                });
            }

            if (input.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Name Cant be Null!",
                    Success = false,
                    Payload = null
                });
            }

            if (input.Countrycode == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "Countrycode Cant be Null!",
                    Success = false,
                    Payload = null
                });
            }
            if (input.Population == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "You have to type the Population Number!",
                    Success = false,
                    Payload = null
                });
            }
            //till now is ok
            //old
            City city1 = await _context.Cities.Where(a => a.Id == input.Id).FirstOrDefaultAsync();
            if (city1 != null) 
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseDto 
                {
                    Message = "Data Already exists",
                    Success = false,
                    Payload = null
                });
            }

            _context.Cities.Add(input);
            bool isSaved = await _context.SaveChangesAsync() > 0 ;
            if (isSaved == false) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Data Saved Failed",
                    Success = false,
                    Payload = null
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseDto
                {
                    Message = "Data Successfully Saved",
                    Success = true,
                    Payload = null
                });
            }

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
