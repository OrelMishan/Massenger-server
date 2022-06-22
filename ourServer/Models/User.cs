using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ourServer.Models
{
    public class User
    {   
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string Password { get; set; }

        public string? Photo{ get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public string? Token { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        
    }
} 
