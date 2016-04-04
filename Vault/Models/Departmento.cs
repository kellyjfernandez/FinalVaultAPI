using System.Collections.Generic;
using System.Linq;

namespace Vault.Models
{
    public class Departmento
    {
        public Departmento()
        {
            
        }

        public Departmento(Department aspNetDepartment)
        {
            DepartmentName = aspNetDepartment.DepartmentName;
            Computers = aspNetDepartment.Computers.Select(aspNetComputer => new Comptadora(aspNetComputer)).ToList();
        }
        public string DepartmentName { get; set; }
        public List<Comptadora> Computers { get; set; }
    }
}