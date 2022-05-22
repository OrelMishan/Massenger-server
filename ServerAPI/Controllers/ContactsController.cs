using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ourServer.Models;
using ourServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerAPI.Controllers
{
    [EnableCors("Allow All")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private ContactService contacts = new ContactService();

        // GET: api/<ContactsController>
        [HttpGet]
        public IActionResult Get()
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            return Ok(q.Contacts);
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));

            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            return Ok(q.Contacts.Find(x => x.Id == id));


        }

        // POST api/<ContactsController>
        [HttpPost]
        public IActionResult Post(string id, string name, string server)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) != null){
                return BadRequest(); 
            }
            contacts.Update(HttpContext.Session.GetString("username"), new Contact() { Id = id, Name = name, Server = server });
            return Ok();
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, string name, string server)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            var toDel = q.Contacts.Find(x => x.Id == id);
            Contact add = new Contact() { Id = id, Name = name, Server = server, Last = toDel.Last, Lastdate = toDel.Lastdate, Messages = toDel.Messages };
            q.Contacts.Remove(toDel);
            contacts.Update(HttpContext.Session.GetString("username"), add);
            return Ok();

        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            var toDel = q.Contacts.Find(x => x.Id == id);
            q.Contacts.Remove(toDel);
            return Ok();

        }

        [HttpGet("{id}/messages")]
        public IActionResult GetMessages(string id)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            return Ok(q.Contacts.Find(x => x.Id == id).Messages);
        }

        [HttpPost("{id}/messages")]
        public IActionResult sentMessage(string id, string content){
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            Message add = new Message() { Content = content, Type = "text", Sent = true };
            q.Contacts.Find(x => x.Id == id).Messages.Add(add);
            return Created("{id}/messages",add);

        }

        [HttpGet("{id}/messages/{id2}")]
        public IActionResult GetMessages(string id, int id2)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            if(q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2)==null) {
                return BadRequest();
            }
            return Ok(q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2));
        }

        [HttpPut("{id}/messages/{id2}")]
        public IActionResult PutMessege(string id, int id2, string content)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2) == null)
            {
                return BadRequest();
            }
            q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2).Content = content;
            return NoContent();
        }

        [HttpDelete("{id}/messages/{id2}")]
        public IActionResult DelMessage(string id, int id2)
        {
            if (HttpContext.Session.GetString("username") == null)
                return BadRequest();
            var q = contacts.Get(HttpContext.Session.GetString("username"));
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2) == null)
            {
                return BadRequest();
            }
               var del= q.Contacts.Find(x => x.Id == id).Messages.Find(x => x.Id == id2);
            q.Contacts.Find(x => x.Id == id).Messages.Remove(del);
            return NoContent();
        }

        [HttpPost("invitations")]
        public IActionResult Invitation(string from,string to, string server)
        {
            Contact add = new Contact()
            {
                Id = from,
                Server = server,
                Messages = new List<Message>()
            };
            var q = contacts.Get(to);
            if (q.Contacts.Find(x => x.Id == from)!=null)
            {
                return BadRequest();
            }
            q.Contacts.Add(add);
            return Created("invitations",add);
        }

        [HttpPost("transfer")]
        public IActionResult Transfer(string from, string to, string content)
        {
            Message add = new Message()
            {
                Content = content,
                Sent = false,
                Type = "text"
            };
            var q = contacts.Get(to).Contacts.Find(x=>x.Id ==from);
            q.Messages.Add(add);
            return Created("transfer", add);
        }

        [HttpPost("login")]
        public IActionResult Login(string id, string password)
        {
            var q = contacts.Get(id);
            if (q == null)
            {
                return NotFound();
            }
            if (q.Password != password)
            {
                return NotFound();
            }
        //    HttpContext.Session.SetString("username", q.Id);
            return NoContent();
        }

        [HttpPost("register")]
        public IActionResult Register(string id,string password, string nickname,string? photo)
        {
            if (contacts.Get(id) != null)
            {
                return BadRequest();
            }
            User add = new User()
            {
                Id = id,
                Password = password,
                Photo = photo,
                Name = nickname
            };
            contacts.Add(add);
            HttpContext.Session.SetString("username", id);
            return Created("register", add);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            return NoContent();
        }
    }
}
