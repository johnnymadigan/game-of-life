using System;
using System.Collections.Generic;

namespace Life
{
    /// <summary>
    /// Common parent neighbourhood
    /// </summary>
    /// <author>Johnny Madigan</author>
    /// <date>October 2020</date>
    public class ParentNeighbourhood
    {
        public List<List<int>> neighbours = new List<List<int>>();

        /// <summary>
        /// By creating an instance with specific values...
        /// the instance could be a Moore or VonNuemann neighbourhood
        /// </summary>
        /// <param name="order">How far the neighbourhood spreads</param>
        /// <param name="xCord">The centre cell's x coordinate</param>
        /// <param name="yCord">The centre cell's y coordinate</param>
        /// <param name="includeCentreCell">A bool that's true if the centre cell is included</param>
        /// <param name="von">A bool that's true if the instance is a VonNuemann</param>
        public ParentNeighbourhood(int order, int xCord, int yCord, bool includeCentreCell, bool von)
        {
            int furthestxNeighbour = xCord + order;
            int furthestyNeighbour = yCord + order;
            int closestxNeighbour = xCord - order;
            int closestyNeighbour = yCord - order;
            int vonY = yCord;
            int vonYcountTo = yCord;

            /*-----------------------------------------------------------------
                   COUNTING NEIGHBOURS FROM BOTTOM TO TOP, LEFT TO RIGHT
             -----------------------------------------------------------------*/
            for (int currentx = closestxNeighbour; currentx <= furthestxNeighbour; currentx++)
            {
                // By changing where the columns start counting and when to stop counting
                // A VonNuemann neighbourhood can be checked
                // Starts in-line with the y coordinate, then incriments by 1 each way,
                // counting from -1 and stopping at +1, creating a V shape
                // As soon as the row is in-line with the x coordinate, it's type to reverse
                // this shape, this is how to check a VonNuemann neighbourhood
                if (von)
                {
                    if (currentx < xCord)
                    {
                        closestyNeighbour = vonY;
                        furthestyNeighbour = vonYcountTo;
                        vonY--;
                        vonYcountTo++;
                    }
                    else
                    {
                        closestyNeighbour = vonY;
                        furthestyNeighbour = vonYcountTo;
                        vonY++;
                        vonYcountTo--;
                    }
                }
                
                for (int currenty = closestyNeighbour; currenty <= furthestyNeighbour; currenty++)
                {
                    if (includeCentreCell)
                    {
                        neighbours.Add(new List<int>() { currentx, currenty });
                    }
                    else if (!(currentx == xCord && currenty == yCord))
                    {
                        neighbours.Add(new List<int>() { currentx, currenty });
                    }    
                }
            }
        }
    }
}
