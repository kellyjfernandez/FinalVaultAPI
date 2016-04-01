using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vault;

namespace Vault.Controllers
{
    [Authorize]
    public class CredentialsController : ApiController
    {
        private qahmed1Entities db = new qahmed1Entities();

        // GET: api/Credentials
        public IQueryable<Credential> GetCredentials()
        {
            return db.Credentials;
        }

        // GET: api/Credentials/5
        [ResponseType(typeof(Credential))]
        public IHttpActionResult GetCredential(string id)
        {
            Credential credential = db.Credentials.Find(id);
            if (credential == null)
            {
                return NotFound();
            }

            return Ok(credential);
        }

        // PUT: api/Credentials/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCredential(string id, Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credential.UserName)
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

        // POST: api/Credentials
        [ResponseType(typeof(Credential))]
        public IHttpActionResult PostCredential(Credential credential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Credentials.Add(credential);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CredentialExists(credential.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = credential.UserName }, credential);
        }

        // DELETE: api/Credentials/5
        [ResponseType(typeof(Credential))]
        public IHttpActionResult DeleteCredential(string id)
        {
            Credential credential = db.Credentials.Find(id);
            if (credential == null)
            {
                return NotFound();
            }

            db.Credentials.Remove(credential);
            db.SaveChanges();

            return Ok(credential);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CredentialExists(string id)
        {
            return db.Credentials.Count(e => e.UserName == id) > 0;
        }
    }
}