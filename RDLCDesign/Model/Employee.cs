using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDLCDesign.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public double Salary { get; set; }
        public int Seniority { get; set; }
        public string Name { get; set; }

        public string Test { get; set; }
        public List<string> Title { get; set; }
    }
}
