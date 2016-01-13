using System;

namespace TestApplikation
{
    public class Game
    {
        public RulesEngine rulesEngine;
        public PlayerAbstract player1;
        public PlayerAbstract player2;
        public PlayerAbstract currentPlayer;
        private Boolean allowMoves, gameEnded = false;

        public Game(RulesEngine rulesEngine)
        {
            this.rulesEngine = rulesEngine;
            allowMoves = true;
        }

        public void setWinState(Boolean gameEnded)
        {
            this.gameEnded = gameEnded;
        }

        private void startingCurrentPlayer(PlayerAbstract startingPlayer)
        {
            currentPlayer = startingPlayer;
            rulesEngine._board.allowMovesAgain += allowMovesAgain;

            Action<String> onPlayerChange = playerChange;
            if (onPlayerChange != null)
            {
                onPlayerChange(currentPlayer._name + " spelar nu och har "
                    + currentPlayer._tilesRemaining + " brickor kvar");
            }

            currentPlayer.doThings(this);
        }

        public void loadGame()
        {
            PlayerAbstract[] loadedPlayers = rulesEngine.linq.loadPlayers();
            Console.WriteLine(loadedPlayers.Length);
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
            rulesEngine._tilesRemaining = rulesEngine._tilesRemaining - player1._tilesRemaining - player2._tilesRemaining; Action<String> onPlayerChange = playerChange;
            if (onPlayerChange != null)
            {
                onPlayerChange(currentPlayer._name + " spelar nu och har "
                    + currentPlayer._tilesRemaining + " brickor kvar");
            }
            currentPlayer.doThings(this);
        }

        public Action<String> playerChange { get; set; }

        public void changeCurrentPlayer()
        {
            if (gameEnded == false)
            {
                if (currentPlayer == player1)
                {
                    currentPlayer = player2;
                }
                else
                {
                    currentPlayer = player1;
                }

                Action<String> onPlayerChange = playerChange;
                if (onPlayerChange != null)
                {
                    onPlayerChange(currentPlayer._name + " spelar nu och har "
                        + currentPlayer._tilesRemaining + " brickor kvar");
                }

                currentPlayer.doThings(this);
            }
        }

        public void allowMovesAgain()
        {
            allowMoves = true;
        }

        public void restartGame()
        {
            gameEnded = true;
            rulesEngine.linq.gameEnd();
            rulesEngine.linq.initBoard();
            rulesEngine._tilesRemaining = 60;
        }

        public void setStartingColor(String colorStr, String playerStr)
        {
            PlayerAbstract tempPlayer = null;
            rulesEngine.linq.initBoard();
            if (playerStr.Equals("AI"))
            {
                player1 = new AI("Dator", "Black");
                player2 = new AI("Dumburk", "White");
                rulesEngine.linq.createPlayers(player1, player2);
                tempPlayer = player1;
            }
            else if (playerStr.Equals("Human"))
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new Human("Människa", "White");
                    player2 = new Human("En annan människa", "Black");
                    rulesEngine.linq.createPlayers(player1, player2);
                    tempPlayer = player2;
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new Human("Människa", "Black");
                    player2 = new Human("En annan människa", "White");
                    rulesEngine.linq.createPlayers(player1, player2);
                    tempPlayer = player1;
                }
            }
            else
            {
                if (colorStr.Equals("light"))
                {
                    player1 = new Human("Människa", "White");
                    player2 = new AI("Dumburk", "Black");
                    rulesEngine.linq.createPlayers(player1, player2);
                    tempPlayer = player2;
                }
                else if (colorStr.Equals("dark"))
                {
                    player1 = new Human("Människa", "Black");
                    player2 = new AI("Dumburk", "White");
                    tempPlayer = player1;
                }
            }
            rulesEngine.linq.createPlayers(player1, player2);
            rulesEngine._board.loadBoard(rulesEngine.linq.loadGame());
            gameEnded = false;
            startingCurrentPlayer(tempPlayer);
        }

        public void initateMove(int row, int column)
        {
            if (gameEnded == false)
            {
                if (allowMoves && rulesEngine._tilesRemaining <= 60)
                {
                    allowMoves = false;
                    rulesEngine.makeMove(row, column, currentPlayer);
                }
            }
        }
    }
}