using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TestApplikation
{
    public class LINQ
    {
        private XDocument xdoc;
        private String[,] comparisonArray = new String[8, 8];
        private RulesEngine rulesEngine;
        private FileSystemWatcher watcher;
        public bool playerNotDoingThings = true;

        public LINQ(RulesEngine rulesEngine)
        {
            //Starts the filesystemwatcher
            this.rulesEngine = rulesEngine;
            xdoc = XDocument.Load(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
            watcher = new FileSystemWatcher(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "board.xml";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public Action<int[]> onXMLChanged { get; set; }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //loads the whole board from the XML and replaces the current gameboard
            try
            {
                rulesEngine._board.loadBoard(loadGame());
                watcher.EnableRaisingEvents = false;
            }
            finally
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        public void updateTilesRemaining(PlayerAbstract currentPlayer)
        {
            //Updates the number of tiles remaining in the playerobject
            //Reads the current number of tiles remaining in the XML and rewrites it -1
            currentPlayer.updateTiles();

            IEnumerable<XElement> piece = from pieces in xdoc.Descendants("Player")
                                          where pieces.Attribute("Color").Value.Equals(currentPlayer._color)
                                          select pieces;

            foreach (XElement itemElement in piece)
            {
                itemElement.SetAttributeValue("TilesRemaining", currentPlayer._tilesRemaining);
            }

            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void createPlayers(PlayerAbstract player1, PlayerAbstract player2)
        {
            //Adds the new players to the XML-file
            xdoc = XDocument.Load(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");

            if (!(xdoc.Root.Element("Players").HasElements))
            {
                xdoc.Root.Element("Players").Add(new XElement("Player",
                new XAttribute("isAI", player1._isAI),
                new XAttribute("Color", player1._color),
                new XAttribute("Name", player1._name),
                new XAttribute("TilesRemaining", player1._tilesRemaining)));
                xdoc.Root.Element("Players").Add(new XElement("Player",
                new XAttribute("isAI", player2._isAI),
                new XAttribute("Color", player2._color),
                new XAttribute("Name", player2._name),
                new XAttribute("TilesRemaining", player2._tilesRemaining)));
            }
            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void gameEnd()
        {
            //Old players and pieces placed are removed
            xdoc.Descendants("Players").Descendants().Remove();
            xdoc.Descendants("Board").Descendants().Remove();
            xdoc.Descendants("TurnsLeft").Remove();

            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void initBoard()
        {
            //When a new game session is started gameend is called
            //The initial 4 mid-pieces are placed
            gameEnd();
            xdoc.Root.Element("Board").Add(
                new XElement("piece",
                new XAttribute("row", 3),
                new XAttribute("column", 3),
                new XAttribute("color", "Black")));
            xdoc.Root.Element("Board").Add(
                new XElement("piece",
                new XAttribute("row", 3),
                new XAttribute("column", 4),
                new XAttribute("color", "White")));
            xdoc.Root.Element("Board").Add(
                new XElement("piece",
                new XAttribute("row", 4),
                new XAttribute("column", 3),
                new XAttribute("color", "White")));
            xdoc.Root.Element("Board").Add(
                new XElement("piece",
                new XAttribute("row", 4),
                new XAttribute("column", 4),
                new XAttribute("color", "Black")));
            xdoc.Root.Add(
                 new XElement("TurnsLeft",
                 new XAttribute("Turns", 60)));
            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");

        }

        public void updateBoardOnChange(String[,] data)
        {
            //When a player has placed a piece and the following pieces have been turned
            //Adds the pieces to the XML if the position isn't null
            //Parses the current amount of turns left from the XML and removes 1 and saves it again
            xdoc = XDocument.Load(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
            xdoc.Descendants("Board").Descendants().Remove();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (data[i, j] != null)
                    {
                        xdoc.Root.Element("Board").Add(
                                    new XElement("piece",
                                    new XAttribute("row", i),
                                    new XAttribute("column", j),
                                    new XAttribute("color", data[i, j])));
                    }
                }
            }
            int oldTurnsLeft = int.Parse(xdoc.Root.Element("TurnsLeft").Attribute("Turns").Value);
            xdoc.Root.Element("TurnsLeft").SetAttributeValue("Turns", oldTurnsLeft - 1);

            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public String[,] loadGame()
        {
            //If a player currently isn't doing anything
            //Wait for a short time and load the document again to find new changes
            //Create a new String Array and read the pieces found in XML to it
            if (playerNotDoingThings)
            {
                System.Threading.Thread.Sleep(100);
                xdoc = XDocument.Load(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
            }

            String[,] loadedGameBoard = new String[8, 8];

            List<XElement> tempList = new List<XElement>();

            foreach (XElement e in xdoc.Descendants("piece"))
                tempList.Add(e);
            for (int i = 0; i < tempList.Count; i++)
            {
                int row = int.Parse(tempList[i].Attribute("row").Value);
                int column = int.Parse(tempList[i].Attribute("column").Value);
                loadedGameBoard[row, column] = tempList[i].Attribute("color").Value;
            }
            return loadedGameBoard;
        }

        public PlayerAbstract[] loadPlayers()
        {
            //Create an array of players
            //Read the player info from XML and create two new playerobjects with the data
            PlayerAbstract[] players = new PlayerAbstract[2];

            List<XElement> tempList = new List<XElement>();

            foreach (XElement e in xdoc.Descendants("Player"))
                tempList.Add(e);

            for (int i = 0; i < 2; i++)
            {
                XElement tempElement = tempList[i];
                if (bool.Parse(tempElement.Attribute("isAI").Value))
                {
                    players[i] = new AI(tempElement.Attribute("Name").Value.ToString(),
                    tempElement.Attribute("Color").Value);
                    players[i]._tilesRemaining = int.Parse(tempElement.Attribute("TilesRemaining").Value);
                }
                else
                {
                    players[i] = new Human(tempElement.Attribute("Name").Value.ToString(),
                    tempElement.Attribute("Color").Value);
                    players[i]._tilesRemaining = int.Parse(tempElement.Attribute("TilesRemaining").Value);
                }
            }
            return players;
        }

        public int loadTurnsRemaining()
        {
            //Read the turnsremaining from the XML
            int turnsRemaining = int.Parse(xdoc.Root.Element("TurnsLeft").Attribute("Turns").Value);
            return turnsRemaining;
        }
    }
}
