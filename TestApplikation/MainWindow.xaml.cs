using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestApplikation
{
    public partial class MainWindow : Window
    {
        private Game game;
        private RulesEngine rulesEngine;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public MainWindow()
        {
            InitializeComponent();
            rulesEngine = new RulesEngine();
            game = new Game(this, rulesEngine);
            initGame();
            rulesEngine._board.onBoardChange += onBoardChange;
            rulesEngine.onRoundFinished += game.changeCurrentPlayer;
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
            lightImage.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Light.bmp") as ImageSource;
            darkImage.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Dark.bmp") as ImageSource;

            System.Windows.Controls.Button darkBtn = new Button();
            darkBtn.Name = "dark";
            darkBtn.Content = darkImage;
            darkBtn.Click += new RoutedEventHandler(chooseColorClick);
            Grid.SetRow(darkBtn,3);
            Grid.SetColumn(darkBtn,3);
            buttonGrid.Children.Add(darkBtn);

            System.Windows.Controls.Button lightBtn = new Button();
            lightBtn.Name = "light";
            lightBtn.Content = lightImage;
            lightBtn.Click += new RoutedEventHandler(chooseColorClick);
            Grid.SetRow(lightBtn, 3);
            Grid.SetColumn(lightBtn, 4);
            buttonGrid.Children.Add(lightBtn);

            System.Windows.Controls.TextBlock textBlock = new TextBlock();
            textBlock.Height = 100;
            textBlock.Width = 200;
            textBlock.Text = "Välj färg!";
            Grid.SetRow(textBlock, 2);
            Grid.SetColumn(textBlock, 3);
            buttonGrid.Children.Add(textBlock);
        }

        private void initGameboard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image blankImage = changeColor(i,j);
                    System.Windows.Controls.Button newBtn = new Button();

                    newBtn.Name = "_" + i + "_"+ j;
                    newBtn.Height = 50;
                    newBtn.Width = 50;
                    newBtn.Content = blankImage;
                    newBtn.Click += new RoutedEventHandler(clickButton);

                    Grid.SetRow(newBtn, i);
                    Grid.SetColumn(newBtn, j);
                    buttonGrid.Children.Add(newBtn);
                }
            }
        }

        private Image changeColor(int row, int column)
        {
            Image image = new Image();
            int color = rulesEngine._board.getBoardPosition(row, column);
            if (color == 0)
            {
                image.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Blank.bmp") as ImageSource;
            }
            else if (color == 1)
            {
                image.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Light.bmp") as ImageSource;
            }
            else if (color == 2)
            {
                image.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Dark.bmp") as ImageSource;
            }
            return image;
        }

        public void onBoardChange(int[] argArr)
        {
            int row = argArr[0];
            int column = argArr[1];
            Image image = changeColor(row, column);
            System.Windows.Controls.Button newBtn = new Button();

            newBtn.Name = "_" + row + "_" + column;
            newBtn.Height = 50;
            newBtn.Width = 50;
            newBtn.Content = image;
            newBtn.Click += new RoutedEventHandler(clickButton);

            Grid.SetRow(newBtn, row);
            Grid.SetColumn(newBtn, column);
            buttonGrid.Children.Add(newBtn);
        }
        private void chooseColorClick(object sender, RoutedEventArgs e)
        {
            String colorStr = ((Button)sender).Name;
            game.setStartingPlayer(colorStr);
            initGameboard();
            playerBox.TextWrapping = TextWrapping.Wrap;
            playerBox.Text = game.playerStringBuilder();
        }

        private void clickButton(object sender, RoutedEventArgs e)
        {
            int row = Convert.ToInt32(((Button)sender).Name.Substring(1,1));
            int column = Convert.ToInt32(((Button)sender).Name.Substring(3));

            game.initateMove(row, column);
            playerBox.Text = game.playerStringBuilder();
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
