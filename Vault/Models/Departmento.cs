using System.Collections.Generic;

namespace Vault.Models
{
    public class Departmento
    {
        public string DepartmentName { get; set; }
        public List<Comptadora> Computers { get; set; }
    }
}