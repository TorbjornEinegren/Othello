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

        private int move;
        public int _move
        {
            get
            {
                return move;
            }
            set
            {
                move = value;
            }
        }

        public RulesEngine()
        {
            //a counter to keep track of whos turn it is to play
            move = 0;
            //sets the starting positions for the players
            //0 is an empty spot, 1 is white and 2 is black
            board = new Board();
            linq = new LINQ();
            board.setBoardPosition(3, 3, 2);
            board.setBoardPosition(3, 4, 1);
            board.setBoardPosition(4, 3, 1);
            board.setBoardPosition(4, 4, 2);
            board.onBoardChangeLINQ += linq.boardChange;
        }

        private void moveCounter()
        {
            move++;
        }

        private void winState()
        {
            int whiteScore = 0;
            int blackScore = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (board.getBoardPosition(row, column) == 1)
                    {
                        whiteScore++;
                    }
                    else
                    {
                        blackScore++;
                    }
                }
            }
            if (whiteScore > blackScore)
            {

            }
            else
            {

            }
        }

        public Action onRoundFinished { get; set; }

        public Action onMoveFinished { get; set; }

        public Action<String> onBadMove { get; set; }

        public async void forfeitRound()
        {
            moveCounter();
            await Task.Delay(1000);
            Action localOnChange = onRoundFinished;
            if (localOnChange != null)
            {
                localOnChange();
            }
        }
        
        public async void makeMove(int row, int column, int player)
        {
            if (isMoveLegal(row, column, player))
            {
                turnTile(row, column, player);
                board.setBoardPosition(row, column, player);
                moveCounter();
                await Task.Delay(500);
                Action localOnChange = onRoundFinished;
                if (localOnChange != null)
                {
                    localOnChange();
                }
            }
            else
            {
                Action<String> localOnChange = onBadMove;
                if (localOnChange != null)
                {
                    localOnChange("Sorry, you can't make that move");
                }
            }
            Action localOnFinished = onMoveFinished;
            if (localOnFinished != null)
            {
                localOnFinished();
            }
            if (move == 60)
            {
                winState();
            }
        }

        private void turnTile(int row, int column, int player)
        {
            int a = checkHorizontalLeft(row, column, player);
            int b = checkHorizontalRight(row, column, player);
            int c = checkVerticalUp(row, column, player);
            int d = checkVerticalDown(row, column, player);
            int e = checkTopLeft(row, column, player);
            int f = checkTopRight(row, column, player);
            int g = checkBotLeft(row, column, player);
            int h = checkBotRight(row, column, player);
            //fungerar
            turnHorizontalLeft(a, player, row, column);
            turnHorizontalRight(b, player, row, column);
            turnVerticalUp(c, player, row, column);
            turnVerticalDown(d, player, row, column);
            //paj
            turnTopLeft(e, player, row, column);
            turnTopRight(f, player, row, column);
            turnBotLeft(g, player, row, column);
            turnBotRight(h, player, row, column);
        }

        private int checkBotLeft(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                if ((!(row == 7) && !(column == 0)) && board.getBoardPosition(row + 1, column - 1) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    lastRow = i;
                    break;
                }
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
                else
                {
                    break;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnBotLeft(int lastRow, int player, int row, int currentColumn)
        {
            //The actual turning
            for (int i = row; i < lastRow; i++)
            {
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
        }

        private int checkBotRight(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                if ((!(row == 7) && !(column == 7)) && board.getBoardPosition(row + 1, column + 1) == player)
                {
                    break;
                }

                else if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    lastRow = i;
                    lastColumn = currentColumn;
                    break;
                }
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
                else
                {
                    break;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnBotRight(int lastRow, int player, int row, int currentColumn)
        {
            //The actual turning
            for (int i = row; i < lastRow; i++)
            {
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
        }

        private int checkTopLeft(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                if ((!(row == 0) && !(column == 0)) && board.getBoardPosition(row - 1, column - 1) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    lastRow = i;
                    lastColumn = currentColumn;
                    break;
                }
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
                else
                {
                    break;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnTopLeft(int lastRow, int player, int row, int currentColumn)
        {
            //The actual turning
            for (int i = row; i > lastRow; i--)
            {
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
        }

        private int checkTopRight(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                if ((!(row == 0) && !(column == 7)) && board.getBoardPosition(row - 1, column + 1) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, column) == player)
                {
                    lastRow = i;
                    break;
                }
                if (column < 7)
                {
                    column++;
                }
                else
                {
                    break;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnTopRight(int lastRow, int player, int row, int currentColumn)
        {
            for (int i = row; i > lastRow; i--)
            {
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
        }

        private int checkVerticalUp(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                if (!(row == 0) && board.getBoardPosition(row - 1, column) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, column) == player)
                {
                    lastRow = i;
                    break;
                }
                firstLoop = false;
            }
            return lastRow;
        }

        private void turnVerticalUp(int lastRow, int player, int row, int column)
        {
            //The actual turning
            for (int j = row; j > lastRow; j--)
            {
                board.setBoardPosition(j, column, player);
            }
        }

        private int checkVerticalDown(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                if (!(row == 7) && board.getBoardPosition(row + 1, column) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(i, column) == player)
                {
                    lastRow = i;
                    break;
                }
                firstLoop = false;
            }

            return lastRow;
        }

        private void turnVerticalDown(int lastRow, int player, int row, int column)
        {
            //The actual turning
            for (int j = row; j <= lastRow; j++)
            {
                board.setBoardPosition(j, column, player);
            }
        }

        private int checkHorizontalLeft(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = column; i >= 0; i--)
            {
                if ((!(column == 0)) && board.getBoardPosition(row, column - 1) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(row, i) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(row, i) == player)
                {
                    lastColumn = i;
                    break;
                }
                firstLoop = false;
            }
            return lastColumn;
        }

        private void turnHorizontalLeft(int lastColumn, int player, int row, int column)
        {
            //The actual turning
            for (int j = column; j > lastColumn; j--)
            {
                board.setBoardPosition(row, j, player);
            }
        }

        private int checkHorizontalRight(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = column; i < 8; i++)
            {
                if ((!(column == 7)) && board.getBoardPosition(row, column + 1) == player)
                {
                    break;
                }
                else if (board.getBoardPosition(row, i) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                }
                else if (board.getBoardPosition(row, i) == player)
                {
                    lastColumn = i;
                    break;
                }
                firstLoop = false;
            }
            return lastColumn;
        }

        private void turnHorizontalRight(int lastColumn, int player, int row, int column)
        {
            //The actual turning
            for (int j = column; j < lastColumn; j++)
            {
                board.setBoardPosition(row, j, player);
            }
        }

        public bool isMoveLegal(int row, int column, int player)
        {
            bool moveIsLegal = true;
            if (board.getBoardPosition(row, column) != 0)
            {
                moveIsLegal = false;
            }
            else if (!nextToOthers(row, column))
            {
                moveIsLegal = false;
            }
            else if (!(turningTile(row, column, player) > 0))
            {
                moveIsLegal = false;
            }
            return moveIsLegal;
        }

        public int turningTile(int row, int column, int player)
        {
            int turnedTiles = 0;
            turnedTiles += Math.Abs(checkBotLeft(row, column, player) - row);
            turnedTiles += Math.Abs(checkBotRight(row, column, player) - row);
            turnedTiles += Math.Abs(checkHorizontalLeft(row, column, player) - column);
            turnedTiles += Math.Abs(checkHorizontalRight(row, column, player) - column);
            turnedTiles += Math.Abs(checkTopLeft(row, column, player) - row);
            turnedTiles += Math.Abs(checkTopRight(row, column, player) - row);
            turnedTiles += Math.Abs(checkVerticalDown(row, column, player) - row);
            turnedTiles += Math.Abs(checkVerticalUp(row, column, player) - row);
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
                    if (board.getBoardPosition(i, j) != 0)
                    {
                        nextToOther = true;
                    }
                }
            }
            return nextToOther;
        }
    }
}
