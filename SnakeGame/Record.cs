using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGAme2
{
    public class Record
    {
        public string Name { get; set; }
        public int points { get; set; }

        public Record(string name, int points)
        {
            Name = name;
            this.points = points;
        }
    }
}
