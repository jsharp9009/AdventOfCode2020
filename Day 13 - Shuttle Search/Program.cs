using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace ShuttleSearch;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var target = int.Parse(input[0]);
        string[] busses = input[1].Split(',');

        Console.WriteLine("Part 1: {0}", Part1(target, busses));
        Console.WriteLine("Part 2: {0}", Part2(busses));
    }

    static long Part1(long target, string[] busses)
    {
        var track = new List<Tuple<int, long>>();
        foreach (var bus in busses)
        {
            if (int.TryParse(bus, out var id))
            {
                track.Add(new Tuple<int, long>(id, nextOccurence(target, id) - target));
            }
        }
        var closest = track.OrderBy(t => t.Item2).First();
        return closest.Item1 * closest.Item2;
    }

    public static long nextOccurence(long target, int id) => (target / id * id) + id;

    static long Part2(string[] input)
    {
        List<long> values = new List<long>();
        List<long> remainders = new List<long>();
        for (int i = 0; i < input.Length; i++)
        {
            if (int.TryParse(input[i], out int id)) {
                values.Add(id);
                remainders.Add(id - i);
            }
        }

        return ChineseRemainder(values, remainders);
    }

    //Slower solution
    // static long Part2(string[] busses)
    // {
    //     var step = busses.Select(t => int.Parse(t)).First();
    //     long timestamp = 100000000000000;
    //     for(int i = 1; i < busses.Length; i++){
    //         if(int.TryParse(busses[i], out int id)){
    //             while(nextOccurence(timestamp, id) - timestamp != i){
    //                 timestamp += step;
    //             }
    //             step *= id;
    //         }
    //     }
    //     return timestamp;
    // }

    static long ChineseRemainder(List<long> values, List<long> remainders){
        var moduliProduct = values.Aggregate(1L, (a, b) => a * b);

        var vals = new List<long>();

        for (int i = 0; i < values.Count; i++)
        {
            var coefficeint = moduliProduct / values[i];
            var x = coefficeint % values[i];
            long count = modInverse(x, values[i]);
            vals.Add(remainders[i] * coefficeint * count);
        }

        return vals.Aggregate(1L, (a, b) => a + b) % moduliProduct - 1;
    }

    static long modInverse(long a, long n)
    {
        long i = n;
        long v = 0L,  d = 1L;
        while (a > 0)
        {
            long t = i / a, x = a;
            a = i % x;
            i = x;
            x = d;
            d = v - t * x;
            v = x;
        }
        v %= n;
        if (v < 0) v = (v + n) % n;
        return v;
    }

}
