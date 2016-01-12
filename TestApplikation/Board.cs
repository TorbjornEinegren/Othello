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

        public void loadBoard(int[,] loadedBoard)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    setBoardPosition(i, j, loadedBoard[i, j]);
                }
            }
        }

        public Action<int[]> onBoardChange { get; set; }

        public Action<int[]> onBoardChangeLINQ { get; set; }

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

            int[] argArrLINQ = { row, column, player };
            Action<int[]> localOnChangeLINQ = onBoardChangeLINQ;
            if (localOnChangeLINQ != null)
            {
                localOnChangeLINQ(argArrLINQ);
            }
        }

    }
}
