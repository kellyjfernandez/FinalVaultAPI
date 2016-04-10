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

        // GET: api/AspNetUsers/user@example.com
        [ResponseType(typeof(User))]
        public IHttpActionResult GetAspNetUser(string email)
        {
            IQueryable<AspNetUser> aspNetUsers = db.AspNetUsers;
            User userToBeFound = new User();
            foreach (User user in GetAspNetUsers().ToList())
            {
                if (user.Email.Equals(email))
                {
                    userToBeFound = user;
                }
            }
            if (userToBeFound.Email == null)
            {
                return NotFound();
            }

            return Ok(userToBeFound);
        }

        // PUT: api/AspNetUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetUser(string id, User user)
        {
            var userToUpdate = db.AspNetUsers.FirstOrDefault(x => x.Id == id);

            //These lines are what makes the write to the permission table

            db.AspNetUsers.Attach(userToUpdate);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.isAdmin = user.IsAdmin;
            userToUpdate.UserName = user.Email;

            List<Departmento> updatedPermissions = user.Permissions.ToList();
            List<Department> currentPermissions = userToUpdate.Departments.ToList();
            List<String> updatedPermissionsDepartmentNames = new List<string>();
            List<String> currentPermissionsDepartmentNames = new List<string>();

            foreach (Departmento item in updatedPermissions)
            {
                updatedPermissionsDepartmentNames.Add(item.DepartmentName);
            }
            foreach (Department item in currentPermissions)
            {
                currentPermissionsDepartmentNames.Add(item.DepartmentName);
            }
            List<String> namesOfDepartmentsToBeRemovedFromPermissions =
                currentPermissionsDepartmentNames.Except(updatedPermissionsDepartmentNames).ToList();

            foreach (String name in namesOfDepartmentsToBeRemovedFromPermissions)
            {
                var departmentToDelete = db.Departments.FirstOrDefault(x => x.DepartmentName == name);
                userToUpdate.Departments.Remove(departmentToDelete);
            }

            foreach (String name in updatedPermissionsDepartmentNames)
            {
                var departmentToAdd = db.Departments.FirstOrDefault(x => x.DepartmentName == name);
                userToUpdate.Departments.Add(departmentToAdd);
            }

            //end of added code

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userToUpdate.Id)
            {
                return BadRequest();
            }

            db.Entry(userToUpdate).State = EntityState.Modified;

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

        //DON'T USE, USE REGISTER METHOD IN ACCOUNTCONTROLLER CLASS INSTEAD
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

        //WORKS
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