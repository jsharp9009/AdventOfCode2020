using System;
using System.IO;
using System.Linq;

namespace EncodingError;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt").Select(l => long.Parse(l)).ToArray();
        var firstBad = FindFirstBadRecord(input, 25);
        Console.WriteLine($"First bad number: {firstBad}");

        var weakness = FindWeakness(input, firstBad);
        Console.WriteLine($"Weakness is: {weakness}");  
    }

    static long FindWeakness(long[] input, long badNumber){
        var end = Array.IndexOf(input, badNumber);
        long min = long.MaxValue;
        long max = long.MinValue;
        for(int i = 0; i < end; i++){
            long total = 0;
            for(int n = i; n < end; n++){
                var num = input[n];
                total += num;
                if(num < min){
                    min = num;
                }
                if(num > max){
                    max = num;
                }

                if(total == badNumber){
                    return max + min;
                }
                if(total > badNumber){
                    min = long.MaxValue;
                    max = long.MinValue;
                    break;
                }
            }
        }
        return long.MinValue;
    }

    static long FindFirstBadRecord(long[] input, long bufferSize){
        for(long i = bufferSize; i < input.Length; i++){
            bool found = false;
            for(long n = i - bufferSize; n < i + bufferSize + 1; n++){
                for (long t = i - bufferSize; t < i + bufferSize + 1; t++)
                {
                    if (input[t] == input[n]) continue;
                    if(input[t] + input[n] == input[i]){
                        found = true;
                        break;
                    }
                    if (found) break;
                }
            }
            if (!found) return input[i];
        }
        return long.MaxValue;
    }
}
