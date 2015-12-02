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
            initButtons();
            //chooseColor();
        }

        private void initButtons()
        {
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image blankImage = new Image();
                    blankImage.Source = (new ImageSourceConverter()).ConvertFromString("C:\\Users\\Yin\\Documents\\GitHub\\Othello\\TestApplikation\\Blank.bmp") as ImageSource;

                    System.Windows.Controls.Button newBtn = new Button();
                    newBtn.Name = "_" + ((i+1) * (j+1));
                    newBtn.Height = 50;
                    newBtn.Width = 50;
                    newBtn.Content = blankImage;
                    newBtn.Click += new RoutedEventHandler(button_Click);

                    Grid.SetRow(newBtn, i);
                    Grid.SetColumn(newBtn, j);


                    buttonGrid.Children.Add(newBtn);
                }
            }
        }

        private void chooseColor()
        {
            Image image1 = new Image();
            Image image2 = new Image();
            image1.Source = new BitmapImage(new Uri("C:\\Users\\Yin\\Documents\\GitHub\\Othello\\TestApplikation\\Light.bmp"));
            image2.Source = new BitmapImage(new Uri("C:\\Users\\Yin\\Documents\\GitHub\\Othello\\TestApplikation\\Dark.bmp"));
            image1.Width = 250;
            image1.Height = 250;
            image2.Width = 250;
            image2.Height = 250;
        }

        private void button_Click(object sender, RoutedEventArgs e)
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
