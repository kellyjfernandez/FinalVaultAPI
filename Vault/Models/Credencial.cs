namespace Vault.Models
{
    public class Credencial
    {
        public Credencial()
        {
            
        }
        public Credencial(Credential aspNetCredential)
        {
            UserName = aspNetCredential.UserName;
            Password = aspNetCredential.Password;
            Type = aspNetCredential.Type;
        }
        public string UserName { get; set;}
        public string Password { get; set;}
        public string Type { get; set;}
}
}