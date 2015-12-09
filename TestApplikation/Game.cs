using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
            else
            {
                currentPlayer = player1;
            }
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
