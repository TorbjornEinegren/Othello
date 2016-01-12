using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

//Ett undantag från originalreglerna, att man står över ett kast och turen går till motståndaren om man inte kan lägga en bricka.
//Skulle bli massor med extrajobb då en tur skulle gå, men en bricka inte skulle placeras, speltiden skulle förlängas och i slutet skulle spelaren med extra brickor lägga alla sina på rad.

namespace TestApplikation
{
    public partial class MainWindow : Window
    {
        private Game game;
        private RulesEngine rulesEngine;
        private string playerStr;
        System.Windows.Controls.Button mixButton;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public MainWindow()
        {
            InitializeComponent();
            rulesEngine = new RulesEngine();
            game = new Game(this, rulesEngine);
            rulesEngine._board.onBoardChange += onBoardChange;
            rulesEngine.onRoundFinished += game.changeCurrentPlayer;
            rulesEngine.onMoveFinished += game.allowMovesAgain;
            rulesEngine.onBadMove += textChange;
            choosePlayers();
        }

        private void chooseColor()
        {
            buttonGrid.Children.Remove(mixButton);
            if (playerStr.Equals("AI"))
            {
                game.setStartingColor("dark", playerStr);
                initGameboard();
            }
            else
            {
                Image darkImage = new Image();
                Image lightImage = new Image();
                lightImage.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Light.bmp") as ImageSource;
                darkImage.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Dark.bmp") as ImageSource;

                System.Windows.Controls.Button darkBtn = new Button();
                darkBtn.Name = "dark";
                darkBtn.Content = darkImage;
                darkBtn.Click += new RoutedEventHandler(chooseColorClick);
                Grid.SetRow(darkBtn, 3);
                Grid.SetColumn(darkBtn, 3);
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
        }

        public void choosePlayers()
        {
            Button aiButton = new Button();
            aiButton.Name = "AI";
            aiButton.Content = "AI\nAI";
            aiButton.Click += new RoutedEventHandler(choosePlayerClick);
            Grid.SetRow(aiButton, 3);
            Grid.SetColumn(aiButton, 3);
            buttonGrid.Children.Add(aiButton);

            Button humanButton = new Button();
            humanButton.Content = "Human\nHuman";
            humanButton.Name = "Human";
            humanButton.Click += new RoutedEventHandler(choosePlayerClick);
            Grid.SetRow(humanButton, 3);
            Grid.SetColumn(humanButton, 4);
            buttonGrid.Children.Add(humanButton);

            Button mixButton = new Button();
            mixButton.Name = "Mix";
            mixButton.Content = "Human\nAI";
            mixButton.Click += new RoutedEventHandler(choosePlayerClick);
            Grid.SetRow(mixButton, 3);
            Grid.SetColumn(mixButton, 5);
            buttonGrid.Children.Add(mixButton);

            Button loadButton = new Button();
            loadButton.Name = "Mix";
            loadButton.Content = "Ladda \nspel";
            loadButton.Click += new RoutedEventHandler(loadGameButton);
            Grid.SetRow(loadButton, 4);
            Grid.SetColumn(loadButton, 4);
            buttonGrid.Children.Add(loadButton);
        }

        private void initGameboard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image blankImage = changeColor(i, j);
                    System.Windows.Controls.Button newBtn = new Button();

                    newBtn.Name = "_" + i + "_" + j;
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
        }        public void textChange(String newText)
        {
            playerBox.Text = newText;
        }
        private void chooseColorClick(object sender, RoutedEventArgs e)
        {
            String colorStr = ((Button)sender).Name;
            game.setStartingColor(colorStr, playerStr);
            initGameboard();
            playerBox.TextWrapping = TextWrapping.Wrap;
        }

        private void choosePlayerClick(object sender, RoutedEventArgs e)
        {
            playerStr = ((Button)sender).Name;
            chooseColor();
        }

        private void loadGameButton(object sender, RoutedEventArgs e)
        {
            initGameboard();
            game.loadGame();
        }

        private void clickButton(object sender, RoutedEventArgs e)
        {
            int row = Convert.ToInt32(((Button)sender).Name.Substring(1, 1));
            int column = Convert.ToInt32(((Button)sender).Name.Substring(3));

            game.initateMove(row, column);
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
