using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace SET09122___CW
{
    class Program
    {
        static void Main(string[] args)
        {
            string cavFile = Path.GetFileName(args[0] + ".cav");
            string fileDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileContents = File.ReadAllText(cavFile);

            
            // Calculates the elapsed time of the pathfinding
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //The time at the start of running the program
            DateTime startTime = DateTime.Now;
            
            //List of all the integers of the imported file
            List<int> fileValues = fileContents.Split(',').Select(int.Parse).ToList();

            //List of Caverns
            List<Node> cavernList = new List<Node>();
            //List of the navigation relationship between all caverns
            List<int> pathRelations = new List<int>();
            
            //List of caverns that are not yet visited
            List<Node> unvisitedCaverns = new List<Node>();
            //List of caverns that have been visited
            List<Node> visitedCaverns = new List<Node>();


            //Collection of key-value pairs for each cavern and the cavern it can be most efficiently reached from
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            // Cost of start cavern to a given cavern
            Dictionary<Node, double> gScore = new Dictionary<Node, double>();
            // Cost of start cavern to goal cavern passing through given cavern
            Dictionary<Node, double> fScore = new Dictionary<Node, double>();

            bool success = false;
            Node currentCavern = null;

            // Get number of caverns (indicated by the first digit in the read-in file)
            int noOfCaverns = fileValues[0];

            // Add all caverns to the list of all caverns
            for (int i = 1, j = 1; i < noOfCaverns * 2; i = i + 2, j++)
            {
                Node myNode = new Node(fileValues[i], fileValues[i + 1], j);
                cavernList.Add(myNode);
            }

            // Add the first cavern in the cavern list and the cost to the gScore map
            gScore.Add(cavernList[0], 0);
            // Add the first cavern and the distance between the start and goal caverns
            fScore.Add(cavernList[0], EuclDist(cavernList[0], cavernList[cavernList.Count() - 1]));

            // Goal cavern is the final cavern in the cavern list
            Node goalCavern = cavernList[cavernList.Count() - 1];
            // Add the first cavern in the cavern list to the list of unvisited caverns
            unvisitedCaverns.Add(cavernList[0]);

            // For every connection in the cavern matrix, add to a list of the path relations between caverns
            for (int i = noOfCaverns * 2 + 1; i < fileValues.Count(); i++)
            {
                int connectivity = fileValues[i];
                pathRelations.Add(connectivity);
            }

            // For every cavern
            for (int j = 0; j < noOfCaverns; j++)
            {
                // For every cavern in the list of caverns
                for (int i = 0; i < cavernList.Count(); i++)
                {
                    Node testNode = cavernList[j];
                    
                    if (pathRelations[i + (cavernList.Count * j)] == 1)
                    {
                        cavernList[i].nearestCavern().Add(testNode);
                    }
                }
            }

            while (unvisitedCaverns.Count() > 0)
            {
                double lowestValue = double.MaxValue;

                //For every object in the list of univisted caverns
                foreach (Node iteration in unvisitedCaverns)
                {
                    // For every object in fScore
                    foreach (KeyValuePair<Node, double> entry in fScore)
                    {
                        Node key = entry.Key;
                        double val = entry.Value;

                        // If the value from unvisitedCaverns is in fScore, compare and set it to that if lower
                        if (iteration == key)
                        {
                            if (lowestValue > val)
                            {
                                lowestValue = val;
                                currentCavern = key;
                            }
                        }
                    }
                }

                // If the current cavern is the goal cavern, exit the loop
                if (currentCavern.Equals(goalCavern))
                {
                    success = true;
                    break;
                }

                // Remove the current cavern from the list of unvisited caverns
                unvisitedCaverns.Remove(currentCavern);
                // Add the current cavern to the list of visited caverns
                visitedCaverns.Add(currentCavern);

                // For every cavern that is a neighbour of the current cavern
                foreach (Node neighbour in currentCavern.nearestCavern())
                {
                    // If the neighbour has been visited before, skip to the next neighbour
                    if (visitedCaverns.Contains(neighbour))
                    {
                        continue;
                    }

                    // Distance from start to neighbour
                    double tempgScore = gScore[currentCavern] + EuclDist(currentCavern, neighbour);

                    // If the unvisited caverns list does not contain the neighbour
                    if (!unvisitedCaverns.Contains(neighbour))
                    {
                        // Add neighbour to the list of univisited caverns
                        unvisitedCaverns.Add(neighbour);
                    }
                    // Else if the added gScore of the current cavern and distance to neighbour is greater 
                        // or equal to the gScore of the neighbour, skip to the next neighbour
                    else if (tempgScore >= gScore[neighbour])
                    {
                        continue;
                    }

                    // The current best path
                    // Add neighbour and current cavern to the cameFrom key/value list
                    cameFrom.Add(neighbour, currentCavern);
                    // Add neighbour and the temp gScore to key/value list
                    gScore.Add(neighbour, tempgScore);
                    // Add neighbour node and gSocre of neighbour added to distance between the neighbour and goal cavern
                    fScore.Add(neighbour, gScore[neighbour] + EuclDist(neighbour, goalCavern));
                }
            }

            // If a path to the goal cavern has been found
            if (success)
            {
                // Stop the stopwatch
                stopwatch.Stop();
                
                //The time at the end of running the program
                long elapsedTime = stopwatch.ElapsedMilliseconds;
                

                // Write to console
                Console.WriteLine("\n" + "Path found!" + "\nElapsed Time: " + elapsedTime + "ms\n");

                Console.WriteLine("Path travelled: ");

                List<Node> finalPath = RecreatePath(cameFrom, currentCavern);

                string outputString = "";

                // Print the final cavern path
                for (int i = 0; i < finalPath.Count(); i++)
                {
                    Node current = finalPath[i];
                    outputString = outputString + current.Id + " ";
                }
                // Write the final path to the console
                Console.WriteLine(outputString.Trim());
                // Write .csn file
                File.WriteAllText(fileDirectory + "/" + args[0] + ".csn", outputString.Trim());
            }
            else
            {
                Console.WriteLine("No path was found.\n");
                // Write .csn file
                File.WriteAllText(fileDirectory + "/" + args[0] + ".csn", "0");
            }
        }


        //Calculates the Euclidean Distance between two specified caverns
        static double EuclDist(Node fromCavern, Node toCavern)
        {
            double x_coord = (toCavern.PosX - fromCavern.PosX);
            double y_coord = (toCavern.PosY - fromCavern.PosY);
            double distance = Math.Sqrt(x_coord * x_coord + y_coord * y_coord);
            return distance;
        }

        //Calculates total path
        static List<Node> RecreatePath(Dictionary<Node, Node> cameFrom, Node currentNode)
        {
            // Add the current cavern to the total path list
            List<Node> totalPath = new List<Node>();
            totalPath.Add(currentNode);
            
            // While the previous travel is the current cavern
            while (cameFrom.ContainsKey(currentNode))
            {
                currentNode = cameFrom[currentNode];
                totalPath.Add(currentNode);
            }
            // Reverse the list of the travelled path and return it
            totalPath.Reverse();
            return (totalPath);
        }
    }
}
