using System.Collections.Generic;

namespace Vault.Models
{
    public class Computer
    {
        public string ComputerName { get; set; }
        public List<Credential> Credentials { set; get; }
    }
}