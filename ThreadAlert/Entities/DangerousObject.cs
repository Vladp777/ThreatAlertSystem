using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadAlert.Entities
{
    public class DangerousObject
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AppUser> Users { get; set; }
    }
}
