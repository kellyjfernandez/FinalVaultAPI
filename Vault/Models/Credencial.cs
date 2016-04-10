using System.ComponentModel.DataAnnotations.Schema;

namespace Vault.Models
{
    public class Credencial
    {
        AESencryptdecrypt kellyMonster = new AESencryptdecrypt();
        public Credencial()
        {
            
        }
        public Credencial(Credential aspNetCredential)
        {
            UserName = kellyMonster.decrypt(aspNetCredential.UserName);
            Password = kellyMonster.decrypt(aspNetCredential.Password);
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