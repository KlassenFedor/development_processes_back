using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace dev_processes_backend.Models.Dtos.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
