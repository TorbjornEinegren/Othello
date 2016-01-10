using System;

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

        public Action<int[]> onBoardChange { get; set; }

        public Action<int[,]> onBoardChangeLINQ { get; set; }

        public void setBoardPosition(int row, int column, int player)
        {
            boardArray[row, column] = player;
            int[] argArr = { row, column };
            // notify Subscribers
            Action<int[]> localOnChange = onBoardChange;
            if (localOnChange != null)
            {
                localOnChange(argArr);
            }
            Action<int[,]> localOnChangeLINQ = onBoardChangeLINQ;
            if (localOnChangeLINQ != null)
            {
                localOnChangeLINQ(boardArray);
            }
        }

    }
}
