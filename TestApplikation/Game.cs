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
            //initateMove(4,4, player2);
            //mainWindow.printBox(mainWindow.TextBlock1);
        }

        private void startingCurrentPlayer(PlayerAbstract startingPlayer)
        {
            currentPlayer = startingPlayer;
        }

        public void setCurrentPlayer(int color)
        {
            if (color == 1)
            {
                player1 = new Human("Fritjof", 1);
                player2 = new AI(2);
                startingCurrentPlayer(player1);
            }
            else
            {
                player1 = new Human("Fritjof", 2);
                player2 = new AI(1);
                startingCurrentPlayer(player2);
            }
        }

        public void initateMove(int row, int column, PlayerAbstract player)
        {
            Console.WriteLine("initateMove: row " + row + " column " + column);
            if (row <= 7 && column <= 7)
            {
                rulesEngine.makeMove(row, column, player._color);
            }
        }
    }
}
