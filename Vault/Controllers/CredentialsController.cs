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
                if (cred.Id == id)
                {
                    credentialToBeFound = cred;
                }
            }
            
            if (credentialToBeFound.Id == 0)
            {
                return NotFound();
            }

            return Ok(new Credencial(credentialToBeFound));
        }

        //WORKS
        // PUT: api/Credentials/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCredential(int id, Credential credential)
        {
            IQueryable<Credential> aspNetCredentials = db.Credentials;
            Credential credentialToBeUpdated = new Credential();

            foreach (Credential cred in aspNetCredentials.ToList())
            {
                if (cred.Id == id)
                {
                    credentialToBeUpdated = cred;
                }
            }

            db.Credentials.Attach(credentialToBeUpdated);
            credentialToBeUpdated.UserName = kellyMonster.encrypt(credential.UserName);
            credentialToBeUpdated.Password = kellyMonster.encrypt(credential.Password);
            credentialToBeUpdated.Type = credential.Type;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credentialToBeUpdated.Id)
            {
                return BadRequest();
            }

            db.Entry(credentialToBeUpdated).State = EntityState.Modified;

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
                UserName = kellyMonster.encrypt(credential.UserName),
                Password = kellyMonster.encrypt(credential.Password),
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

        //WORKS
        // DELETE: api/Credentials/5
        [ResponseType(typeof(Credencial))]
        public IHttpActionResult DeleteCredential(int id)
        {
            IQueryable<Credential> aspNetCredentials = db.Credentials;
            Credential credentialToBeDeleted = new Credential();

            foreach (Credential cred in aspNetCredentials.ToList())
            {
                if (cred.Id == id)
                {
                    credentialToBeDeleted = cred;
                }
            }

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