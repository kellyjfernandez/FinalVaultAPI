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
            Id = aspNetUser.Id;
            IsAdmin = aspNetUser.isAdmin;
            Permissions = aspNetUser.Departments.Select(aspNetDepartment => new Models.Departmento
            (aspNetDepartment)).ToList();
        }
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public bool IsAdmin { get; set; }
        public List<Departmento> Permissions { get; set; }
    }
}