using ourServer.Data;
using ourServer.Models;

namespace ourServer.Services
{
    public class ContactService
    {
        private static List<User> users = new List<User>()
        {
            new User()
            {
                Id = "Orel",
                Password = "22222222",
                Photo = "orel.jpg",
                Name = "Orel",
                Contacts =
           new List<Contact>()
           {
               new Contact()
               {
                   Id = "David",
                   Name = "David",
                   Lastdate = DateTime.Now.ToString(),
                   Last = "Here's a pic!",
                   Server = "localhost:5108",
                   Messages = new List<Message>()
                   {
                       new Message() { Sent = true, Type = "text", Content = "Hey, how are you?" },
                       new Message() { Sent = false, Type = "text", Content = "I'm great, i'm in Paris" },
                       new Message() { Sent = false, Type = "text", Content = "Here's a pic!" }
                   }
               },
               new Contact()
               {
                   Id = "Sarah",
                   Name = "Sarah",
                   Lastdate = "17/5/2022 18:46",
                   Last = "ParisSelfie.jpg",
                   Server = "localhost:5108",
                   Messages = new List<Message>()
                   {
                       new Message() { Sent = true, Type = "text", Content = "Hey, how are you?" },
                       new Message() { Sent = false, Type = "text", Content = "I'm great, i'm in Paris" },
                       new Message() { Sent = false, Type = "text", Content = "ParisSelfie.jpg" }
                   }
               }
           }
            },
            new User()
            {
                Id = "David",
                Password = "22222222",
                Photo = "orel.jpg",
                Name = "david",
                Contacts =
                new List<Contact>() {
               new Contact()
               {
                   Id = "Orel",
                   Name = "Orel",
                   Lastdate = "17/5/2022 18:46",
                   Last = "Here's a pic!",
                   Server = "localhost:5108",
                   Messages = new List<Message>()
                   {
                       new Message() { Sent = false, Type = "text", Content = "Hey, how are you?" },
                       new Message() { Sent = true, Type = "text", Content = "I'm great, i'm in Paris" },
                       new Message() { Sent = true, Type = "text", Content = "Here's a pic!" }
                   }
               },
               new Contact()
               {
                   Id = "Sarah",
                   Name = "Sarah",
                   Lastdate = "17/5/2022 18:46",
                   Last = "ParisSelfie.jpg",
                   Server = "localhost:5108",
                   Messages = new List<Message>()
                   {
                       new Message() { Sent = true, Type = "text", Content = "Hey, how are you?" },
                       new Message() { Sent = false, Type = "text", Content = "I'm great, i'm in Paris" },
                       new Message() { Sent = false, Type = "text", Content = "ParisSelfie.jpg" }
                   }
               }
           }
            },
            new User()
            {
                Id = "Moshe",
                Password = "22222222",
                Photo = "orel.jpg",
                Name = "moshe",
                Contacts = new List<Contact>()
            },
            new User()
            {
                Id = "Sarah",
                Name = "Sarah",
                 Password = "22222222",
                Photo = "orel.jpg",
                Contacts =
                new List<Contact>() {
                      new Contact() {
                        Id = "Orel",
                        Name = "Orel",
                        Lastdate = "17/5/2022 18:46",
                        Last = "Here's a pic!",
                        Server = "localhost:5108",
                        Messages = new List<Message>(){
                           new Message() { Sent = false, Type = "text", Content = "Hey, how are you?" },
                           new Message() { Sent = true, Type = "text", Content = "I'm great, i'm in Paris" },
                           new Message() { Sent = true, Type = "text", Content = "ParisSelfie.jpg" }
                         }
                      },
                      new Contact() {
                       Id = "David",
                       Name = "david",
                       Server = "localhost:5108",
                       Lastdate = "17/5/2022 18:46",
                       Last = "ParisSelfie.jpg",
                       Messages = new List<Message>(){
                           new Message() { Sent = false, Type = "text", Content = "Hey, how are you?" },
                           new Message() { Sent = true, Type = "text", Content = "I'm great, i'm in Paris" },
                           new Message() { Sent = true, Type = "text", Content = "ParisSelfie.jpg" }
                       }
                      }
                }
            }
        };

        public List<User> GetALL()
        {
            return users;
        }

        public User Get(string id)
        {
            return users.Find(x => x.Id == id);
        }

        public void Delete(string id)
        {
            users.Remove(Get(id));
        }

        public void Add(User contact)
        {
            users.Add(contact);
        }

        public void Update(string id, Contact contact)
        {
            var q = users.Find(x => x.Id == id);
            q.Contacts.Add(contact);
        }

    }
}
