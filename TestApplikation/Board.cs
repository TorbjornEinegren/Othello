using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplikation
{
    public class Board
    {
        int[,] boardArray;

        public Board()
        {
            boardArray = new int[8, 8];
        }

        public int getBoardPosition(int row, int column)
        {
            return boardArray[row, column];
        }
        public void setBoardPosition(int row, int column, int player)
        {
            boardArray[row, column] = player;
        }

    }
}
