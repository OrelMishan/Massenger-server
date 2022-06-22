using Microsoft.AspNetCore.Mvc;
using ourServer.Models;
using ourServer.Services;
using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNet.SignalR;
using ServerAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using Message = ourServer.Models.Message;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerAPI.Controllers
{
    [EnableCors("Allow All")]
    [Route("api")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private ContactService contacts = new ContactService();
        private readonly IHubContext<ServerHub> hub;

        public ContactsController(IHubContext<ServerHub> hub)
        {
            this.hub = hub;
           

        }
        // GET: api/<ContactsController>
        [HttpGet("contacts")]
        public IActionResult Get(string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            return Ok(q.Contacts);
        }

        // GET api/<ContactsController>/5
        [HttpGet("contacts/{id}")]
        public IActionResult Get(string id, string username)
        {

            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            return Ok(q.Contacts.Find(x => x.Id == id));
        }

        // POST api/<ContactsController>
        [HttpPost("contacts")]
        public IActionResult Post([FromBody] Contact add, string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == add.Id) != null){
                return BadRequest();
            }
            contacts.Update(username,add);
            return Ok();
        }

        // PUT api/<ContactsController>/5
        [HttpPut("contacts/{id}")]
        public IActionResult Put([FromBody] Contact edit, string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == edit.Id) == null)
            {
                return BadRequest();
            }
            var toDel = q.Contacts.Find(x => x.Id == edit.Id);
            q.Contacts.Remove(toDel);
            contacts.Update( username,edit);
            return Ok();

        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("contacts/{id}")]
        public IActionResult Delete(string id,string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            var toDel = q.Contacts.Find(x => x.Id == id);
            q.Contacts.Remove(toDel);
            return Ok();

        }

        [HttpGet("contacts/{id}/messages")]
        public IActionResult GetMessages(string id, string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            return Ok(q.Contacts.Find(x => x.Id == id).Messages);
        }

        [HttpPost("contacts/{id}/messages")]
        public IActionResult sentMessage( string id, string username,[FromBody] messageContent content)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            Message add = new Message() { Content = content.Content, Type = "text", Sent = true };
            var user = q.Contacts.Find(x => x.Id == id);
            user.Messages.Add(add);
            user.Last = content.Content;
            user.Lastdate = DateTime.Now.ToString();
            return Created("contacts/{id}/messages", add);

        }

        [HttpGet("contacts/{id}/messages/{id2}")]
        public IActionResult GetMessages(string id, int id2,string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            if (q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2) == null) {
                return BadRequest();
            }
            return Ok(q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2));
        }

        [HttpPut("contacts/{id}/messages/{id2}")]
        public IActionResult PutMessege(string id, int id2, [FromBody]string content, string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            if (q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2) == null)
            {
                return BadRequest();
            }
            q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2).Content = content;
            return NoContent();
        }

        [HttpDelete("contacts/{id}/messages/{id2}")]
        public IActionResult DelMessage(string id, int id2, string username)
        {
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            var del = q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2);
            if (del == null)
            {
                return BadRequest();
            }
            q.Contacts.Find(x => x.Id == id).Messages.Remove(del);
            return NoContent();
        }

        [HttpPost("invitations")]
        public async Task<IActionResult> Invitation([FromBody] Invatation invatation)
        {
            Contact add = new Contact()
            {
                Id = invatation.From,
                Name = invatation.From,
                Server = invatation.Server
            };
            var q = contacts.Get(invatation.To);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == invatation.From)!=null)
            {
                return BadRequest();
            }
            q.Contacts.Add(add);
            await hub.Clients.All.SendAsync("ReceiveContact");
            return Created("invitations",add);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] Transfer message)
        {
            Message add = new Message()
            {
                Content = message.Content,
                Sent = false,
                Type = "text"
            };
            var q = contacts.Get(message.To);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == message.From) == null)
            {
                return BadRequest();
            }
            var user = q.Contacts.Find(x=>x.Id==message.From);
            user.Messages.Add(add);
            user.Last = message.Content;
            user.Lastdate = DateTime.Now.ToString();
            await hub.Clients.All.SendAsync("ReceiveMessage");
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("private_key.json")

                });
            }
            if (q.Token != null)
            {
                var registrationToken = q.Token;
                    var fbmessage = new FirebaseAdmin.Messaging.Message()
                {

                    Token = registrationToken,
                    Notification = new Notification()
                    {
                        Title = message.From.ToString(),
                        Body = message.Content.ToString()
                    }
                };
                string response =
                    FirebaseMessaging.DefaultInstance.SendAsync(fbmessage).Result;
            }
            return Created("transfer", add);
        }
        

        [HttpPost("contacts/login")]
        public IActionResult Login([FromBody] Login login)
        {
            var q = contacts.Get(login.Username);
            if (q == null)
            {
                return NotFound();
            }
            if (q.Password != login.Password)
            {
                return NotFound();
            }
            q.Token = login.Token;
            return NoContent();
        }

        [HttpPost]
        [Route("/api/contacts/register")]
        public IActionResult Register([FromBody] User add)
        {
            if (contacts.Get(add.Id) != null)
            {
                return BadRequest();
            }
            add.Contacts = new List<Contact>();
            contacts.Add(add);
            return Created("register", add);
        }

        [HttpGet("/api/contacts/user/{id}")]
        public IActionResult GetUser(string id)
        {
            var q = contacts.Get(id);
            var list = new List<User>();
            list.Add(q);
            if (q == null)
            {
                return BadRequest();
            }
            return Ok(list);
        }

    }
}
