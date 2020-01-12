# AI Cave Navigator

Submitted in November 2018 for the Artificial Intelligence module (SET09122), as part of the BEng Software Engineering (Hons) degree at Edinburgh Napier University.

## Description

This program uses the A* Search Algorithm to find the shortest possible path through a cave system, where there is a start and end cavern. This is achieved as the application uses heuristics to determine the 'cost' of a particular route, and favours paths that are of a lesser cost. Each cycle, the cost is calculated with the neighbouring nodes (caverns) and after enough cycles, the minimum cost is established: the shortest path.

## How to Use

To run this program, use a CLI to navigate to the filepath that contains the program executable, the batchfile and the test .CAV files. Type the command 'caveroute', followed by the .CAV file to be tested. For example:
```
caveroute demo1
```
The console will output whether the test was successful, the path the program took and the elapsed time of the search.
