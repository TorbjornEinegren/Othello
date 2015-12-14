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
                Console.WriteLine("White won!");
            }
            else
            {
                Console.WriteLine("Black won!");
            }
        }

        public Boolean makeMove(int row, int column, int player)
        {
            Boolean doItAgain = false;
            if (isMoveLegal(row, column))
            {
                board.setBoardPosition(row, column, player);
                Console.WriteLine("makeMove: isLegal");
                turnTile(row, column, player);
                moveCounter();
                if (move >= 60)
                {
                    winState();
                }
                doItAgain = true;
                Console.WriteLine("Nice move!");
            }
            else
            {
                Console.WriteLine("Sorry, you can not make that move!");
            }
            return doItAgain;
        }

        private void turnTile(int row, int column, int player)
        {
            Console.WriteLine("turnTile");
            turnHorizontalLeft(row, column, player);
            turnHorizontalRight(row, column, player);
            turnVerticalUp(row, column, player);
            turnVerticalDown(row, column, player);
            turnTopLeft(row, column, player);
            turnTopRight(row, column, player);
            turnBotLeft(row, column, player);
            turnBotRight(row, column, player);
        }

        private void turnBotLeft(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                Console.WriteLine("turnBotLeft: for i = " + i);
                if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    if (!firstLoop)
                    {
                        lastRow = i;
                        lastColumn = currentColumn;
                        Console.WriteLine("turnBotLeft: lastRow = " + lastRow + " and lastColumn " + lastColumn);
                        break;
                    }
                    firstLoop = false;
                }
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
            //The actual turning
            currentColumn = column;
            for (int i = row; i < lastRow; i++)
            {
                Console.WriteLine("turnBotLeft: for i = " + i);
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
        }

        private void turnBotRight(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                Console.WriteLine("turnBotRight: for i = " + i);
                if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    if (!firstLoop)
                    {
                        lastRow = i;
                        lastColumn = currentColumn;
                        Console.WriteLine("turnBotRight: lastRow = " + lastRow + " and lastColumn " + lastColumn);
                        break;
                    }
                    firstLoop = false;
                }
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
            //The actual turning
            currentColumn = column;
            for (int i = row; i < lastRow; i++)
            {
                Console.WriteLine("turnBotRight: for i = " + i);
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
        }

        private void turnTopLeft(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                Console.WriteLine("turnTopLeft: for i = " + i);
                if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    Console.WriteLine("Skit händer");
                    if (!firstLoop)
                    {
                        lastRow = i;
                        lastColumn = currentColumn;
                        Console.WriteLine("turnTopLeft: lastRow = " + lastRow + " and lastColumn " + lastColumn);
                        break;
                    }
                    firstLoop = false;
                }
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
            //The actual turning
            currentColumn = column;
            for (int i = row; i > lastRow; i--)
            {
                Console.WriteLine("turnTopLeft: for i = " + i);
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
        }

        private void turnTopRight(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            int lastColumn = column;
            int currentColumn = column;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                Console.WriteLine("turnTopRight: for i = " + i + " currentColumn " + currentColumn);
                if (board.getBoardPosition(i, currentColumn) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(i, currentColumn) == player)
                {
                    if (!firstLoop)
                    {
                        lastRow = i;
                        lastColumn = currentColumn;
                        Console.WriteLine("turnTopRight: lastRow = " + lastRow + " and lastColumn " + lastColumn);
                        break;
                    }
                    firstLoop = false;
                }
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
            //The actual turning
            currentColumn = column;
            for (int i = row; i > lastRow; i--)
            {
                Console.WriteLine("turnTopRight: turn for i = " + i);
                board.setBoardPosition(i, currentColumn, player);
                if (currentColumn < 7)
                {
                    currentColumn++;
                }
            }
        }

        private void turnVerticalUp(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            Boolean firstLoop = true;
            for (int i = row; i >= 0; i--)
            {
                Console.WriteLine("turnVerticalUp: for i = " + i);
                if (board.getBoardPosition(i, column) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == player)
                {
                    if (!firstLoop)
                    {
                        lastRow = i;
                        Console.WriteLine("turnVerticalUp: lastRow = " + lastRow);
                        break;
                    }
                    firstLoop = false;
                }
            }
            //The actual turning
            for (int j = row; j > lastRow; j--)
            {
                Console.WriteLine("turnVerticalUp: for j = " + j);
                board.setBoardPosition(j, column, player);
            }
        }

        private void turnVerticalDown(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastRow = row;
            Boolean firstLoop = true;
            for (int i = row; i < 8; i++)
            {
                Console.WriteLine("turnVerticalDown: for i = " + i);
                if (board.getBoardPosition(i, column) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(i, column) == player)
                {
                    if (!firstLoop)
                    {
                        lastRow = i;
                        Console.WriteLine("turnVerticalDown: lastColumn = " + lastRow);
                        break;
                    }
                    firstLoop = false;
                }
            }
            //The actual turning
            for (int j = row; j <= lastRow; j++)
            {
                Console.WriteLine("turnVerticalDown: for j = " + j);
                board.setBoardPosition(j, column, player);
            }
        }

        private void turnHorizontalLeft(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = column; i >= 0; i--)
            {
                Console.WriteLine("turnHorizontalLeft: for i = " + i);
                if (board.getBoardPosition(row, i) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(row, i) == player)
                {
                    if (!firstLoop)
                    {
                        lastColumn = i;
                        Console.WriteLine("turnHorizontalLeft: lastColumn = " + lastColumn);
                        break;
                    }
                    firstLoop = false;
                }
            }
            //The actual turning
            for (int j = column; j > lastColumn; j--)
            {
                Console.WriteLine("turnHorizontalLeft: for j = " + j);
                board.setBoardPosition(row, j, player);
            }
        }

        private void turnHorizontalRight(int row, int column, int player)
        {
            //finds the last tile of the same player and turns the ones in between
            int lastColumn = column;
            Boolean firstLoop = true;
            for (int i = column; i < 8; i++)
            {
                Console.WriteLine("turnHorizontalRight: for i = " + i);
                if (board.getBoardPosition(row, i) == 0)
                {
                    break;
                }
                else if (board.getBoardPosition(row, i) == player)
                {
                    if (!firstLoop)
                    {
                        lastColumn = i;
                        Console.WriteLine("turnHorizontalRight: lastColumn = " + lastColumn);
                        break;
                    }
                    firstLoop = false;
                }
            }
            //The actual turning
            for (int j = column; j < lastColumn; j++)
            {
                Console.WriteLine("turnHorizontalRight: for j = " + j);
                board.setBoardPosition(row, j, player);
            }
        }

        public bool isMoveLegal(int row, int column)
        {
            bool moveIsLegal = true;
            if (board.getBoardPosition(row, column) != 0)
            {
                Console.WriteLine("moveIsLegal: busy spot");
                moveIsLegal = false;
            }
            else if (!nextToOthers(row, column))
            {
                Console.WriteLine("moveIsLegal: not next to other");
                moveIsLegal = false;
            }
            Console.WriteLine("moveIsLegal " + moveIsLegal);
            return moveIsLegal;
        }

        private bool nextToOthers(int row, int column)
        {
            bool nextToOther = false;
            int maxRow = row + 1;
            int minRow = row - 1;
            int maxColumn = column + 1;
            int minColumn = column - 1;
            Console.WriteLine("nextToOther: is starting");

            //So that we don't look for legal moves outside of the board
            if (row == 0)
            {
                minRow = row;
                Console.WriteLine("nextToOther: minI=i");
            }
            else if (row == 7)
            {
                maxRow = row;
                Console.WriteLine("nextToOther: maxI=i");
            }
            if (column == 0)
            {
                minColumn = column;
                Console.WriteLine("nextToOther: minJ=j");
            }
            else if (column == 7)
            {
                maxColumn = column;
                Console.WriteLine("nextToOther: maxJ=j");
            }

            for (int i = minRow; i <= maxRow; i++)
            {
                Console.WriteLine("nextToOther: first loop");
                for (int j = minColumn; j <= maxColumn; j++)
                {
                    Console.WriteLine("nextToOther: second loop");
                    Console.WriteLine("nextToOther: i:" + i + " j:" + j);
                    if (board.getBoardPosition(i, j) != 0)
                    {
                        nextToOther = true;
                        Console.WriteLine("nextToOther: position: i:" + i + " j:" + j);
                    }
                }
            }
            return nextToOther;
        }
    }
}
