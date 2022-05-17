using Microsoft.AspNetCore.Mvc;
using ourServer.Models;
using ourServer.Services;

namespace ourServer.Controllers
{
    public class RatesController : Controller
    {
       public RatesService service;

        public RatesController()
        {
            service = new RatesService();
        }

        // GET: Rates
        public IActionResult Index()
        {
              return View(service.GetALL());

        }

        // GET: Rates/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rate = service.Get((int)id);
            if (rate == null)
            {
                return NotFound();
            }

            return View(rate);
        }

        // GET: Rates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,numRate,stringRate")] Rate rate)
        {   
            if(service.GetALL().Count() == 0)
                rate.Id = 1;
            else
                rate.Id = service.GetALL().Max(x=>x.Id)+1;
            if (ModelState.IsValid)
            {
                service.Add(rate);
                return RedirectToAction(nameof(Index));
            }
            return View(rate);
        }

        // GET: Rates/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rate = service.Get((int)id);
            if (rate == null)
            {
                return NotFound();
            }
            return View(rate);
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int numRate,string stringRate)
        {
            service.Edit(id, numRate, stringRate);
            return RedirectToAction(nameof(Index));
        }

        // GET: Rates/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var rate = service.Get((int)id);
            if (rate == null)
            {
                return NotFound();
            }
            return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var rate = service.Get(id);
            if (rate != null)
            {
                service.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search()
        {
            return View("index",service.GetALL());
        }
        
        
        [HttpPost]
        public IActionResult Search(string query)
        {
           
            var q = service.GetALL().Where(x => x.stringRate.Contains(query));
            if (q.Count() > 0)
            {
                return View("index", q);
            }
            return View("index", new List<Rate>());

        }


    }
}
