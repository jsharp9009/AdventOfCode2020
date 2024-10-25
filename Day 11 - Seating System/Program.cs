using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeatingSystem;

class Program
{
    static readonly List<Tuple<int, int>> DIRECTIONS = new List<Tuple<int, int>>(){
        new Tuple<int, int>(0,1),
        new Tuple<int, int>(1,0),
        new Tuple<int, int>(0,-1),
        new Tuple<int, int>(-1,0),
        new Tuple<int, int>(1,1),
        new Tuple<int, int>(1,-1),
        new Tuple<int, int>(-1,-1),
        new Tuple<int, int>(-1,1),
    };

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(s => s.ToCharArray()).ToArray();
        var NoChanges = false;
        var part1Input = input.Select(a => a.ToArray()).ToArray();
        while(!NoChanges){
            part1Input = transformPart1(part1Input, out NoChanges);
        }
        Console.WriteLine($"Part 1 Occupied: " + part1Input.Sum(i => i.Count(c => c == '#')));

        NoChanges = false;
        while(!NoChanges){
            input = transformPart2(input, out NoChanges);
        }
        Console.WriteLine($"Part 2 Occupied: " + input.Sum(i => i.Count(c => c == '#')));
    }

    static char[][] transformPart1(char[][] input, out bool NoChanges){
        NoChanges = true;
        var output = new char[input.Length][];

        for (int i = 0; i < input.Length; i++){
            output[i] = new char[input[i].Length];
            for(int n = 0; n < input[i].Length; n++){
                if(input[i][n] == '.'){
                    output[i][n] = '.';
                    continue;
                }
                var occupied = 0;
                foreach(var dir in DIRECTIONS){
                    var x = i + dir.Item1;
                    var y = n + dir.Item2;

                    if (x < 0 || x >= input.Length || y < 0 || y >= input[x].Length) continue;

                    if (input[x][y] == '#') occupied++;
                }

                if (input[i][n] == 'L' && occupied == 0) { output[i][n] = '#';  NoChanges = false; }
                else if (input[i][n] == '#' && occupied >= 4) { output[i][n] = 'L'; NoChanges = false; }
                else output[i][n] = input[i][n];
            }
        }
        return output;
    }


    static char[][] transformPart2(char[][] input, out bool NoChanges){
        NoChanges = true;
        var output = new char[input.Length][];

        for (int i = 0; i < input.Length; i++){
            output[i] = new char[input[i].Length];
            for(int n = 0; n < input[i].Length; n++){
                if(input[i][n] == '.'){
                    output[i][n] = '.';
                    continue;
                }
                var occupied = 0;
                foreach(var dir in DIRECTIONS){
                    var x = i + dir.Item1;
                    var y = n + dir.Item2;
                    if (x < 0 || x >= input.Length || y < 0 || y >= input[x].Length) continue;

                    bool breakout = false;

                    while(input[x][y] == '.'){
                        x = x + dir.Item1;
                        y = y + dir.Item2;

                        if (x < 0 || x >= input.Length || y < 0 || y >= input[x].Length) {
                            breakout = true;
                            break;
                        }
                    }
                    if (breakout) continue;
                    if (input[x][y] == '#') occupied++;
                }

                if (input[i][n] == 'L' && occupied == 0) { output[i][n] = '#';  NoChanges = false; }
                else if (input[i][n] == '#' && occupied >= 5) { output[i][n] = 'L'; NoChanges = false; }
                else output[i][n] = input[i][n];
            }
        }
        return output;
    }
}
