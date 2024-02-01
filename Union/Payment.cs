using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union
{
    public class Payment
    {
        public int IdOplaty { get; set; }
        public int Summa { get; set; }
        public DateTime GodOplaty { get; set; }
        public string StatusOplaty { get; set; }
        public int IdPerioda { get; set; }
        public int Nbileta { get; set; }
    }

}
