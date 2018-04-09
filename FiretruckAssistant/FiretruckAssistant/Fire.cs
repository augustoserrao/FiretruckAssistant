using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiretruckAssistant
{
    class Fire
    {
        private List<string> routeList = new List<string>();
        private string firefighter = "";

        public int ID { get; set; }
        public int Location { get; set; }
        public int StatusID { get; set; }

        public string Firefighter
        {
            get { return firefighter; }
            set { firefighter = value; }
        }

        public List<string> RouteList
        {
            get { return routeList; }
            set { NumberOfRoutes = value.Count(); routeList = value; }
        }

        public int NumberOfRoutes { get; set; }

        public enum StatusList {
            URGENT = 1,
            PROCESSING,
            FINISHED
        }

        public void Reset()
        {
            ID = 0;
            Location = 0;
            StatusID = 0;
            Firefighter = "";
            RouteList = null;
            NumberOfRoutes = 0;
        }
    }
}
