using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Vault.Controllers
{
    [Authorize]
    public class BaseController : ApiController
    {
    
        protected qahmed1Entities db = new qahmed1Entities();
    }
}