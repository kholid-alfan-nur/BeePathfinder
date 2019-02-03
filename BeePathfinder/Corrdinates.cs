//Group 4 Final Assignment Bee Breeding
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeePathfinder
{
    //initializing local variables for three coordinates of Hexagons as cube coordinates --> assembles the hexagons into a grid (cube coordinate)
    class Hexagon
    {
        public int x;
        public int y;
        public int z;

        public Hexagon(int a, int b, int c)
        {
            x = a;
            y = b;
            z = c;
        }
    }
    //to establish the method for mapping with hexagonal coordinates
    class Corrdinates
    {
        //methods for movement directions for 6 sides of hexagons
		//one function return one direction from that cube direction array
        private readonly Hexagon[] CUBE_DIRECTIONS = new Hexagon[] {
            new Hexagon(1, -1, 0),
            new Hexagon(1, 0, -1),
            new Hexagon(0, 1, -1),
            new Hexagon(-1, 1, 0),
            new Hexagon(-1, 0, 1),
            new Hexagon(0, -1, 1)
        };
		//store list for available hexagons
        private List<Hexagon> Results = new List<Hexagon>();
		//scale, add, and neighbour operations
		//scale is radius from current ring
        public Hexagon HexAdd(Hexagon a, Hexagon b)
        {
            return new Hexagon(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public Hexagon HexScale(Hexagon a, int k)
        {
            return new Hexagon(a.x * k, a.y * k, a.z * k);
        }

        public Hexagon HexNeighbour(Hexagon a, int direction)
        {
            return HexAdd(a, CUBE_DIRECTIONS[direction]);
        }

        //loops for movements within a ring and detecting the neighbouring hexagons
		//4 because it is the direction of the next ring --> the movement direction of the ring
        public List<Hexagon> HexRing(Hexagon center, int radius)
        { 
            List<Hexagon> results = new List<Hexagon>();
			//next ring always starts from direction 4 of the last hexagon of current ring
            Hexagon cube = HexAdd(center, HexScale(CUBE_DIRECTIONS[4], radius));
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    results.Add(cube);
                    cube = HexNeighbour(cube, i);
                }
            }
            return results;
        }
		//to find the last cell(hexagon) in the current ring
        public int FindHex(List<Hexagon> hexList, Hexagon cube)
        {
            for (int i = 0; i < hexList.Count; i++)
            {
				//compare the last hexagon of ring with list of all hexagons which is available in ring
                if (hexList[i].x == cube.x && hexList[i].y == cube.y && hexList[i].z == cube.z)
                {
                    return i;
                }
            }
            return -1;
        }

        //find and move across the ring to last hexagon
        public List<Hexagon> GetRing(Hexagon center, int radius, Hexagon lastHex)
        {
			//to find out whether a given hex is on a ring of a given radius, we calculate the distance from that hex to the center and see if it's radius. To get a list of all such hexes, we take steps away from the center, then follow the rotated vectors in a path around the ring.
            List<Hexagon> ring = HexRing(center, radius);
            Hexagon firstHex = new Hexagon(lastHex.x - 1, lastHex.y, lastHex.z + 1);
            int index = FindHex(ring, firstHex);
            if (index == -1)
            {
                return ring;
            }
            List<Hexagon> currentRing = new List<Hexagon>();
            for (int j = index; j < ring.Count; j++)
            {
				currentRing.Add(ring[j]);
            }

            for (int k = 0; k < index; k++)
            {
				currentRing.Add(ring[k]);
            }
            return currentRing;
        }
		//movments to the next neighbouring hexagon in a spiral fashion
		//movement starts from center (0,0,0) where first and last point is the same
        public List<Hexagon> CubeSpiral(Hexagon center, int radius)
        {
			//result for cubespiral, storing all available hexagons in a ring in a local list
			//traversing the rings one by one in a spiral pattern, the area of the larger hexagon will be the sum of the circumferences, plus 1 for the centre
            List<Hexagon> spiralResults = new List<Hexagon>();
			spiralResults.Add(center);
            Hexagon lastHex = new Hexagon(0, 0, 0);
            for (int k = 1; k <= radius; k++)
            {
                List<Hexagon> ring = GetRing(center, k, lastHex);
                lastHex = ring[ring.Count - 1];
				spiralResults.AddRange(ring);
            }
            return spiralResults;
        }

        //find the radius of ring in honeycomb
        private int FindRadius(int number)
        {
            double count = 0;
            double i = 1;
            while (true)
            {
                count = (3 * Math.Pow(i, 2) - 3 * i + 1); 
                i++;
                if (count > number)
                    return Convert.ToInt32(i);
            }
        }
		//return the total numbers of hexagons from center to maximum hexagon in grid
        private List<Hexagon> GetGridList(int maxNum)
        {
            if (Results.Count < maxNum)
            {
                int radius = FindRadius((maxNum));
                Results = CubeSpiral(new Hexagon(0, 0, 0), radius);
            }
            return Results;

        }

        //calculate the distance between two cells.
        public int FindDistance(int firstNum, int secondNum)
        {
			//heuristic function to calculate distance
			//finding distance between a & b in max number, one of the three coordinates must be the sum of the other two, then picking that maxNum as the distance.
			//the max number of the three coordinates is the distance, another formula is abs(a.x-b.x) + abs(a.y-b.y)+abs(a.z - b.z)/2
			//maxNum variable to call from the gridlist
            int maxNum = new int[] { firstNum, secondNum }.Max();
            List<Hexagon> results = GetGridList(maxNum);
            Hexagon firstHex = results[firstNum - 1];
            Hexagon secondHex = results[secondNum - 1];
            int distance = new int[] {Math.Abs(firstHex.x - secondHex.x),
               Math.Abs(firstHex.y - secondHex.y),
               Math.Abs(firstHex.z - secondHex.z) }.Max();
            return distance;
        }
    }
}
