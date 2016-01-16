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
        private string playerStr;
        System.Windows.Controls.Button mixButton;
        System.Windows.Controls.Button loadButton;
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        bool aiMatch = false;

        public MainWindow()
        {
            InitializeComponent();
            rulesEngine = new RulesEngine();
            game = new Game(rulesEngine);
            rulesEngine._board.onBoardChange += onBoardChange;
            rulesEngine.onRoundFinished += game.changeCurrentPlayer;
            rulesEngine.onMoveFinished += game.allowMovesAgain;
            rulesEngine.onMoveFeedback += textChange;
            restartButton.Click += new RoutedEventHandler(restartGame);
            game.playerChange += textChange;
            rulesEngine.onWin += textChange;
            rulesEngine.onWinState += game.setWinState;
            choosePlayers();
        }

        private void chooseColor()
        {
            buttonGrid.Children.Remove(mixButton);
            buttonGrid.Children.Remove(loadButton);
            if (playerStr.Equals("AI"))
            {
                aiMatch = true;
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

            mixButton = new Button();
            mixButton.Name = "Mix";
            mixButton.Content = "Human\nAI";
            mixButton.Click += new RoutedEventHandler(choosePlayerClick);
            Grid.SetRow(mixButton, 3);
            Grid.SetColumn(mixButton, 5);
            buttonGrid.Children.Add(mixButton);

            loadButton = new Button();
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
            String color = rulesEngine._board.getBoardPosition(row, column);
            if (color == null)
            {
                image.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Blank.bmp") as ImageSource;
            }
            else if (color.Equals("White"))
            {
                image.Source = (new ImageSourceConverter()).ConvertFromString(startupPath + "\\Light.bmp") as ImageSource;
            }
            else if (color.Equals("Black"))
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
        }

        private void restartGame(object sender, RoutedEventArgs e)
        {
            textChange("");
            game.restartGame();
            buttonGrid.Children.Clear();
            aiMatch = false;
            choosePlayers();
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
            if (!(aiMatch))
            {
                int row = Convert.ToInt32(((Button)sender).Name.Substring(1, 1));
                int column = Convert.ToInt32(((Button)sender).Name.Substring(3));

                game.initateMove(row, column);
            }
        }
    }
}