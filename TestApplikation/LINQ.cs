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

        public LINQ(RulesEngine rulesEngine)
        {
            this.rulesEngine = rulesEngine;
            xdoc = XDocument.Load(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void updateTilesRemaining(PlayerAbstract currentPlayer)
        {
            IEnumerable<XElement> piece = from pieces in xdoc.Descendants("Player")
                                          where pieces.Attribute("Color").Value.Equals(currentPlayer._color)
                                          select pieces;

            foreach (XElement itemElement in piece)
            {
                itemElement.SetAttributeValue("TilesRemaining", currentPlayer._tilesRemaining);
            }

            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public Object[] updatedXML()
        {
            String[,] fromXMLArray = loadGame();
            int differenceCounter = 0;
            Object[] changedPositionArray = new Object[3];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (fromXMLArray[i, j] != null)
                    {
                        if (fromXMLArray[i, j] != comparisonArray[i, j])
                        {
                            if (comparisonArray[i, j] != null || differenceCounter > 2)
                            {
                                removeTile(i, j);
                            }
                            else
                            {
                                if ((rulesEngine._tilesRemaining % 2 == 0 && "Black".Equals(fromXMLArray[i, j]))
                                    || (rulesEngine._tilesRemaining % 2 != 0 && "White".Equals(fromXMLArray[i, j])))
                                {
                                    if (rulesEngine.isMoveLegal(i, j, fromXMLArray[i, j]))
                                    {
                                        differenceCounter++;
                                        if (differenceCounter == 1)
                                        {
                                            changedPositionArray[0] = i;
                                            changedPositionArray[1] = j;
                                            changedPositionArray[2] = fromXMLArray[i, j];
                                        }
                                        else
                                        {
                                            removeTile(i, j);
                                        }
                                    }
                                    else
                                    {
                                        removeTile(i, j);
                                    }
                                }
                                else
                                {
                                    removeTile(i, j);
                                }
                            }
                        }
                    }
                    else if (comparisonArray[i, j] != null)
                    {

                    }
                }
            }
            return changedPositionArray;
        }

        private void returnTile()
        {

        }

        private void removeTile(int i, int j)
        {
            var xElement = (from el in xdoc.Descendants("piece")
                            where int.Parse(el.Attribute("row").Value).Equals(i)
                            && int.Parse(el.Attribute("column").Value).Equals(j)
                            select el);
            xElement.Remove();
            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void createPlayers(PlayerAbstract player1, PlayerAbstract player2)
        {
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
            xdoc.Descendants("Players").Descendants().Remove();
            xdoc.Descendants("Board").Descendants().Remove();

            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void initBoard()
        {
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

            xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }

        public void boardChange(Object[] data)
        {
            if (data[2].ToString().Equals("Black") || data[2].ToString().Equals("White"))
            {
                comparisonArray[(int)data[0], (int)data[1]] = (String)data[2];

                if (xdoc.Root.Element("Board").HasElements)
                {
                    IEnumerable<XElement> piece = from pieces in xdoc.Descendants("piece")
                                                  where (int.Parse(pieces.Attribute("row").Value) == (int)data[0]
                                                  && int.Parse(pieces.Attribute("column").Value) == (int)data[1])
                                                  select pieces;

                    bool exist = true;
                    foreach (XElement itemElement in piece)
                    {
                        itemElement.SetAttributeValue("color", data[2]);
                        exist = false;
                    }

                    if (exist)
                    {
                        xdoc.Root.Element("Board").Add(
                                new XElement("piece",
                                new XAttribute("row", data[0]),
                                new XAttribute("column", data[1]),
                                new XAttribute("color", data[2])));
                    }
                }

                xdoc.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
            }
        }

        public String[,] loadGame()
        {
            xdoc = XDocument.Load(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");

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
    }
}
