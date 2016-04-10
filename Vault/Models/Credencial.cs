using System.ComponentModel.DataAnnotations.Schema;
using Vault.Controllers;

namespace Vault.Models
{
    public class Credencial
    {
        public Credencial()
        {
            
        }
        public Credencial(Credential aspNetCredential)
        {
            UserName = BaseController.kellyMonster.decrypt(aspNetCredential.UserName);
            Password = BaseController.kellyMonster.decrypt(aspNetCredential.Password);
            Type = aspNetCredential.Type;
            ComputerId = aspNetCredential.ComputerId;
            if (aspNetCredential.Id != 0)
                Id = aspNetCredential.Id;

        }
        public string UserName { get; set;}
        public string Password { get; set;}
        public string Type { get; set;}
        public int ComputerId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
}
}