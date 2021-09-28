using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiBlogEngine.Data;
using WebApiBlogEngine.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiBlogEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublishedController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<PublishedController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Published>>> GetPublished()
        {
            return await _context.Published.ToListAsync();
        }

        // GET: api/PublishedController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Published>> GetPublished(long id)
        {
            var published = await _context.Published.FindAsync(id);

            if (published == null)
            {
                return NotFound();
            }

            return published;
        }



        // POST api/<PublishedController>
        [HttpPost]
        public async Task<ActionResult<Published>> PostPublished(Published published)
        {
            _context.Published.Add(published);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublished", new { id = published.idPublished }, published);
        }

        // PUT: api/Published/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublished(long id, Published published)
        {
            if (id != published.idPublished)
            {
                return BadRequest();
            }

            _context.Entry(published).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublishedExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(long id)
        {
            var published = await _context.Published.FindAsync(id);
            if (published == null)
            {
                return NotFound();
            }

            _context.Published.Remove(published);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublishedExists(long id)
        {
            return _context.Published.Any(e => e.idPublished == id);
        }

        // PUT: api/Published/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> SubmitPublished(long id, Published published)
        //{
        //    if (id != published.idPublished)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(published).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PublishedExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<Published>>> GetPublishedWriter(long idUser)
        //{
        //    var appResponse =  GetPublished().Result;
        //    var publishedList = JsonConvert.DeserializeObject<List<Published>>(appResponse.ToString());

        //    //Hacer foreach y eliminar registros que no sean igual a id
        //    foreach (var publishL in publishedList)
        //    {
        //        if (publishL.idUser != idUser)
        //        {
        //            publishedList.Remove(publishL);
        //        }
        //    }


        //    return publishedList;

        //}


        // GET api/<PublishedController>/5
        /*[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        

        // PUT api/<PublishedController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PublishedController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
