using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicketTranslation;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        (var ranges, var myTicket, var nearbyTickets) = parseInput(input);

        List<int> badNumbers = new List<int>();

        foreach(var ticket in nearbyTickets){
            var badNums = ticket.Key.Where(n => !ranges.Any(r => r.Value.Any(c => c.InRange(n))));
            if (badNums.Any()) {
                badNumbers.AddRange(badNums);
                nearbyTickets[ticket.Key] = false;
            }
        }

        Console.WriteLine("Part 1: {0}", badNumbers.Sum());

        var potentialPostions = new Dictionary<string, List<int>>();
        var notPostions = new Dictionary<string, List<int>>();
        foreach(var r in ranges){
            potentialPostions.Add(r.Key, new List<int>());
            notPostions.Add(r.Key, new List<int>());
        }

        foreach(var ticket in nearbyTickets.Where(c => c.Value)){
            for(int i = 0; i < ticket.Key.Count; i++){
                foreach(var r in ranges){
                    if (r.Value.Any(s => s.InRange(ticket.Key[i]))) {
                        if (!potentialPostions[r.Key].Contains(i)
                            && !notPostions[r.Key].Contains(i))
                            potentialPostions[r.Key].Add(i);
                    }
                    else {
                        potentialPostions[r.Key].Remove(i);
                        if (!notPostions[r.Key].Contains(i)){
                            notPostions[r.Key].Add(i);
                        }
                    }
                }
            }

            if (potentialPostions.All(p => p.Value.Count == 1)) break;
        }

        var changed = true;
        while(changed){
            changed = false;
            var only1 = potentialPostions.Where(p => p.Value.Count == 1);
            foreach(var p in only1) {
                foreach(var o in potentialPostions){
                    if (o.Equals(p)) continue;
                    int removed = o.Value.RemoveAll(i => p.Value.Contains(i));
                    if(removed > 0)
                        changed = true;
                }
            }
        }

        var departures = potentialPostions.Where(c => c.Key.Contains("departure"));

        var depValues = departures.Select(d => myTicket[d.Value.First()]).Aggregate(1L, (a, b) => a * b);

        Console.WriteLine("Part 2: {0}", depValues);
    }

    static (Dictionary<string, List<Range>>, List<int> myTicket, Dictionary<List<int>, bool> nearbyTickets) parseInput(string[] input){
        var step = 0;
        Dictionary<string, List<Range>> ranges = new Dictionary<string, List<Range>>();
        List<int> myTicket = new List<int>();
        Dictionary<List<int>, bool> nearbyTickets = new Dictionary<List<int>, bool>();

        foreach(var line in input){
            switch(step){
                case 0:
                    if(string.IsNullOrEmpty(line)){
                        step = 1;
                        continue;
                    }
                    var myRanges = new List<Range>();
                    var matches = Regex.Matches(line, "([0-9]+-[0-9]+)");
                    foreach(Match match in matches){
                        var parts = match.Value.Split("-");
                        myRanges.Add(new Range(int.Parse(parts[0]), int.Parse(parts[1])));
                    }
                    ranges.Add(line.Split(":")[0], myRanges);
                    break;
                case 1:
                    if (line.Equals("your ticket:", StringComparison.InvariantCultureIgnoreCase)) continue;
                    if(string.IsNullOrEmpty(line)){
                        step = 2;
                        continue;
                    }
                    
                    myTicket = line.Split(",").Select(i => int.Parse(i)).ToList();
                    break;
                case 2:
                    if (line.Equals("nearby tickets:", StringComparison.InvariantCultureIgnoreCase)) continue;
                    nearbyTickets.Add(line.Split(",").Select(i => int.Parse(i)).ToList(), true);
                    break;
            }
        }

        return (ranges, myTicket, nearbyTickets);
    }
}

record Range{
    public int Min{ get; set; }
    public int Max{ get; set; }

    public Range(int min, int max){
        Min = min;
        Max = max;
    }

    public bool InRange(int toCheck){
        return Min <= toCheck && toCheck <= Max;
    }
}
