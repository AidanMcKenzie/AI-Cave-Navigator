using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET09122___CW
{
    class Node
    {
        //Store the X & Y coordinates for this node
        private int posX, posY;                             
        //Store the navigation relationship between this node and every other node
        private List<int> pathRelations = new List<int>();  
        private List<Node> neighbours = new List<Node>();


        private int id;
        private int x;
        private int y;

        public Node(int x, int y, int id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }


        //Modifier method for setting and retrieving the X Coordinate
        public int PosX                                     
        {
            get { return posX; }
            set { posX = value; }
        }
        //Modifier method for setting and retrieving the Y Coordinate
        public int PosY                                     
        {
            get { return posY; }
            set { posY = value; }
        }

        //Modifier method for setting and retrieving the navigation relationship list
        public List<int> PathRelations                     
        {
            get { return pathRelations; }
            set { pathRelations = value; }
        }

        public List<Node> nearestCavern()
        {
            return neighbours;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
