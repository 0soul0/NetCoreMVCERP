using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreMVCERP.Models
{
    public class Employee
    {
        [Display(Name = "編輯")]
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public int Seniority { get; set; }
        public string Name { get; set; }

        public decimal Currency { get; set; }
        public DateTimeOffset Time { get; set; }

        public string Test { get; set; }
        public List<string> Title { get; set; }
    }
}
