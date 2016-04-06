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
    public class CredentialsController : BaseController
    {
        //WORKS
        // GET: api/Credentials
        public IQueryable<Credencial> GetCredentials()
        {
            IQueryable<Credential> aspNetCredentials = db.Credentials;
            List<Credencial> credencials = aspNetCredentials.ToList().Select(aspNetCredential => new Credencial(aspNetCredential)).ToList();
            return credencials.AsQueryable();
        }

        //WORKS
        // GET: api/Credentials/5
        [ResponseType(typeof(Credencial))]
        public IHttpActionResult GetCredential(int id)
        {
            IQueryable<Credential> aspNetCredentials = db.Credentials;
            Credential credentialToBeFound = new Credential();

            foreach(Credential cred in aspNetCredentials.ToList())
            {
                System.Diagnostics.Debug.WriteLine(cred.Id);
                if (cred.Id == id)
                {
                    credentialToBeFound = cred;
                    System.Diagnostics.Debug.WriteLine("Found a Match!");
                }
            }

            //Credential credential = db.Credentials.Find(id);
            if (credentialToBeFound.Id == 0)
            {
                return NotFound();
            }

            return Ok(new Credencial(credentialToBeFound));
        }

        // PUT: api/Credentials/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCredential(int id, Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credential.Id)
            {
                return BadRequest();
            }

            db.Entry(credential).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredentialExists(id))
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
        // POST: api/Credentials
        [ResponseType(typeof(Credencial))]
        public IHttpActionResult PostCredential(Credential credential)
        {
            Credential credentialToBeAdded = new Credential
            {
                UserName = credential.UserName,
                Password = credential.Password,
                ComputerId = credential.ComputerId,
                Type = credential.Type

            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Credentials.Add(credentialToBeAdded);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CredentialExists(credential.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = credentialToBeAdded.Id}, credentialToBeAdded);
        }

        // DELETE: api/Credentials/5
        [ResponseType(typeof(Credencial))]
        public IHttpActionResult DeleteCredential(int id)
        {
            IQueryable<Credential> aspNetCredentials = db.Credentials;
            Credential credentialToBeDeleted = new Credential();

            foreach (Credential cred in aspNetCredentials.ToList())
            {
                System.Diagnostics.Debug.WriteLine(cred.Id);
                if (cred.Id == id)
                {
                    credentialToBeDeleted = cred;
                    System.Diagnostics.Debug.WriteLine("Found a Match!");
                }
            }


            //Credential credential = db.Credentials.Find(id);
            if (credentialToBeDeleted.Id == 0)
            {
                return NotFound();
            }

            db.Credentials.Remove(credentialToBeDeleted);
            db.SaveChanges();

            return Ok(new Credencial(credentialToBeDeleted));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CredentialExists(int id)
        {
            return db.Credentials.Count(e => e.Id == id) > 0;
        }
    }
}