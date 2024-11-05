using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RainRisk;

class Program
{
    public static Dictionary<char, Dictionary<int, char>> turns = new Dictionary<char, Dictionary<int, char>>(){
        {'N', new Dictionary<int, char>(){
            {90, 'E'},
            {180, 'S'},
            {270, 'W'}
        }},
        {'E', new Dictionary<int, char>(){
            {90, 'S'},
            {180, 'W'},
            {270, 'N'}
        }},
        {'S', new Dictionary<int, char>(){
            {90, 'W'},
            {180, 'N'},
            {270, 'E'}
        }},
        {'W', new Dictionary<int, char>(){
            {90, 'N'},
            {180, 'E'},
            {270, 'S'}
        }},
    };

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(l => new Tuple<char, int>(l[0], int.Parse(l.Substring(1)))).ToArray();
        var dist = FindDistanceAfterRun(input);
        Console.WriteLine("Part 1 Distance: {0}", dist);

        dist = FindDistanceAfterRunWithWayPoint(input);
        Console.WriteLine("Part 2 Distance: {0}", dist);
    }

    static int FindDistanceAfterRun(Tuple<char, int>[] input){
        var facing = 'E';
        var north = 0;
        var east = 0;

        foreach (var ins in input){
            switch (ins.Item1 == 'F' ? facing : ins.Item1) {
                case 'N':
                    north += ins.Item2;
                    break;
                case 'S':
                    north -= ins.Item2;
                    break; 
                case 'E':
                     east += ins.Item2;
                    break;
                case 'W':
                     east -= ins.Item2;
                    break;
                case 'R':
                    facing = turns[facing][ins.Item2];
                    break;
                case 'L':
                    facing = turns[facing][360 - ins.Item2];
                    break;
            }
        }
        return Math.Abs(north) + Math.Abs(east);
    }

    static int FindDistanceAfterRunWithWayPoint(Tuple<char, int>[] input){
        var waypointnorth = 1;
        var waypointeast = 10;
        var shipnorth = 0;
        var shipeast = 0;

        var tempwaypointeast = 0;

        foreach (var ins in input){
            switch (ins.Item1) {
                case 'N':
                    waypointnorth += ins.Item2;
                    break;
                case 'S':
                    waypointnorth -= ins.Item2;
                    break; 
                case 'E':
                     waypointeast += ins.Item2;
                    break;
                case 'W':
                     waypointeast -= ins.Item2;
                    break;
                case 'F':
                    shipnorth += waypointnorth * ins.Item2;
                    shipeast += waypointeast * ins.Item2;
                    break;
                case 'R':
                    switch(ins.Item2){
                        case 90:
                            tempwaypointeast = waypointeast;
                            waypointeast = waypointnorth;
                            waypointnorth = tempwaypointeast * -1;
                            break;
                        case 180:
                            tempwaypointeast = waypointeast;
                            waypointnorth = waypointnorth * -1;
                            waypointeast = tempwaypointeast * -1;
                            break;       
                        case 270:
                            tempwaypointeast = waypointeast;
                            waypointeast = waypointnorth * -1;
                            waypointnorth = tempwaypointeast;
                            break;                     
                    }
                    break;
                case 'L':
                    switch(ins.Item2){
                        case 90:
                            tempwaypointeast = waypointeast;
                            waypointeast = waypointnorth * -1;
                            waypointnorth = tempwaypointeast;
                            break;
                        case 180:
                            tempwaypointeast = waypointeast;
                            waypointnorth = waypointnorth * -1;
                            waypointeast = tempwaypointeast * -1;
                            break;       
                        case 270:
                            tempwaypointeast = waypointeast;
                            waypointeast = waypointnorth;
                            waypointnorth = tempwaypointeast * -1;
                            break;                     
                    }
                    break;
            }
        }
        return Math.Abs(shipnorth) + Math.Abs(shipeast);
    }
}
