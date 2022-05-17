using ourServer.Models;

namespace ourServer.Services
{
    
    public class RatesService
    {
        private static List<Rate> rates = new List<Rate>() { new Rate() {Name = "orel",numRate=4,stringRate="great" },
                                                             new Rate() {Name = "David",numRate=1,stringRate="bad" },
                                                             new Rate() {Name = "Shimi",numRate=3,stringRate="good" },
                                                             new Rate() {Name = "Avi",numRate=2,stringRate="ok" }};
        public List<Rate> GetALL() { 
            return rates;
        }

        public Rate Get(int id)
        {   
            return rates.Find(x => x.Id == id);
        }

        public void Edit(int id, int numRate, string stringRate)        {
            Rate rate = Get(id);
            rate.stringRate = stringRate;
            rate.numRate = numRate;
        }
        public  void Delete(int id)
        {
            rates.Remove(Get(id));
        }

        public void Add(Rate rate)
        {
            rates.Add(rate);
        }

        public double GetAverage()
        {
            double average = rates.Sum(x=>x.numRate);
            return average/rates.Count;
        }


    }
}
