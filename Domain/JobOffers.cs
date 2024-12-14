using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class JobOffer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }



        public override string ToString()
        {
            return $"Stanowisko: {Title}, Firma: {Company}, Lokalizacja: {Location}";
        }
    }
}
