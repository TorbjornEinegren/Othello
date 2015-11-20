using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplikation
{
    class AI : PlayerAbstract
    {
        public AI(int color)
        {
            this._name = "Dumburk";
            this._tilesRemaining = 32;
            this._color = color;
        }
    }
}
