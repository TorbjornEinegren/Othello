using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestApplikation
{
    class LINQ
    {
        int[,] data;

        public void boardChange(int[,] data)
        {
            XElement newBoardData = new XElement("Board",
                new XAttribute("row0", "" + data[0, 0] + data[0, 1] + data[0, 2] + data[0, 3] + data[0, 4] + data[0, 5] + data[0, 6] + data[0, 7]),
                new XAttribute("row1", "" + data[1, 0] + data[1, 1] + data[1, 2] + data[1, 3] + data[1, 4] + data[1, 5] + data[1, 6] + data[1, 7]),
                new XAttribute("row2", "" + data[2, 0] + data[2, 1] + data[2, 2] + data[2, 3] + data[2, 4] + data[2, 5] + data[2, 6] + data[2, 7]),
                new XAttribute("row3", "" + data[3, 0] + data[3, 1] + data[3, 2] + data[3, 3] + data[3, 4] + data[3, 5] + data[3, 6] + data[3, 7]),
                new XAttribute("row4", "" + data[4, 0] + data[4, 1] + data[4, 2] + data[4, 3] + data[4, 4] + data[4, 5] + data[4, 6] + data[4, 7]),
                new XAttribute("row5", "" + data[5, 0] + data[5, 1] + data[5, 2] + data[5, 3] + data[5, 4] + data[5, 5] + data[5, 6] + data[5, 7]),
                new XAttribute("row6", "" + data[6, 0] + data[6, 1] + data[6, 2] + data[6, 3] + data[6, 4] + data[6, 5] + data[6, 6] + data[6, 7]),
                new XAttribute("row7", "" + data[7, 0] + data[7, 1] + data[7, 2] + data[7, 3] + data[7, 4] + data[7, 5] + data[7, 6] + data[7, 7]));

            newBoardData.Save(@Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\board.xml");
        }
    }
}
