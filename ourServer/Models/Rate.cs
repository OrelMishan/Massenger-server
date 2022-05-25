using System.ComponentModel.DataAnnotations;

namespace ourServer.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Range(0,5)]

        public int numRate { get; set; }

        public string stringRate { get; set; }
        public string date = DateTime.Now.ToString();

    }
}
