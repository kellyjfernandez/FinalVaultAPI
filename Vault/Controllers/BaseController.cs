using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Vault.Models;

namespace Vault.Controllers
{
    [Authorize]
    public class BaseController : ApiController
    {
        protected AESencryptdecrypt kellyMonster = new AESencryptdecrypt();
        protected qahmed1Entities db = new qahmed1Entities();
    }
}