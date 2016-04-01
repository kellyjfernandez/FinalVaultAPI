using System.Collections.Generic;

namespace Vault.Models
{
    public class Department
    {
        public string DepartmentName { get; set; }
        public List<Computer> Computers { get; set; }
    }
}