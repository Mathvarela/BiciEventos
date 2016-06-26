using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using rest_API.Models;

namespace rest_API.Controllers
{
    public class Event_GoingController : ApiController
    {
        private rest_APIContext db = new rest_APIContext();

        // GET: api/Event_Going
        public IQueryable<Event_Going> GetEvent_Going()
        {
            return db.Event_Going;
        }

        // GET: api/Event_Going/5
        [ResponseType(typeof(Event_Going))]
        public async Task<IHttpActionResult> GetEvent_Going(int id)
        {
            Event_Going event_Going = await db.Event_Going.FindAsync(id);
            if (event_Going == null)
            {
                return NotFound();
            }

            return Ok(event_Going);
        }

        // PUT: api/Event_Going/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent_Going(int id, Event_Going event_Going)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != event_Going.Id)
            {
                return BadRequest();
            }

            db.Entry(event_Going).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Event_GoingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Event_Going
        [ResponseType(typeof(Event_Going))]
        public async Task<IHttpActionResult> PostEvent_Going(Event_Going event_Going)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Event_Going.Add(event_Going);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = event_Going.Id }, event_Going);
        }

        // DELETE: api/Event_Going/5
        [ResponseType(typeof(Event_Going))]
        public async Task<IHttpActionResult> DeleteEvent_Going(int id)
        {
            Event_Going event_Going = await db.Event_Going.FindAsync(id);
            if (event_Going == null)
            {
                return NotFound();
            }

            db.Event_Going.Remove(event_Going);
            await db.SaveChangesAsync();

            return Ok(event_Going);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Event_GoingExists(int id)
        {
            return db.Event_Going.Count(e => e.Id == id) > 0;
        }
    }
}