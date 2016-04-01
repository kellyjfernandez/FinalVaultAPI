using System.Collections.Generic;
using System.Linq;
using Vault.Migrations;

namespace Vault.Models
{
    public class User
    {
        public User()
        {
            
        }

        public User(AspNetUser aspNetUser)
        {
            Email = aspNetUser.Email;
            FirstName = aspNetUser.FirstName;
            LastName = aspNetUser.LastName;
            IsAdmin = aspNetUser.isAdmin;
            Permissions = aspNetUser.Departments.Select(aspNetDepartment => new Models.Department
            {
                DepartmentName = aspNetDepartment.DepartmentName,
                Computers = aspNetDepartment.Computers.Select(aspNetComputer => new Models.Computer
                {
                    ComputerName = aspNetComputer.ComputerName,
                    Credentials = aspNetComputer.Credentials.Select(aspNetCredential => new Models.Credential
                    {
                        UserName = aspNetCredential.UserName,
                        Password = aspNetCredential.Password
                    }).ToList()
                }).ToList()
            }).ToList();
        }
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public List<Department> Permissions { get; set; }
    }
}