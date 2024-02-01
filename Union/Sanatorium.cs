using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union
{
    public class Sanatorium
    {
        public int Id { get; set; }
        public int Nbileta { get; set; }
        public int Nzaezda { get; set; }
        public DateTime God { get; set; }
        public DateTime NachaloZaezda { get; set; }
        public DateTime KonecZaezda { get; set; }
        public string StatusOplaty { get; set; }

        public Sanatorium()
        {

        }

        public Sanatorium(int id, int nbileta, int nzaezda, DateTime god, DateTime nachaloZaezda, DateTime konecZaezda, string statusOplaty)
        {
            Id = id;
            Nbileta = nbileta;
            Nzaezda = nzaezda;
            God = god;
            NachaloZaezda = nachaloZaezda;
            KonecZaezda = konecZaezda;
            StatusOplaty = statusOplaty;
        }
    }

}
