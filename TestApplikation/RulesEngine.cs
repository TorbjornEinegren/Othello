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
            onBoardChanged += linq.updateBoardOnChange;
        }

        private void moveCounter()
        {
            roundsLeft--;
        }

        private void winState()
        {
            //If the game has ended, locks the game from doing additional moves
            //Counts the score of both players
            //Pushes a string with information about who won to the UI
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
                        //If a player couldn't place a tile, this should happen
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
            //If a player can't make a move due to the other tiles placement
            //An event pushes the turn to the other player
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
            //Checks if the suggested move if allowed
            //If allowed the it updates the tiles remaining for the player making the move, both in XML and in the playerobject
            //It then calls the method makeMove and pushes an event to the LINQ class telling it to save the new board positions to XML
            //If not allowed it pushes an event to the UI telling the player that the move is impossible
            String playerColor = currentPlayer._color;
            if (isMoveLegal(row, column, playerColor))
            {
                linq.playerNotDoingThings = false;
                linq.updateTilesRemaining(currentPlayer);
                makeMove(row, column, playerColor);

                Action<String[,]> onBoardChangedLINQ = onBoardChanged;
                if (onBoardChangedLINQ != null)
                {
                    onBoardChanged(_board._boardArray);
                }
                linq.playerNotDoingThings = true;
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
            //It calls the method for checking all directions and turning tiles first
            //Then it sets the position on the board to the selected coordinates
            //It updates the turns left on the game and then waits a short while
            //Checks if a player has won
            //Pushes an event to the gameengine telling it to start the round for the next player
            checkAndTurnTiles(i, j, playerColor);
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

        private void checkAndTurnTiles(int row, int column, String playerColor)
        {
            //saves a variable as the result from the checkDirection won't be the same after the first tile have been placed
            
            //Checks horizontally left
            int a = checkDirection(row, column, playerColor, 0, -1);
            //Checks horizontally right
            int b = checkDirection(row, column, playerColor, 0, 1);
            //Checks vertical up
            int c = checkDirection(row, column, playerColor, -1, 0);
            //Checks vertical down
            int d = checkDirection(row, column, playerColor, 1, 0);
            //Checks towards the top-left corner
            int e = checkDirection(row, column, playerColor, -1, -1);
            //Checks towards the top-right corner
            int f = checkDirection(row, column, playerColor, -1, 1);
            //Checks towards the bottom-left corner
            int g = checkDirection(row, column, playerColor, 1, -1);
            //Checks towards the bottom-right corner
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
            //First checks so that the first move won't go out of bounds or if the first tile it will find is the same color
            //As the method is universal we have to check what value to return as the turning needs column when turning horizontally
            //Loops as long as it is within the gameboard (0-7)
            //Unless it finds a tile of the same color it will return the original row and column coordinates
            //If it finds one of the same color it will return that row or column depending on direction it is checking in
            //As this method is used by the AI to count the best place to place the tile, and that the selected coordinate yet has 
            //to place a tile on it we need to skip that return during the first loop
            if ((inputColumn == 7 && columnDirection == 1) || (inputColumn == 0 && columnDirection == -1)
                || (inputRow == 7 && rowDirection == 1) || (inputRow == 0 && rowDirection == -1)
                || (playerColor.Equals(board.getBoardPosition(inputRow + rowDirection, inputColumn + columnDirection))))
            {
                if (rowDirection == 0)
                {
                    return inputColumn;
                }
                else
                {
                    return inputRow;
                }
            }

            int lastRow = inputRow;
            int lastColumn = inputColumn;
            int currentColumn = inputColumn;
            bool firstLoop = true;

            for (int currentRow = inputRow; currentRow < 8 && currentRow >= 0; currentRow = currentRow + rowDirection)
            {
                if (board.getBoardPosition(currentRow, currentColumn) == null)
                {
                    if (!firstLoop)
                    {
                        if (rowDirection == 0)
                        {
                            return inputColumn;
                        }
                        else
                        {
                            return inputRow;
                        }
                    }
                }
                else if (board.getBoardPosition(currentRow, currentColumn).Equals(playerColor))
                {
                    if (rowDirection == 0)
                    {
                        return currentColumn;
                    }
                    else
                    {
                        return currentRow;
                    }
                }
                if ((currentColumn >= 7 && columnDirection == 1) || (currentColumn <= 0 && columnDirection == -1))
                {
                    if (rowDirection == 0)
                    {
                        return inputColumn;
                    }
                    else
                    {
                        return inputRow;
                    }
                }
                currentColumn = currentColumn + columnDirection;
                firstLoop = false;
            }
            if (rowDirection == 0)
            {
                return inputColumn;
            }
            else
            {
                return inputRow;
            }
        }

        //Methods for turning, lastRow/lastColumn is the coordinate that checkDirection finds
        
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

        private void turnTopLeft(int lastRow, String playerColor, int row, int currentColumn)
        {
            for (int i = row; i > lastRow; i--)
            {
                board.setBoardPosition(i, currentColumn, playerColor);
                if (currentColumn > 0)
                {
                    currentColumn--;
                }
            }
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

        private void turnVerticalUp(int lastRow, String playerColor, int row, int column)
        {
            for (int j = row; j > lastRow; j--)
            {
                board.setBoardPosition(j, column, playerColor);
            }
        }

        private void turnVerticalDown(int lastRow, String playerColor, int row, int column)
        {
            for (int j = row; j <= lastRow; j++)
            {
                board.setBoardPosition(j, column, playerColor);
            }
        }

        private void turnHorizontalLeft(int lastColumn, String playerColor, int row, int column)
        {
            for (int j = column; j > lastColumn; j--)
            {
                board.setBoardPosition(row, j, playerColor);
            }
        }

        private void turnHorizontalRight(int lastColumn, String playerColor, int row, int column)
        {
            for (int j = column; j < lastColumn; j++)
            {
                board.setBoardPosition(row, j, playerColor);
            }
        }

        public bool isMoveLegal(int row, int column, String playerColor)
        {
            //Checks if the move is legal through several other methods
            //aiTileTurningCounter
            //Returns true if move is legal
            bool moveIsLegal = true;
            if (board.getBoardPosition(row, column) != null)
            {
                moveIsLegal = false;
            }
            else if (!nextToOthers(row, column))
            {
                moveIsLegal = false;
            }
            else if (!(aiTileTurningCounter(row, column, playerColor) > 1))
            {
                moveIsLegal = false;
            }
            return moveIsLegal;
        }

        public int aiTileTurningCounter(int row, int column, String playerColor)
        {
            //Counts how many tiles would be turned if the move to the suggested coordinates was made
            int turnedTiles = 0;
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 1, -1) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 1, 1) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 0, -1) - column);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 0, 1) - column);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, -1, -1) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, -1, 1) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, 1, 0) - row);
            turnedTiles += Math.Abs(checkDirection(row, column, playerColor, -1, 0) - row);
            return turnedTiles;
        }

        private bool nextToOthers(int row, int column)
        {
            //Checks so that the suggested tile placement is next to another tile
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
