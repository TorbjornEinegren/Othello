using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplikation
{
    public abstract class PlayerAbstract
    {
        private int color;
        public int _color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        private int tilesRemaining;
        public int _tilesRemaining
        {
            get
            {
                return tilesRemaining;
            }
            set
            {
                tilesRemaining = value;
            }
        }

        private String name;
        public String _name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}
