using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public IQueryable<User> GetAspNetUsers()
        {
            IQueryable<AspNetUser> aspNetUsers = db.AspNetUsers;
            List<User> users = aspNetUsers.ToList().Select(aspNetUser => new User(aspNetUser)).ToList();
            return users.AsQueryable();
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

            return Ok(new User(aspNetUser));
        }

        // PUT: api/AspNetUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetUser(string id, User user)
        {
            AspNetUser aspNetUser = new AspNetUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.Password,
                UserName = user.Email,
                isAdmin = user.IsAdmin,
                Departments = new Collection<Department>(user.Permissions.Select(departmento => new Department
                {
                    DepartmentName = departmento.DepartmentName,
                    Computers = new Collection<Computer>(departmento.Computers.Select(computadora => new Computer
                    {
                        ComputerName = computadora.ComputerName,
                        DepartmentName = computadora.DepartmentName,
                        ComputerId = computadora.ComputerId,
                        Credentials = new Collection<Credential>(computadora.Credentials.Select(credencial => new Credential
                        {
                            UserName = credencial.UserName,
                            Password = credencial.Password,
                            Type = credencial.Type
                        }).ToList())
     
                    }).ToList())
                }).ToList()),
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };
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
        [ResponseType(typeof(User))]
        public IHttpActionResult PostAspNetUser(User user)
        {
            AspNetUser aspNetUser = new AspNetUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.Password,
                UserName = user.Email,
                isAdmin = user.IsAdmin,
                Departments = new Collection<Department>(user.Permissions.Select(departmento => new Department
                {
                    DepartmentName = departmento.DepartmentName,
                    Computers = new Collection<Computer>(departmento.Computers.Select(computadora => new Computer
                    {
                        ComputerName = computadora.ComputerName,
                        DepartmentName = computadora.DepartmentName,
                        ComputerId = computadora.ComputerId,
                        Credentials = new Collection<Credential>(computadora.Credentials.Select(credencial => new Credential
                        {
                            UserName = credencial.UserName,
                            Password = credencial.Password,
                            Type = credencial.Type
                        }).ToList())

                    }).ToList())
                }).ToList()),
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

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
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();

            return Ok(new User(aspNetUser));
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