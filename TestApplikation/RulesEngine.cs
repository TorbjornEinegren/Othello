using System;

namespace TestApplikation
{
    public class RulesEngine
    {
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
            board.setBoardPosition(3, 3, 1);
            board.setBoardPosition(3, 4, 2);
            board.setBoardPosition(4, 3, 2);
            board.setBoardPosition(4, 4, 1);
            //testvalues for turning tiles
            /*board.setBoardPosition(3, 3, 1);
            board.setBoardPosition(3, 4, 1);
            board.setBoardPosition(3, 5, 1);
            board.setBoardPosition(4, 3, 1);
            board.setBoardPosition(4, 5, 1);
            board.setBoardPosition(5, 3, 1);
            board.setBoardPosition(5, 4, 1);
            board.setBoardPosition(5, 5, 1);
            board.setBoardPosition(2, 2, 2);
            board.setBoardPosition(2, 4, 2);
            board.setBoardPosition(2, 6, 2);
            board.setBoardPosition(4, 2, 2);
            board.setBoardPosition(4, 6, 2);
            board.setBoardPosition(6, 2, 2);
            board.setBoardPosition(6, 4, 2);
            board.setBoardPosition(6, 6, 2);*/
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

        public Boolean makeMove(int row, int column, int player)
        {
            Boolean doItAgain = false;
            if (isMoveLegal(row, column, player))
            {
                turnTile(row, column, player);
                board.setBoardPosition(row, column, player);
                moveCounter();
                if (move >= 60)
                {
                    winState();
                }
                doItAgain = true;
            }
            else
            {
                Console.WriteLine("Sorry, you can not make that move!");
            }
            return doItAgain;
        }

        private void turnTile(int row, int column, int player)
        {
            //fungerar
            turnHorizontalLeft(checkHorizontalLeft(row, column, player), player, row, column);
            turnHorizontalRight(checkHorizontalRight(row, column, player), player,row,column);
            turnVerticalUp(checkVerticalUp(row, column, player),player,row,column);
            turnVerticalDown(checkVerticalDown(row, column, player),player,row,column);
            //paj
            turnTopLeft(checkTopLeft(row, column, player),player,row,column);
            turnTopRight(checkTopRight(row, column, player),player,row,column);
            turnBotLeft(checkBotLeft(row, column, player), player, row, column);
            turnBotRight(checkBotRight(row, column, player),player,row,column);
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
                    Console.WriteLine("Break1");
                        break;
                    }
                
                else if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    if (!firstLoop)
                    {
                        Console.WriteLine("Break2");
                        break;
                    }
                    firstLoop = false;
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    Console.WriteLine("Break3");
                    lastRow = i;
                    break;
                }
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
            Console.WriteLine("RETURNING THIS VALUE " + lastRow);
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
                    firstLoop = false;
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
                    firstLoop = false;
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
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                    if ((!(row == 0) && !(column == 7)) && board.getBoardPosition(row - 1, column + 1) == player)
                    {
                        break;
                    
                }
                else if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    if (!firstLoop)
                    {
                        break;
                    }
                    firstLoop = false;
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    lastRow = i;
                    Console.WriteLine("Break3");
                    break;
                }
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }

            Console.WriteLine("DETTA VÄRDE RETURNERAS " + lastRow);
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
                    firstLoop = false;
                }
                else if (board.getBoardPosition(i, column) == player)
                {

                    Console.WriteLine("VERTICAL UP" + i);
                    lastRow = i;
                    break;
                }
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
                        firstLoop = false;
                    }
                    else if (board.getBoardPosition(i, column) == player)
                    {
                        lastRow = i;
                        break;
                }
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
                    firstLoop = false;
                }
                else if (board.getBoardPosition(row, i) == player)
                {
                    lastColumn = i;
                    break;
                }
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
                    firstLoop = false;
                }
                else if (board.getBoardPosition(row, i) == player)
                {
                    lastColumn = i;
                    break;
                }
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
            else if(!(turningTile(row, column, player) > 0))
            {
                moveIsLegal = false;
                Console.WriteLine("Jonas förstör spelet");
            }
            return moveIsLegal;
        }

        public int turningTile(int row, int column, int player)
        {
            int turnedTiles = 0;
            turnedTiles += Math.Abs(checkBotLeft(row,column,player) - row);
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
