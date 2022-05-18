using System.ComponentModel.DataAnnotations;

namespace ourServer.Models
{
    public class User
    {   
        [Required]
        public string Id { get; set; }
        
        [Range(8,12)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? Photo{ get; set; }

        [Required]
        public string Name { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();

        
    }
} 
