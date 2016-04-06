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
        //Works
        // GET: api/Computers
        public IQueryable<Comptadora> GetComputers()
        {
            IQueryable<Computer> aspNetComputers = db.Computers;
            List<Comptadora> computers = aspNetComputers.ToList().Select(aspNetComputer => new Comptadora(aspNetComputer)).ToList();
            return computers.AsQueryable();
        }

        //Works
        // GET: api/Computers/5
        [ResponseType(typeof(Comptadora))]
        public IHttpActionResult GetComputer(int id)
        {
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return NotFound();
            }
            Comptadora computerFound = new Comptadora(computer);
            computerFound.ComputerId = computer.ComputerId;
            return Ok(computerFound);
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

        //Works
        // POST: api/Computers
        [ResponseType(typeof(Comptadora))]
        public IHttpActionResult PostComputer(Computer computer)
        {
            Computer compToBeAdded = new Computer
            {
                ComputerName = computer.ComputerName,
                DepartmentName = computer.DepartmentName,
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Computers.Add(compToBeAdded);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (ComputerExists(compToBeAdded.ComputerId))
                {
                    return Conflict();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = compToBeAdded.ComputerId }, compToBeAdded);
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