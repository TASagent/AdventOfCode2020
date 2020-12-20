using AoCTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day20
{
    class Program
    {
        private const string inputFile = @"../../../../input20.txt";

        private static readonly string[] SEE_MONSTAR = new[]
        {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Day 20 - Jurassic Jigsaw");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            List<Tile> tiles = GetTiles(lines).ToList();

            Dictionary<int, List<Tile>> edgeTrackingDict = new Dictionary<int, List<Tile>>();

            foreach (Tile tile in tiles)
            {
                tile.AddToDict(edgeTrackingDict);
            }

            List<Tile> edgeTiles = new List<Tile>(edgeTrackingDict.Values
                .Where(x => x.Count == 1)
                .Select(x => x.First()));

            HashSet<Tile> uniqueEdgeTiles = new HashSet<Tile>(edgeTiles);

            HashSet<Tile> cornerTiles = new HashSet<Tile>(
                uniqueEdgeTiles
                .Where(y => edgeTiles.Count(x => x == y) == 4));

            long output1 = cornerTiles
                .Select(x => x.Id).Aggregate(1L, (x, y) => x * y);

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Tile[,] assembledImage = new Tile[12, 12];

            Tile protoCorner = cornerTiles.First();

            List<int> matchlessCorners = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                if (edgeTrackingDict[protoCorner.edgeValues[i]].Count == 1)
                {
                    matchlessCorners.Add(i);
                }
            }

            //0,3 -> UL
            //0,1 -> UR
            //1,2 -> LR
            //2,3 -> LL

            int protoX;
            int protoY;

            if (matchlessCorners.Contains(0))
            {
                protoY = 0;
            }
            else
            {
                protoY = 11;
            }

            if (matchlessCorners.Contains(1))
            {
                protoX = 11;
            }
            else
            {
                protoX = 0;
            }

            assembledImage[protoX, protoY] = protoCorner;
            protoCorner.Coordinate = (protoX, protoY);

            Queue<Tile> tilesToProcess = new Queue<Tile>();
            tilesToProcess.Enqueue(protoCorner);

            while (tilesToProcess.Count > 0)
            {
                Tile tileToProcess = tilesToProcess.Dequeue();

                Point2D coordinate = tileToProcess.Coordinate;

                var (top, right, bottom, left) = tileToProcess.GetEdges(tileToProcess.Orientation);

                //Check Up
                if (coordinate.y > 0 && assembledImage[coordinate.x, coordinate.y - 1] == null)
                {
                    //Find Upper Match
                    Tile correspondingTile = edgeTrackingDict[top].First(x => x != tileToProcess);
                    correspondingTile.Coordinate = (coordinate.x, coordinate.y - 1);
                    assembledImage[coordinate.x, coordinate.y - 1] = correspondingTile;

                    for (int orientation = 0; orientation < 8; orientation++)
                    {
                        var correspondingEdges = correspondingTile.GetEdges(orientation);
                        if (correspondingEdges.bottom == top)
                        {
                            correspondingTile.Orientation = orientation;
                            break;
                        }
                    }

                    tilesToProcess.Enqueue(correspondingTile);
                }

                //Check Down
                if (coordinate.y < 11 && assembledImage[coordinate.x, coordinate.y + 1] == null)
                {
                    //Find Lower Match
                    Tile correspondingTile = edgeTrackingDict[bottom].First(x => x != tileToProcess);
                    correspondingTile.Coordinate = (coordinate.x, coordinate.y + 1);
                    assembledImage[coordinate.x, coordinate.y + 1] = correspondingTile;

                    for (int orientation = 0; orientation < 8; orientation++)
                    {
                        var correspondingEdges = correspondingTile.GetEdges(orientation);
                        if (correspondingEdges.top == bottom)
                        {
                            correspondingTile.Orientation = orientation;
                            break;
                        }
                    }

                    tilesToProcess.Enqueue(correspondingTile);
                }

                //Check Right
                if (coordinate.x < 11 && assembledImage[coordinate.x + 1, coordinate.y] == null)
                {
                    //Find Lower Match
                    Tile correspondingTile = edgeTrackingDict[right].First(x => x != tileToProcess);
                    correspondingTile.Coordinate = (coordinate.x + 1, coordinate.y);
                    assembledImage[coordinate.x + 1, coordinate.y] = correspondingTile;

                    for (int orientation = 0; orientation < 8; orientation++)
                    {
                        var correspondingEdges = correspondingTile.GetEdges(orientation);
                        if (correspondingEdges.left == right)
                        {
                            correspondingTile.Orientation = orientation;
                            break;
                        }
                    }

                    tilesToProcess.Enqueue(correspondingTile);
                }

                //Check Left
                if (coordinate.x > 0 && assembledImage[coordinate.x - 1, coordinate.y] == null)
                {
                    //Find Lower Match
                    Tile correspondingTile = edgeTrackingDict[left].First(x => x != tileToProcess);
                    correspondingTile.Coordinate = (coordinate.x - 1, coordinate.y);
                    assembledImage[coordinate.x - 1, coordinate.y] = correspondingTile;

                    for (int orientation = 0; orientation < 8; orientation++)
                    {
                        var correspondingEdges = correspondingTile.GetEdges(orientation);
                        if (correspondingEdges.right == left)
                        {
                            correspondingTile.Orientation = orientation;
                            break;
                        }
                    }

                    tilesToProcess.Enqueue(correspondingTile);
                }
            }

            HashSet<Point2D> activeCoordinates = new HashSet<Point2D>();

            for (int tileX = 0; tileX < 12; tileX++)
            {
                for (int tileY = 0; tileY < 12; tileY++)
                {
                    Point2D corner = (8 * tileX, 8 * tileY);
                    Tile currentTile = assembledImage[tileX, tileY];

                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            if (currentTile.GetOrientedValue(x, y))
                            {
                                activeCoordinates.Add(corner + (x, y));
                            }
                        }
                    }
                }
            }

            List<Point2D> originalMonstarCoordinates = new List<Point2D>();

            for (int y = 0; y < SEE_MONSTAR.Length; y++)
            {
                for (int x = 0; x < SEE_MONSTAR[y].Length; x++)
                {
                    if (SEE_MONSTAR[y][x] == '#')
                    {
                        originalMonstarCoordinates.Add((x, y));
                    }
                }
            }

            int monsterCount = 0;
            List<Point2D> seaMonsterPositions = new List<Point2D>();
            List<Point2D> monstarCoordinates = null;

            for (int rotation = 0; rotation < 8; rotation++)
            {
                monstarCoordinates = originalMonstarCoordinates
                    .Select(x => GetRotatedValue(x, rotation)).ToList();

                //Start First Pass Logic

                Point2D maxDisplacement = (0, 0);
                foreach (Point2D point in monstarCoordinates)
                {
                    maxDisplacement = AoCMath.Max(point, maxDisplacement);
                }

                for (int x0 = 0; x0 < (12 * 8 - maxDisplacement.x); x0++)
                {
                    for (int y0 = 0; y0 < (12 * 8 - maxDisplacement.y); y0++)
                    {
                        if (SearchCoordinate((x0, y0), monstarCoordinates, activeCoordinates))
                        {
                            monsterCount++;
                            seaMonsterPositions.Add((x0, y0));
                        }
                    }
                }

                if (monsterCount > 0)
                {
                    break;
                }
            }

            foreach (Point2D monstarPosition in seaMonsterPositions)
            {
                foreach (Point2D monstarCoordinate in monstarCoordinates)
                {
                    activeCoordinates.Remove(monstarPosition + monstarCoordinate);
                }
            }


            Console.WriteLine($"The answer is: {activeCoordinates.Count}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static bool SearchCoordinate(
            Point2D initial,
            List<Point2D> monstarCoordinates,
            HashSet<Point2D> activeCoordinates)
        {
            foreach (Point2D monstarPoint in monstarCoordinates)
            {
                if (!activeCoordinates.Contains(monstarPoint + initial))
                {
                    return false;
                }
            }

            return true;
        }

        public static IEnumerable<Tile> GetTiles(string[] input)
        {
            int lastIndex = 0;
            for (int i = 1; i < input.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(input[i]))
                {
                    yield return new Tile(input[lastIndex..i]);
                    lastIndex = i + 1;
                }
            }

            if (lastIndex != input.Length)
            {
                yield return new Tile(input[lastIndex..]);
            }
        }

        public static IEnumerable<Point2D> GetAdjacent(Point2D centerPoint)
        {
            if (centerPoint.x > 0)
            {
                yield return new Point2D(centerPoint.x - 1, centerPoint.y);
            }

            if (centerPoint.x < 11)
            {
                yield return new Point2D(centerPoint.x + 1, centerPoint.y);
            }

            if (centerPoint.y > 0)
            {
                yield return new Point2D(centerPoint.x, centerPoint.y - 1);
            }

            if (centerPoint.y < 11)
            {
                yield return new Point2D(centerPoint.x, centerPoint.y + 1);
            }
        }


        public static Point2D GetRotatedValue(Point2D point, int newOrientation)
        {
            switch (newOrientation)
            {
                case 0: return (point.x, point.y);
                case 1: return (point.y, 19 - point.x);
                case 2: return (19 - point.x, 2 - point.y);
                case 3: return (2 - point.y, point.x);
                case 4: return (19 - point.x, point.y);
                case 5: return (2 - point.y, 19 - point.x);
                case 6: return (point.x, 2 - point.y);
                case 7: return (point.y, point.x);

                default:
                    throw new Exception();
            }
        }
    }

    public class Tile
    {
        public Point2D Coordinate { get; set; }
        public int Orientation { get; set; }

        public long Id { get; init; }
        private readonly bool[,] tileData;

        private readonly static Regex digitExtractor = new Regex(@"^Tile (\d+):");

        public int[] edgeValues = new int[8];

        public Tile(string[] input)
        {
            Id = long.Parse(digitExtractor.Match(input[0]).Groups[1].Value);

            tileData = new bool[input[1].Length, input.Length - 1];

            for (int x = 0; x < tileData.GetLength(0); x++)
            {
                for (int y = 0; y < tileData.GetLength(1); y++)
                {
                    if (input[y + 1][x] == '#')
                    {
                        tileData[x, y] = true;
                    }
                }
            }

            CalculateEdgeValues();
        }

        private void CalculateEdgeValues()
        {
            int dimLength = tileData.GetLength(0);

            for (int x = 0; x < dimLength; x++)
            {
                edgeValues[0] <<= 1;
                edgeValues[1] <<= 1;
                edgeValues[2] <<= 1;
                edgeValues[3] <<= 1;

                if (tileData[x, 0])
                {
                    edgeValues[0] |= 1;
                    edgeValues[4] |= (1 << x);
                }

                if (tileData[dimLength - 1, x])
                {
                    edgeValues[1] |= 1;
                    edgeValues[5] |= (1 << x);
                }

                if (tileData[x, dimLength - 1])
                {
                    edgeValues[2] |= 1;
                    edgeValues[6] |= (1 << x);
                }

                if (tileData[0, x])
                {
                    edgeValues[3] |= 1;
                    edgeValues[7] |= (1 << x);
                }
            }
        }

        public bool GetOrientedValue(int x, int y)
        {
            switch (Orientation)
            {
                case 0: return tileData[x + 1, y + 1];
                case 1: return tileData[y + 1, 8 - x];
                case 2: return tileData[8 - x, 8 - y];
                case 3: return tileData[8 - y, x + 1];
                case 4: return tileData[8 - x, y + 1];
                case 5: return tileData[8 - y, 8 - x];
                case 6: return tileData[x + 1, 8 - y];
                case 7: return tileData[y + 1, x + 1];

                default:
                    throw new Exception();
            }
        }

        public (int top, int right, int bottom, int left) GetEdges(int orientation)
        {
            switch (orientation)
            {
                case 0: return (edgeValues[0], edgeValues[1], edgeValues[2], edgeValues[3]);
                case 1: return (edgeValues[7], edgeValues[0], edgeValues[5], edgeValues[2]);
                case 2: return (edgeValues[6], edgeValues[7], edgeValues[4], edgeValues[5]);
                case 3: return (edgeValues[1], edgeValues[6], edgeValues[3], edgeValues[4]);
                case 4: return (edgeValues[4], edgeValues[3], edgeValues[6], edgeValues[1]);
                case 5: return (edgeValues[5], edgeValues[4], edgeValues[7], edgeValues[6]);
                case 6: return (edgeValues[2], edgeValues[5], edgeValues[0], edgeValues[7]);
                case 7: return (edgeValues[3], edgeValues[2], edgeValues[1], edgeValues[0]);

                default:
                    throw new Exception();
            }
        }

        public void AddToDict(Dictionary<int, List<Tile>> edgeTrackingDict)
        {
            foreach (int edgeValue in edgeValues)
            {
                if (edgeTrackingDict.ContainsKey(edgeValue))
                {
                    edgeTrackingDict[edgeValue].Add(this);
                }
                else
                {
                    edgeTrackingDict.Add(edgeValue, new List<Tile>() { this });
                }
            }
        }
    }

}
