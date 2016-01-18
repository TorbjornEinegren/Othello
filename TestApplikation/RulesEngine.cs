using System;
using System.Threading.Tasks;

namespace TestApplikation
{
    public class RulesEngine
    {
        public LINQ linq;
        private Board board;
        public Board _board
        {
            get
            {
                return board;
            }
        }

        private int tilesRemaining;
        public int roundsLeft
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

        public RulesEngine()
        {
            roundsLeft = 60;
            board = new Board();
            linq = new LINQ(this);
            onBoardChanged += linq.boardChange;
        }

        private void moveCounter()
        {
            roundsLeft--;
        }

        private void winState()
        {
            Action<Boolean> winState = onWinState;
            if (winState != null)
            {
                winState(true);
            }

            int whiteScore = 0;
            int blackScore = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if ("White".Equals(board.getBoardPosition(row, column)))
                    {
                        whiteScore++;
                    }
                    else if ("Black".Equals(board.getBoardPosition(row, column)))
                    {
                        blackScore++;
                    }
                    else
                    {
                        Console.WriteLine("This should rarely happen");
                    }
                }
            }
            if (whiteScore > blackScore)
            {
                Action<String> onWinState = onWin;
                if (onWinState != null)
                {
                    onWinState("White won against Black with " + whiteScore + " tiles against " + blackScore);
                }
            }
            else
            {
                Action<String> onWinState = onWin;
                if (onWinState != null)
                {
                    onWinState("Black won against White with " + blackScore + " tiles against " + whiteScore);
                }

            }
        }

        public Action onRoundFinished { get; set; }

        public Action<String[,]> onBoardChanged { get; set; }

        public Action<Boolean> onWinState { get; set; }

        public Action<String> onWin { get; set; }

        public Action onMoveFinished { get; set; }

        public Action<String> onMoveFeedback { get; set; }

        public void forfeitRound()
        {
            moveCounter();

            if (roundsLeft == 0)
            {
                winState();
            }
            Action<String[,]> onBoardChangedLINQ = onBoardChanged;
            if (onBoardChangedLINQ != null)
            {
                onBoardChanged(_board._boardArray);
            }
            Action localOnChange = onRoundFinished;
            if (localOnChange != null)
            {
                localOnChange();
            }
        }

        public void playMade(int row, int column, PlayerAbstract currentPlayer)
        {
            String playerColor = currentPlayer._color;
            if (isMoveLegal(row, column, playerColor))
            {
                linq.updateTilesRemaining(currentPlayer);
                makeMove(row, column, playerColor);

                Action<String[,]> onBoardChangedLINQ = onBoardChanged;
                if (onBoardChangedLINQ != null)
                {
                    onBoardChanged(_board._boardArray);
                }
            }
            else
            {
                Action<String> localOnChange = onMoveFeedback;
                if (localOnChange != null)
                {
                    localOnChange("You can't make that move");
                }
            }
            Action localOnFinished = onMoveFinished;
            if (localOnFinished != null)
            {
                localOnFinished();
            }
        }

        private async void makeMove(int i, int j, String playerColor)
        {
            turnTile(i, j, playerColor);
            board.setBoardPosition(i, j, playerColor);
            moveCounter();
            await Task.Delay(90);
            if (roundsLeft == 0)
            {
                winState();
            }
            Action localOnChange = onRoundFinished;
            if (localOnChange != null)
            {
                localOnChange();
            }
        }

        private void turnTile(int row, int column, String playerColor)
        {
            int a = checkHorizontalLeft(row, column, playerColor);
            int b = checkHorizontalRight(row, column, playerColor);
            int c = checkVerticalUp(row, column, playerColor);
            int d = checkVerticalDown(row, column, playerColor);
            int e = checkDirection(row, column, playerColor, -1, -1);
            int f = checkDirection(row, column, playerColor, -1, 1);
            int g = checkDirection(row, column, playerColor, 1, -1);
            int h = checkDirection(row, column, playerColor, 1, 1);
            turnHorizontalLeft(a, playerColor, row, column);
            turnHorizontalRight(b, playerColor, row, column);
            turnVerticalUp(c, playerColor, row, column);
            turnVerticalDown(d, playerColor, row, column);
            turnTopLeft(e, playerColor, row, column);
            turnTopRight(f, playerColor, row, column);
            turnBotLeft(g, playerColor, row, column);
            turnBotRight(h, playerColor, row, column);
        }

        private int checkDirection(int inputRow, int inputColumn, String playerColor, int rowDirection, int columnDirection)
        {
            if ((inputColumn == 7 && columnDirection == 1) || (inputColumn == 0 && columnDirection == -1)
                || (inputRow == 7 && rowDirection == 1) || (inputRow == 0 && rowDirection == -1)
                || (playerColor.Equals(board.getBoardPosition(inputRow + rowDirection, inputColumn + columnDirection))))
            {
                return inputRow;
            }

            int lastRow = inputRow;
            int currentColumn = inputColumn;

            for (int currentRow = inputRow; currentRow < 8 && currentRow >= 0; currentRow = currentRow + rowDirection)
            {
                if (board.getBoardPosition(currentRow, currentColumn) == null)
                {
                    if ((currentColumn != inputColumn) && (currentRow != inputRow))
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(currentRow, currentColumn).Equals(playerColor))
                {
                    lastRow = currentRow;
                    break;
                }
                if ((currentColumn >= 7 && columnDirection == 1) || (currentColumn <= 0 && columnDirection == -1))
                {
                    break;
                }
                currentColumn = currentColumn + columnDirection;
            }

            return lastRow;
            /*do {
                Console.WriteLine(currentRow + " - " + currentColumn);
                if (board.getBoardPosition(currentRow, currentColumn) == null)
                {
                    if ((currentColumn != inputColumn) && (currentRow != inputRow))
                    {
                        return currentRow;
                    }
                }
                else if (board.getBoardPosition(currentRow, currentColumn).Equals(playerColor))
                {
                    return currentRow;
                }
                currentRow = currentRow + rowDirection;
                currentColumn = currentColumn + columnDirection;
            } while ((currentColumn >= 7 && columnDirection == 1) || (currentColumn <= 0 && columnDirection == -1) 
            || (currentRow >= 7 && rowDirection == 1) || (currentRow <= 0 && rowDirection == -1));
            return currentRow;*/
        }

        private int checkBotLeft(int row, int column, String playerColor)
        {
            if (!(row == 7 || column == 0) && playerColor.Equals(board.getBoardPosition(row + 1, column - 1)))
            {
                return row;
            }
            int lastRow = row;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {

                if (board.getBoardPosition(i, currentColumn) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn).Equals(playerColor))
                {
                    lastRow = i;
                    break;
                }
                if (currentColumn == 0 || i == 7)
                {
                    break;
                }
                else
                {
                    currentColumn--;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnBotLeft(int lastRow, String playerColor, int row, int currentColumn)
        {
            for (int i = row; i < lastRow; i++)
            {
                board.setBoardPosition(i, currentColumn, playerColor);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
        }

        private int checkBotRight(int row, int column, String playerColor)
        {
            if (!(row == 7 || column == 7) && playerColor.Equals(board.getBoardPosition(row + 1, column + 1)))
            {
                return row;
            }
            int lastRow = row;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                if (board.getBoardPosition(i, currentColumn) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn).Equals(playerColor))
                {
                    lastRow = i;
                    break;
                }
                if (currentColumn == 7 || i == 7)
                {
                    break;
                }
                else
                {
                    currentColumn++;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnBotRight(int lastRow, String playerColor, int row, int currentColumn)
        {
            for (int i = row; i < lastRow; i++)
            {
                board.setBoardPosition(i, currentColumn, playerColor);
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
        }

        private int checkTopLeft(int row, int column, String playerColor)
        {
            if (!(row == 0 || column == 0) && playerColor.Equals(board.getBoardPosition(row - 1, column - 1)))
            {
                return row;
            }
            int lastRow = row;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                if (board.getBoardPosition(i, currentColumn) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn).Equals(playerColor))
                {
                    lastRow = i;
                    break;
                }
                if (currentColumn == 0 || i == 0)
                {
                    break;
                }
                else
                {
                    currentColumn--;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnTopLeft(int lastRow, String playerColor, int row, int currentColumn)
        {
            //The actual turning
            for (int i = row; i > lastRow; i--)
            {
                board.setBoardPosition(i, currentColumn, playerColor);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
        }

        private int checkTopRight(int row, int column, String playerColor)
        {
            if (!(row == 0 || column == 7) && playerColor.Equals(board.getBoardPosition(row - 1, column + 1)))
            {
                return row;
            }
            int lastRow = row;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                if (board.getBoardPosition(i, currentColumn) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn).Equals(playerColor))
                {
                    lastRow = i;
                    break;
                }
                if (currentColumn == 7 || i == 0)
                {
                    break;
                }
                else
                {
                    currentColumn++;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnTopRight(int lastRow, String playerColor, int row, int currentColumn)
        {
            for (int i = row; i > lastRow; i--)
            {
                board.setBoardPosition(i, currentColumn, playerColor);
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
        }

        private int checkVerticalUp(int row, int column, String playerColor)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                if (!(row == 0) && playerColor.Equals(board.getBoardPosition(row - 1, column)))
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, column).Equals(playerColor))
                {
                    lastRow = i;
                    break;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnVerticalUp(int lastRow, String playerColor, int row, int column)
        {
            //The actual turning
            for (int j = row; j > lastRow; j--)
            {
                board.setBoardPosition(j, column, playerColor);
            }
        }

        private int checkVerticalDown(int row, int column, String playerColor)
        {
            int lastRow = row;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                if (!(row == 7) && playerColor.Equals(board.getBoardPosition(row + 1, column)))
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, column).Equals(playerColor))
                {
                    lastRow = i;
                    break;
                }
                firstLoop = false;
            }

            return lastRow;
        }

        private void turnVerticalDown(int lastRow, String playerColor, int row, int column)
        {
            //The actual turning
            for (int j = row; j <= lastRow; j++)
            {
                board.setBoardPosition(j, column, playerColor);
            }
        }

        private int checkHorizontalLeft(int row, int column, String playerColor)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = column; i >= 0; i--)
            {
                if ((!(column == 0)) && playerColor.Equals(board.getBoardPosition(row, column - 1)))
                {
                    break;
                }
                else if (board.getBoardPosition(row, i) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(row, i).Equals(playerColor))
                {
                    lastColumn = i;
                    break;
                }
                firstLoop = false;
            }
            return lastColumn;
        }

        private void turnHorizontalLeft(int lastColumn, String playerColor, int row, int column)
        {
            //The actual turning
            for (int j = column; j > lastColumn; j--)
            {
                board.setBoardPosition(row, j, playerColor);
            }
        }

        private int checkHorizontalRight(int row, int column, String playerColor)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = column; i < 8; i++)
            {
                if ((!(column == 7)) && playerColor.Equals(board.getBoardPosition(row, column + 1)))
                {
                    break;
                }
                else if (board.getBoardPosition(row, i) == null)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(row, i).Equals(playerColor))
                {
                    lastColumn = i;
                    break;
                }
                firstLoop = false;
            }
            return lastColumn;
        }

        private void turnHorizontalRight(int lastColumn, String playerColor, int row, int column)
        {
            //The actual turning
            for (int j = column; j < lastColumn; j++)
            {
                board.setBoardPosition(row, j, playerColor);
            }
        }

        public bool isMoveLegal(int row, int column, String playerColor)
        {
            bool moveIsLegal = true;
            if (board.getBoardPosition(row, column) != null)
            {
                moveIsLegal = false;
            }
            else if (!nextToOthers(row, column))
            {
                moveIsLegal = false;
            }
            else if (!(turningTile(row, column, playerColor) > 1))
            {
                moveIsLegal = false;
            }
            return moveIsLegal;
        }

        public int turningTile(int row, int column, String playerColor)
        {
            int turnedTiles = 0;
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 1, -1) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 1, 1) - row);
            turnedTiles += Math.Abs(checkHorizontalLeft(row, column, playerColor) - column);
            turnedTiles += Math.Abs(checkHorizontalRight(row, column, playerColor) - column);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, -1, -1) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, -1, 1) - row);
            turnedTiles += Math.Abs(checkVerticalDown(row, column, playerColor) - row);
            turnedTiles += Math.Abs(checkVerticalUp(row, column, playerColor) - row);
            return turnedTiles;
        }

        private bool nextToOthers(int row, int column)
        {
            bool nextToOther = false;
            int maxRow = row + 1;
            int minRow = row - 1;
            int maxColumn = column + 1;
            int minColumn = column - 1;

            //So that we don't look for legal moves outside of the board
            if (row == 0)
            {
                minRow = row;
            }
            else if (row == 7)
            {
                maxRow = row;
            }
            if (column == 0)
            {
                minColumn = column;
            }
            else if (column == 7)
            {
                maxColumn = column;
            }

            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minColumn; j <= maxColumn; j++)
                {
                    if (board.getBoardPosition(i, j) != null)
                    {
                        nextToOther = true;
                    }
                }
            }
            return nextToOther;
        }
    }
}
