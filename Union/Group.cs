using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union
{
    public class Group
    {
        public int id_group { get; set; }
        public string name_group { get; set; }
        public int FacultyId { get; set; } // Добавлено свойство для связи с факультетом
    }
}
