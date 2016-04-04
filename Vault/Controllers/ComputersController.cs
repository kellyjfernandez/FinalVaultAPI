using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Vault.Models;

namespace Vault.Controllers
{
    public class ComputersController : BaseController
    {

        // GET: api/Computers
        public IQueryable<Comptadora> GetComputers()
        {
            IQueryable<Computer> aspNetComputers = db.Computers;
            List<Comptadora> computers = new List<Comptadora>();
            foreach (Computer aspNetComputer in aspNetComputers.ToList())
            {
                computers.Add(new Comptadora(aspNetComputer));
            }
            return computers.AsQueryable();
        }

        // GET: api/Computers/5
        [ResponseType(typeof(Comptadora))]
        public IHttpActionResult GetComputer(int id)
        {
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return NotFound();
            }

            return Ok(new Comptadora(computer));
        }

        // PUT: api/Computers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComputer(int id, Computer computer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != computer.ComputerId)
            {
                return BadRequest();
            }

            db.Entry(computer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputerExists(id))
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

        // POST: api/Computers
        [ResponseType(typeof(Computer))]
        public IHttpActionResult PostComputer(Computer computer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Computers.Add(computer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ComputerExists(computer.ComputerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = computer.ComputerId }, computer);
        }

        // DELETE: api/Computers/5
        [ResponseType(typeof(Computer))]
        public IHttpActionResult DeleteComputer(int id)
        {
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return NotFound();
            }

            db.Computers.Remove(computer);
            db.SaveChanges();

            return Ok(computer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ComputerExists(int id)
        {
            return db.Computers.Count(e => e.ComputerId == id) > 0;
        }
    }
}