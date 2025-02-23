using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspSecond.DAL.Entities
{
    public class Person
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public string OtherInfo { get; set; }
    }
}
