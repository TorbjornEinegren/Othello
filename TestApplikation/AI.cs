using System;

namespace TestApplikation
{
    class AI : PlayerAbstract
    {
        public AI(int color)
        {
            this._name = "Dumburk";
            this._tilesRemaining = 30;
            this._color = color;
            this._isAI = true;
        }

        public override void doThings(Game game)
        {
            Console.WriteLine(_name + " spelar nu och har " + _tilesRemaining + " brickor kvar");
            AILogic(game);
        }

        private void AILogic(Game game)
        {
            
        }
    }
}
