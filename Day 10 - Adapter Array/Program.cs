using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdapterArray;

class Program
{
    static void Main(string[] args)
    {
        var inputOrdered = File.ReadAllLines("input.txt").Select(i => int.Parse(i)).Order().ToList();
        inputOrdered.Insert(0, 0);
        inputOrdered.Add(inputOrdered.Max() + 3);
        Part1(inputOrdered);
        Part2(inputOrdered);
    }

    static void Part1(List<int> input){
        var count3 = 0;
        var count1 = 0;

        for(int i = 0; i < input.Count - 1; i++){
            var dif = input[i + 1] - input[i];
            if (dif == 1) count1++;
            if (dif == 3) count3++;
        }

        Console.WriteLine($"Part 1: {count1 * count3}");
    }

    static void Part2(List<int> input){
        Dictionary<int, long> pathCount = new Dictionary<int, long>();
        foreach(var num in input) pathCount.Add(num, 0);
        pathCount[input.First()] = 1;
        
        for(int i = 0; i < input.Count - 1; i++){
            for(int n = i + 1; n < i + 4 && n < input.Count; n++){
                if(input[n] - input[i] <= 3){
                    pathCount[input[n]] += pathCount[input[i]];
                }
                else{
                    break;
                }
            }
        }

        Console.WriteLine($"Distinct Arragements: {pathCount[input.Last()]}");
    }
}
