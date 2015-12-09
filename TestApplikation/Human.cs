using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplikation
{
    public class Human : PlayerAbstract
    {
        public Human(String name, int color)
        {
            this._name = name;
            this._tilesRemaining = 30;
            this._color = color;
        }
    }
}
