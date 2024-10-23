using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordPhilosophy;

class Program
{
    private static readonly string INPUT_REGEX = @"(\d+)-(\d+) ([a-z]): ([a-z]+)";

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var rules = ParseInput(input);
        SolvePart1(rules);
        SolvePart2(rules);
    }

    static void SolvePart1(List<PasswordRule> rules){
        int valid = 0;
        foreach (var rule in rules){
            var count = rule.Password.Count(c => c == rule.Char);
            if(count >= rule.Min && count <= rule.Max){
                valid++;
            }
        }
        Console.WriteLine($"Part 1: {valid} valid passwords.");
    }

    static void SolvePart2(List<PasswordRule> rules){
        int valid = 0;
        foreach (var rule in rules){
            var num1Valid = rule.Password[rule.Min - 1] == rule.Char;
            var num2Valid = rule.Password[rule.Max - 1] == rule.Char;

            if(num1Valid ^ num2Valid){
                valid++;
            }
        }
        Console.WriteLine($"Part 2: {valid} valid passwords.");
    }

    static List<PasswordRule> ParseInput(string[] input){
        var rules = new List<PasswordRule>();
        foreach (var line in input){
            var match = Regex.Match(line, INPUT_REGEX);
            if (match.Success){
                rules.Add(new PasswordRule(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    match.Groups[3].Value[0],
                    match.Groups[4].Value
                ));
            }
        }
        return rules;
    }
}

record PasswordRule{
    public PasswordRule(int min, int max, char c, string password)
    {
        Min = min;
        Max = max;
        Char = c;
        Password = password;
    }

    public int Min { get; set; }
    public int Max { get; set; }
    public char Char { get; set; }
    public string Password { get; set; }
}
