using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.ButtonsFolder
{
    public class Button
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
        public string Rubric { get; set; }
        public virtual void Paint()
        {

        }

        public virtual void Click()
        {

        }

    }
}
