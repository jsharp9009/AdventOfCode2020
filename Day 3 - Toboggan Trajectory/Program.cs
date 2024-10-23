using System;
using System.IO;
using System.Linq;

namespace TobogganTrajectory;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var map = input.Select(s => s.ToCharArray()).ToArray();
        var part1Trees = CountTrees(map, 3, 1);
        Console.WriteLine($"Part 1: {part1Trees} trees.");

        var slope11 = CountTrees(map, 1, 1);
        var slope51 = CountTrees(map, 5, 1);
        var slope71 = CountTrees(map, 7, 1);
        var slope12 = CountTrees(map, 1, 2);

        Console.WriteLine($"Part 2: {part1Trees * slope11 * slope51 * slope71 * slope12} trees.");
    }

    static int CountTrees(char[][] map, int xStep, int yStep){
        var trees = 0;
        var currentX = 0;
        var currentY = 0;

        while(true){
            currentX += xStep;
            currentY += yStep;

            if(currentX >= map[0].Length){
                currentX = currentX % map[0].Length;
            }

            if(currentY >= map.Length){
                return trees;
            }

            if (map[currentY][currentX] == '#') trees++;
        }
    }
}
