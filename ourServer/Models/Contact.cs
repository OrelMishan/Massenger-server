using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ourServer.Models
{
    public class Contact
    {

        [Required]
        public string Id { get; set; }

        public string Name { get; set; }

        [Required]        
        public string Server { get; set; } = "localhost:7169";

        public string Last { get; set; } = "";

        public String Lastdate { get; set; } = "";

        [Required]
        [JsonIgnore]
        public List<Message> Messages { get; set; } = new List<Message>();

    }
}