﻿using System;

namespace TestApplikation
{
    public class Board
    {
        private String[,] boardArray;
        public String[,] _boardArray
        {
            get
            {
                return boardArray;
            }
            set
            {
                boardArray = value;
            }
        }

        public Board()
        {
            boardArray = new String[8, 8];
        }

        public String getBoardPosition(int row, int column)
        {
            return boardArray[row, column];
        }

        public void loadBoard(String[,] loadedBoard)
        {
            boardArray = loadedBoard;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] argArr = { i, j };
                    Action<int[]> onXMLChange = onBoardChange;
                    if (onXMLChange != null)
                    {
                        onXMLChange(argArr);
                    }
                }
            }
        }

        public Action<int[]> onBoardChange { get; set; }

        public Action allowMovesAgain { get; set; }

        public void setBoardPosition(int row, int column, String playerColor)
        {
            boardArray[row, column] = playerColor;

            int[] changedPosition = { row, column };
            Action<int[]> localOnChange = onBoardChange;
            if (localOnChange != null)
            {
                localOnChange(changedPosition);
            }
            Action allowMoves = allowMovesAgain;
            if (allowMoves != null)
            {
                allowMoves();
            }
        }

    }
}
