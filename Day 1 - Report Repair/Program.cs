using System;
using System.IO;
using System.Linq;

namespace ReportRepair;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var numbers = input.Select(i => int.Parse(i)).ToArray();

        SolvePart1(numbers);
        SolvePart2(numbers);    
    }

    static void SolvePart1(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            for (int n = i + 1; n < numbers.Length; n++)
            {
                if (numbers[i] + numbers[n] == 2020)
                {
                    Console.WriteLine("Part 1: " + (numbers[i] * numbers[n]));
                    return;
                }
            }
        }
    }

    static void SolvePart2(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            for (int n = i + 1; n < numbers.Length; n++)
            {
                for (int t = n + 1; t < numbers.Length; t++)
                    if (numbers[i] + numbers[n] + numbers[t] == 2020)
                    {
                        Console.WriteLine("Part 1: " + (numbers[i] * numbers[n] * numbers[t]));
                        return;
                    }
            }
        }
    }
}
