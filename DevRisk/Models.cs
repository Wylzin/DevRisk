using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRisk
{
    public class ModelCategory
    {
        public int id { get; set; }
        public string name { get; set; }
        
    }

    public class ModelNormalTrade : ITrade
    {
        public double Value  { get; set; }

        public string ClientSector { get; set; }

        public DateTime NextPaymentDate { get; set; }

        public int idCategory { get; set; }

    }
}
