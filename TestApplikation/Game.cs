using System;

namespace TestApplikation
{
    public class Game
    {
        public RulesEngine rulesEngine;
        public  PlayerAbstract player1;
        public  PlayerAbstract player2;
        public PlayerAbstract currentPlayer;
        private MainWindow mainWindow;

        public Game(MainWindow mainWindow, RulesEngine rulesEngine)
        {
            this.mainWindow = mainWindow;
            this.rulesEngine = rulesEngine;
        }

        private void startingCurrentPlayer(PlayerAbstract startingPlayer)
        {
            currentPlayer = startingPlayer;
        }

        private void changeCurrentPlayer()
        {
            if(currentPlayer == player1)
            {
                currentPlayer = player2;
                if (currentPlayer._isAI)
                {
                    currentPlayer.doThings(this);
                }
                else
                {
                    currentPlayer.doThings(this);
                }
            }
            else
            {
                currentPlayer = player1;
                if (currentPlayer._isAI)
                {
                    currentPlayer.doThings(this);
                }
                else
                {
                    currentPlayer.doThings(this);
                }
            }
        }

        public String playerStringBuilder()
        {
            String color = "";
            if(currentPlayer._color == 1)
            {
                color = "vit";
            }
            else if (currentPlayer._color == 2)
            {
                color = "svart";
            }
            String returnString = "Det är " + currentPlayer._name + " (" + color + ") tur att placera en bricka ";

            return returnString;
        }

        public void setStartingPlayer(String colorStr)
        {
            if (colorStr.Equals("light"))
            {
                player1 = new Human("Fritjof", 1);
                player2 = new AI(2);
                startingCurrentPlayer(player1);
            }
            else if (colorStr.Equals("dark"))
            {
                player1 = new Human("Fritjof", 2);
                player2 = new AI(1);
                startingCurrentPlayer(player2);
            }
        }

        public void initateMove(int row, int column)
        {
            Boolean doItAgain = false;
            Console.WriteLine("initateMove: row " + row + " column " + column);
            doItAgain = rulesEngine.makeMove(row, column, currentPlayer._color);
            if (doItAgain)
            {
                changeCurrentPlayer();
            }
        }
    }
}
