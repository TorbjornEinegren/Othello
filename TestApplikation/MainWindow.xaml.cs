using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApplikation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private RulesEngine rulesEngine;
        public MainWindow()
        {
            InitializeComponent();
            rulesEngine = new RulesEngine();
            game = new Game(this, rulesEngine);
        }

        public void printBox(TextBlock TextBlock1)
        {
            String stringtest = "";
            int inttest;
            for (int row = 0; row < 8; row++)
            {
                if (row == 0)
                {
                    stringtest = stringtest + "     A   B  C   D  E   F   G   H\n";
                }
                for (int column = 0; column < 8; column++)
                {
                    if (column == 0)
                    {
                        stringtest = stringtest + (row + 1) + "   ";
                    }
                    if (column < 8)
                    {
                        inttest = rulesEngine._board.getBoardPosition(row, column);
                        stringtest = stringtest + inttest.ToString();
                        stringtest = stringtest + "   ";
                    }
                }
                stringtest = stringtest + "\n";
            }
            TextBlock1.Text = stringtest;
        }
    }
}
