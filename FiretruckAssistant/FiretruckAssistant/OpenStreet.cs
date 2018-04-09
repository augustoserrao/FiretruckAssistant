using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiretruckAssistant
{
    class OpenStreet
    {
        public int FirstCorner { get; set; }
        public int SecondCorner { get; set; }

        public void Reset()
        {
            FirstCorner = 0;
            SecondCorner = 0;
        }
    }
}
