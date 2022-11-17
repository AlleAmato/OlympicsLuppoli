using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympics.Models
{
    internal partial class Partecipation
    {
        public string SexString 
        { 
            get 
            {
                return Sex == "M" ? "Male" : "Female";
            } 
        }
    }
}
