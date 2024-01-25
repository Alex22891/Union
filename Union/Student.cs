using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union
{
    public class Student
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public int Nbileta { get; set; }
        public int IdGroup { get; set; }
        public string OtherInfo { get; set; }

        public Group Group { get; set; }
        public Faculty Faculty { get; set; }
    }
}
