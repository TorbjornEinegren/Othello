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

        public async override void doThings(Game game)
        {
            await System.Threading.Tasks.Task.Delay(500);
            Action<String> localOnChange = onPlayerChange;
            if (localOnChange != null)
            {
                localOnChange(_name + " spelar nu och har " + _tilesRemaining + " brickor kvar");
            }
            if (_tilesRemaining > 0)
            {
                _tilesRemaining--;
                AILogic(game);
            }
        }

        private void AILogic(Game game)
        {
            int bestRow = 0;
            int bestColumn = 0;
            int bestScore = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (game.rulesEngine.isMoveLegal(row, column, _color))
                    {
                        int tempScore = game.rulesEngine.turningTile(row, column, _color);
                        if (tempScore > bestScore)
                        {
                            bestScore = tempScore;
                            bestRow = row;
                            bestColumn = column;
                        }
                    }
                }
            }
            if (bestScore > 1)
            {
                game.initateMove(bestRow, bestColumn);
            }
            else
            {
                game.rulesEngine.forfeitRound();
            }
        }
    }
}
