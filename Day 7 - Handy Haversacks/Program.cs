using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandyHaversacks;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var rules = ParseInput(input);
        var bagsContainingGold = BagsThatCanCarry(rules, "shiny gold");
        Console.WriteLine($"{bagsContainingGold.Count} bags can contain shiny gold.");

        var carrying = BagsThatAreCarriedBy(rules, "shiny gold");
        Console.WriteLine($"{carrying} carried by shiny gold");
    }

    static int BagsThatAreCarriedBy(Dictionary<string, Dictionary<string, int>> rules, string name){
        var carried = rules[name];
        var count = 0;
        foreach(var c in carried){
            count += c.Value + (BagsThatAreCarriedBy(rules, c.Key) * c.Value);
        }
        return count;
    }

    static List<string> BagsThatCanCarry(Dictionary<string, Dictionary<string, int>> rules, string toFind){
        var allBags = BagsThatCanDirectlyCarry(rules, toFind);
        var bagsToCheck =  new Queue<string>(allBags);
        while(bagsToCheck.Count > 0){
            var bag = bagsToCheck.Dequeue();
            var possible = BagsThatCanDirectlyCarry(rules, bag);
            foreach(var b in possible){
                if(!allBags.Contains(b)){
                    allBags.Add(b);
                    bagsToCheck.Enqueue(b);
                }
            }
        }
        return allBags;
    }

    static List<string> BagsThatCanDirectlyCarry(Dictionary<string, Dictionary<string, int>> rules, string bag){
        return rules.Where(r => r.Value.ContainsKey(bag)).Select(r => r.Key).ToList();
    }

    static Dictionary<string, Dictionary<string, int>> ParseInput(string[] input){
        Dictionary<string,Dictionary<string, int>> rules = new Dictionary<string,Dictionary<string, int>>();
        foreach(var line in input){
            if(line.IndexOf("no other bags") > -1){
                var split = line.Split(" ", 3);
                rules.Add(split[0] + " " + split[1], new Dictionary<string, int>());
                continue;
            }
            var matches = Regex.Matches(line, "(\\d*[a-z ]+) bag");
            Dictionary<string, int> rule = new Dictionary<string, int>();
            for (int i = 1; i < matches.Count; i++){
                var match = matches[i];
                var group = match.Groups[1];
                var split = group.Value.Split(' ', 2);
                rule.Add(split[1], int.Parse(split[0]));
            }
            rules.Add(matches[0].Groups[1].Value, rule);
        }
        return rules;
    }
}
