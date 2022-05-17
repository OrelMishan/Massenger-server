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
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private ContactService contacts = new ContactService();

        // GET: api/<ContactsController>
        [HttpGet]
        public IActionResult Get()
        {
            //var q = contacts.GetALL().Find(x => x.Id == HttpContext.Session.GetString("username"));
            var q = contacts.Get("Orel");
            return Ok(q.Contacts);
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            //if (HttpContext.Session.GetString("username") == null) {
              //  return NotFound();
                //return to login
                //}
            //var q = contacts.Get(HttpContext.Session.GetString("username"));
            var q = contacts.Get("Orel");

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
            var q = contacts.Get("Orel");
            if (q.Contacts.Find(x => x.Id == id) != null){
                return BadRequest(); 
            }
            contacts.Update("Orel",new Contact() { Id = id, Name = name, Server = server });
            return Ok();
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, string name, string server)
        {
            var q = contacts.Get("Orel");
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            var toDel = q.Contacts.Find(x => x.Id == id);
            Contact add = new Contact() { Id = id, Name = name, Server = server, Last = toDel.Last, Lastdate = toDel.Lastdate, Messages = toDel.Messages };
            q.Contacts.Remove(toDel);
            contacts.Update("Orel", add);
            return Ok();

        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var q = contacts.Get("Orel");
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
            var q = contacts.Get("Orel");
            if (q.Contacts.Find(x => x.Id == id) == null)
            {
                return BadRequest();
            }
            return Ok(q.Contacts.Find(x => x.Id == id).Messages);
        }

        [HttpPost("{id}/messages")]
        public IActionResult sentMessage(string id, string content){
            var q = contacts.Get("Orel");
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
            var q = contacts.Get("Orel");
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
            var q = contacts.Get("Orel");
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
            var q = contacts.Get("Orel");
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
    }
}






//orel the king