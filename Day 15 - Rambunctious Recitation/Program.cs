using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RambunctiousRecitation;

class Program
{
    static void Main(string[] args)
    {
        
        var input = File.ReadAllLines("input.txt")[0].Split(',').Select(i => int.Parse(i)).ToArray();
        var lastSpoken = PlayMemory(input, 2020);
        Console.WriteLine("Last Spoken Part 1: " + lastSpoken);

        lastSpoken = PlayMemory(input, 30000000);
        Console.WriteLine("Last Spoken Part 2: " + lastSpoken);
    }

    private static int PlayMemory(int[] input, int turnToPlay){
        Dictionary<int, List<int>> numberCounts = new Dictionary<int, List<int>>();
        int lastSpoken = 0;
        for (int i = 0; i < input.Length; i++) {
            numberCounts.Add(input[i], new List<int>(){i});
            lastSpoken = input[i];
        }

        for (int i = input.Length; i < turnToPlay; i++) {
            if (numberCounts[lastSpoken].Count == 1) {
                IncrementNumber(numberCounts, 0, i);
                lastSpoken = 0;
            }
            else { 
                var turns = numberCounts[lastSpoken];
                var newNumber = turns[turns.Count - 1] - turns[turns.Count - 2];
                IncrementNumber(numberCounts, newNumber, i);
                lastSpoken = newNumber;
            }
        }
        return lastSpoken;
    }

    static void IncrementNumber(Dictionary<int, List<int>> numberCounts, int num, int turn){
        if(numberCounts.ContainsKey(num)){
            numberCounts[num].Add(turn);
        }
        else{
            numberCounts.Add(num, new List<int>() { turn });
        }
    }
}
