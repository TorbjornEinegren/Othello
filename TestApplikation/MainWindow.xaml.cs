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
    public partial class MainWindow : Window
    {
        private Game game;
        private RulesEngine rulesEngine;
        public MainWindow()
        {
            InitializeComponent();
            rulesEngine = new RulesEngine();
            game = new Game(this, rulesEngine);
            initGame();
        }

        private void initGame()
        {
            chooseColor();
            //Gameboard is initialized through a click in colorClick()            
        }

        private void chooseColor()
        {
            Image darkImage = new Image();
            Image lightImage = new Image();
            darkImage.Source = (new ImageSourceConverter()).ConvertFromString("C:\\Users\\Yin\\Documents\\GitHub\\Othello\\TestApplikation\\Light.bmp") as ImageSource;
            lightImage.Source = (new ImageSourceConverter()).ConvertFromString("C:\\Users\\Yin\\Documents\\GitHub\\Othello\\TestApplikation\\Dark.bmp") as ImageSource;

            System.Windows.Controls.Button darkBtn = new Button();
            darkBtn.Name = "dark";
            darkBtn.Content = darkImage;
            darkBtn.Click += new RoutedEventHandler(buttonClick);
            Grid.SetRow(darkBtn,3);
            Grid.SetColumn(darkBtn,3);
            buttonGrid.Children.Add(darkBtn);

            System.Windows.Controls.Button lightBtn = new Button();
            lightBtn.Name = "light";
            lightBtn.Content = lightImage;
            lightBtn.Click += new RoutedEventHandler(buttonClick);
            Grid.SetRow(lightBtn, 3);
            Grid.SetColumn(lightBtn, 4);
            buttonGrid.Children.Add(lightBtn);

            System.Windows.Controls.TextBlock textBlock = new TextBlock();
            textBlock.Height = 100;
            textBlock.Width = 200;
            textBlock.Text = "hej";
            Grid.SetRow(lightBtn, 2);
            Grid.SetColumn(lightBtn, 3);
            buttonGrid.Children.Add(textBlock);
        }

        private void initGameboard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image blankImage = new Image();
                    blankImage.Source = (new ImageSourceConverter()).ConvertFromString("C:\\Users\\Yin\\Documents\\GitHub\\Othello\\TestApplikation\\Blank.bmp") as ImageSource;

                    System.Windows.Controls.Button newBtn = new Button();
                    newBtn.Name = "_" + ((i + 1) * (j + 1));
                    newBtn.Height = 50;
                    newBtn.Width = 50;
                    newBtn.Content = blankImage;
                    newBtn.Click += new RoutedEventHandler(buttonClick);

                    Grid.SetRow(newBtn, i);
                    Grid.SetColumn(newBtn, j);


                    buttonGrid.Children.Add(newBtn);
                }
            }
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            String colorStr = ((Button)sender).Name;

            game.setStartingPlayer(colorStr);

            initGameboard();
        }

        private void colorClick(object sender, RoutedEventArgs e)
        {
            int senderID = Convert.ToInt32(((Button)sender).Name.Substring(1));

            Console.WriteLine(senderID);
        }
    }
}

//public void printBox(TextBlock TextBlock1)
//{
//    String stringtest = "";
//    int inttest;
//    for (int row = 0; row < 8; row++)
//    {
//        if (row == 0)
//        {
//            stringtest = stringtest + "     A   B  C   D  E   F   G   H\n";
//        }
//        for (int column = 0; column < 8; column++)
//        {
//            if (column == 0)
//            {
//                stringtest = stringtest + (row + 1) + "   ";
//            }
//            if (column < 8)
//            {
//                inttest = rulesEngine._board.getBoardPosition(row, column);
//                stringtest = stringtest + inttest.ToString();
//                stringtest = stringtest + "   ";
//            }
//        }
//        stringtest = stringtest + "\n";
//    }
//    TextBlock1.Text = stringtest;
//}
