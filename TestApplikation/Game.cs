using System;

namespace TestApplikation
{
    public class Game
    {
        public RulesEngine rulesEngine;
        public PlayerAbstract player1;
        public PlayerAbstract player2;
        public PlayerAbstract currentPlayer;
        private MainWindow mainWindow;
        private Boolean allowMoves;

        public Game(MainWindow mainWindow, RulesEngine rulesEngine)
        {
            this.mainWindow = mainWindow;
            this.rulesEngine = rulesEngine;
            allowMoves = true;
        }

        private void startingCurrentPlayer(PlayerAbstract startingPlayer)
        {
            currentPlayer = startingPlayer;
            currentPlayer.onPlayerChange += mainWindow.textChange;
            currentPlayer.doThings(this);
        }

        public void loadGame()
        {
            PlayerAbstract[] loadedPlayers = rulesEngine.linq.loadPlayers();

            player1 = loadedPlayers[0];
            player2 = loadedPlayers[1];

            if (player1._tilesRemaining > player2._tilesRemaining)
            {
                currentPlayer = player2;
            }
            else
            {
                currentPlayer = player1;
            }
            rulesEngine._board.loadBoard(rulesEngine.linq.loadGame());

            currentPlayer.doThings(this);
        }

        public void changeCurrentPlayer()
        {
            if (currentPlayer == player1)
            {
                currentPlayer = player2;
            }
            else
            {
                currentPlayer = player1;
            }
            allowMovesAgain();
            currentPlayer.doThings(this);
        }

        public void allowMovesAgain()
        {
            allowMoves = true;
        }

        public void setStartingColor(String colorStr, String playerStr)
        {
            rulesEngine.linq.initBoard();
            if (playerStr.Equals("AI"))
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new AI("Dator", 1);
                    player2 = new AI("Dumburk", 2);
                    rulesEngine.linq.createPlayers(player1, player2);
                    startingCurrentPlayer(player2);
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new AI("Dator", 2);
                    player2 = new AI("Dumburk", 1);
                    rulesEngine.linq.createPlayers(player1, player2);
                    startingCurrentPlayer(player1);
                }
            }
            else if (playerStr.Equals("Human"))
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new Human("Människa", 1);
                    player2 = new Human("En annan människa", 2);
                    rulesEngine.linq.createPlayers(player1, player2);
                    startingCurrentPlayer(player2);
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new Human("Människa", 2);
                    player2 = new Human("En annan människa", 1);
                    rulesEngine.linq.createPlayers(player1, player2);
                    startingCurrentPlayer(player1);
                }
            }
            else
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new Human("Människa", 1);
                    player2 = new AI("Dumburk", 2);
                    rulesEngine.linq.createPlayers(player1, player2);
                    startingCurrentPlayer(player2);
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new Human("Människa", 2);
                    player2 = new AI("Dumburk", 1);
                    startingCurrentPlayer(player1);
                }
            }
            rulesEngine.linq.createPlayers(player1, player2);
            rulesEngine._board.loadBoard(rulesEngine.linq.loadGame());

        }

        public void initateMove(int row, int column)
        {
            if (allowMoves && rulesEngine._move <= 60)
            {
                allowMoves = false;
                rulesEngine.makeMove(row, column, currentPlayer._color);
            }
        }
    }
}