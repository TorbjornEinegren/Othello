using System;

namespace TestApplikation
{
    public class Human : PlayerAbstract
    {
        public Human(String name, String color)
        {
            this._name = name;
            this._tilesRemaining = 30;
            this._color = color;
            this._isAI = false;
        }
        
        public override void doThings(Game game)
        {
        }
    }
}
