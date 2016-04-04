using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Vault;
using Vault.Models;

namespace Vault.Controllers
{
    public class AspNetUsersController : BaseController
    {

        // GET: api/AspNetUsers
        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            return db.AspNetUsers;
        }

        // GET: api/AspNetUsers/5Z
        [ResponseType(typeof(User))]
        public IHttpActionResult GetAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return Ok(new User
            {
                Email = aspNetUser.Email,
                FirstName = aspNetUser.FirstName,
                LastName = aspNetUser.LastName,
                IsAdmin = aspNetUser.isAdmin,
                Permissions = aspNetUser.Departments.Select(aspNetDepartment => new Models.Departmento
                {
                    DepartmentName = aspNetDepartment.DepartmentName,
                    Computers = aspNetDepartment.Computers.Select(aspNetComputer => new Models.Comptadora
                    {
                        ComputerName = aspNetComputer.ComputerName,
                        Credentials = aspNetComputer.Credentials.Select(aspNetCredential => new Models.Credencial
                        {
                            UserName = aspNetCredential.UserName,
                            Password = aspNetCredential.Password
                        }).ToList()
                    }).ToList()
                }).ToList()

            });
        }

        // PUT: api/AspNetUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetUser(string id, AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetUser.Id)
            {
                return BadRequest();
            }

            db.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
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

        // POST: api/AspNetUsers
        [ResponseType(typeof(AspNetUser))]
        public IHttpActionResult PostAspNetUser(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUsers.Add(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspNetUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspNetUser.Id }, aspNetUser);
        }

        // DELETE: api/AspNetUsers/5
        [ResponseType(typeof(AspNetUser))]
        public IHttpActionResult DeleteAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();

            return Ok(aspNetUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}