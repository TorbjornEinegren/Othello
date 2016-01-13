using System;

namespace TestApplikation
{
    public class Human : PlayerAbstract
    {
        public Human(String name, int color)
        {
            this._name = name;
            this._tilesRemaining = 30;
            this._color = color;
            this._isAI = false;
        }
        
        public override void doThings(Game game)
        {
            Action<String> localOnChange = onPlayerChange;
            if (localOnChange != null)
            {
                localOnChange(_name + " spelar nu och har " + _tilesRemaining + " brickor kvar");
            }
            if (_tilesRemaining > 0)
            {
                _tilesRemaining--;
            }
            game.rulesEngine.linq.updateTilesRemaining(this);
        }
    }
}
