using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadAlert.Entities;

namespace ThreadAlert.DTOs
{
    public class CreateMessage
    {
        [DisplayName("Dangerous Object")]
        public int DangerousObjectId { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Priority Priority {  get; set; }
    }
        
}
