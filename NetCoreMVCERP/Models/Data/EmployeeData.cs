using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCERP.Models.Data
{
    public class EmployeeData
    {   
        /// <summary>
        /// 員工資料
        /// </summary>
        public static IEnumerable<Employee> Employees{
            get{
                List<Employee> employees = new List<Employee>();

                for (int i = 0; i < 100; i++) {
                    employees.Add(
                        new Employee() {
                            Id = i,
                            Name = "A" + i,
                            Salary = new Random().Next(1, 100) * 10000,
                            Seniority = 0
                           // Seniority = new Random().Next(1, 20)
                        });
                }
                return employees;
            }
        }

    }
}
