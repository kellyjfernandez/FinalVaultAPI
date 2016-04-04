using System.Collections.Generic;
using System.Linq;

namespace Vault.Models
{
    public class Comptadora
    {
        public Comptadora()
        {
            
        }
        public Comptadora(Computer aspNetComputer)
        {
            ComputerName = aspNetComputer.ComputerName;
            DepartmentName = aspNetComputer.DepartmentName;
            Credentials = aspNetComputer.Credentials.Select(aspNetCredential => new Credencial(aspNetCredential)).ToList();
        }

     

        public string ComputerName { get; set; }
        public string DepartmentName { get; set; }
        public List<Credencial> Credentials { set; get; }
    }
}