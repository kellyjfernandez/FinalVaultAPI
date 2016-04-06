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
    public class DepartmentsController : BaseController
    {
        //WORKS
        // GET: api/Departments
        public IQueryable<Departmento> GetDepartments()
        {
            IQueryable<Department> aspNetDepartments = db.Departments;
            List<Departmento> departments = aspNetDepartments.ToList().Select(aspNetDepartment => new Departmento(aspNetDepartment)).ToList();
            return departments.AsQueryable();
        }

        //DOESN"T WORK BUT IT IS NOT NEEDED 
        // GET: api/Departments/5
        [ResponseType(typeof(Departmento))]
        public IHttpActionResult GetDepartment(string deptName)
        {
            IQueryable<Department> aspNetDepartments = db.Departments;
            Departmento departmentToBeFound = new Departmento();

            foreach(Departmento dpt in GetDepartments().ToList())
            {
                if (dpt.DepartmentName.Equals(deptName))
                {
                    departmentToBeFound = dpt;
                }
            }

            if(departmentToBeFound.DepartmentName == null)
            {
                return NotFound();
            }

            //Department department = db.Departments.
            //if (department == null)
            //{
            //  return NotFound();
            //}

            //return Ok(new Departmento(department));
            return Ok(departmentToBeFound);
        }

        //DOESN"T WORK BUT IT IS NOT NEEDED 
        // PUT: api/Departments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepartment(string name, Department department)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (name != department.DepartmentName)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(name))
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

        //WORKS
        // POST: api/Departments
        [ResponseType(typeof(Departmento))]
        public IHttpActionResult PostDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DepartmentExists(department.DepartmentName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentName }, department);
        }

        //WORKS
        // DELETE: api/Departments/5
        [ResponseType(typeof(Departmento))]
        public IHttpActionResult DeleteDepartment(string name)
        {
            System.Diagnostics.Debug.WriteLine("The name passed in is" + name);

            IQueryable<Department> aspNetDepartments = db.Departments;
            Department departmentToBeRemoved = new Department();

            foreach (Department dpt in aspNetDepartments.ToList())
            {
                System.Diagnostics.Debug.WriteLine(dpt.DepartmentName);
                if (dpt.DepartmentName.Equals(name))
                {
                    departmentToBeRemoved = dpt;
                    System.Diagnostics.Debug.WriteLine("Found a Match!");
                }
            }

            if (departmentToBeRemoved.DepartmentName == null)
                return NotFound();

            db.Departments.Remove(departmentToBeRemoved);
            db.SaveChanges();

            return Ok(new Departmento(departmentToBeRemoved));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(string id)
        {
            return db.Departments.Count(e => e.DepartmentName == id) > 0;
        }
    }
}