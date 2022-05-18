using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ourServer.Models
{
    public class Message
    {
        private static int numId = 1;

        public int Id { get; set; } = numId++;
       
        [Required]
        public string Content { get; set; }


        public string Created { get; set; } = DateTime.Now.ToString();
        
        [Required]
        public bool Sent { get; set; }
        
        [Required]
        [JsonIgnore]
        public string Type { get; set; }
        

    }
}