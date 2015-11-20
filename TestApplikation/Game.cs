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
        private PlayerAbstract player2;
        private MainWindow mainWindow;

        public Game(MainWindow mainWindow, RulesEngine rulesEngine)
        {
            this.mainWindow = mainWindow;
            this.rulesEngine = rulesEngine;
            player1 = new Human("Fritjof", 1);
            player2 = new AI(2);
            //initateMove(4,4, player2);
            mainWindow.printBox(mainWindow.TextBlock1);
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
