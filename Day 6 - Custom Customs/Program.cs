using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomCustoms;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var forms = parseInput(input);

        var total = forms.Sum(x => CountYes(x));
        Console.WriteLine($"Total Yes: {total}");

        total = forms.Sum(x => CountAllYes(x));
        Console.WriteLine($"Total All Yes: {total}");
    }

    static int CountYes(Tuple<char[], int> input){
        return input.Item1.Distinct().Count();
    }

    static int CountAllYes(Tuple<char[], int> input){
        var distinct = input.Item1.Distinct();
        var count = 0;
        foreach(var c in distinct){
            if (input.Item1.Count(i => i == c) == input.Item2) count++;
        }
        return count;
    }

    static List<Tuple<char[], int>> parseInput(string[] input){
        var result = new List<Tuple<char[], int>>();
        var current = new StringBuilder();
        int count = 0;
        foreach (var line in input){
            if(string.IsNullOrEmpty(line)){
                result.Add(new Tuple<char[], int>(current.ToString().ToCharArray(), count));
                current = new StringBuilder();
                count = 0;
                continue;
            }

            current.Append(line);
            count++;
        }
        result.Add(new Tuple<char[], int>(current.ToString().ToCharArray(), count));
        return result;
    }
}
