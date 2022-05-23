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
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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

        [HttpGet("{id}/messages")]
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

        [HttpPost("{id}/messages")]
        public IActionResult sentMessage(string id, [FromBody] string content,string username){
            var q = contacts.Get(username);
            if (q == null)
            {
                return BadRequest();
            }
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return NotFound();
            }
            Message add = new Message() { Content = content, Type = "text", Sent = true };
            q.Contacts.Find(x => x.Id == id).Messages.Add(add);
            return Created("{id}/messages",add);

        }

        [HttpGet("{id}/messages/{id2}")]
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

        [HttpPut("{id}/messages/{id2}")]
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

        [HttpDelete("{id}/messages/{id2}")]
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
        public IActionResult Invitation([FromBody] Invatation invatation)
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
            return Created("invitations",add);
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] Transfer message)
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
            q.Contacts.Find(x=>x.Id==message.From).Messages.Add(add);
            return Created("transfer", add);
        }

        [HttpPost]
        [Route("/api/contacts/login")]
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
            contacts.Add(add);
            return Created("register", add);
        }

        [HttpGet("/api/contacts/user/{id}")]
        public IActionResult GetUser(string id)
        {
            var q = contacts.Get(id);
            if (q == null)
            {
                return BadRequest();
            }
            return Ok(q);
        }

    }
}
