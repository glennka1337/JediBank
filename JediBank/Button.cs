using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    class Button
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public string Text {  get; set; }

        public virtual void Paint()
        {

        }

    }
}
