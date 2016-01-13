using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplikation
{
    public abstract class PlayerAbstract
    {
        private String color;
        public String _color
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

        private Boolean isAI;
        public Boolean _isAI
        {
            get
            {
                return isAI;
            }
            set
            {
                isAI = value;
            }
        }
        
        public abstract void doThings(Game game);

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
