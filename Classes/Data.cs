using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Classes
{
    public class Data
    {
        public List<Country> country { get; set; }
        public List<League> league { get; set; }
        public List<Match> match { get; set; }
        public List<Table> table { get; set; }


        public List<Fixture> fixture { get; set; }
    }
}
