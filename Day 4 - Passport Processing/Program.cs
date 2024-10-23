using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PassportProcessing;

class Program
{
    static readonly string[] validEyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var passports = ParseInput(input);
        var part1Valid = passports.Where(V => ValidatePassport(V, true)).ToList();

        Console.WriteLine($"Part 1: {part1Valid.Count()} valid Passports.");

        var part2Valid = part1Valid.Where(v => ValidatePassportData(v)).Count();
        Console.WriteLine($"Part 2: {part2Valid} valid Passports.");
    }

    static bool ValidatePassport(Dictionary<string, string> passport, bool cidOptional){
        return passport.Count >= 7 && passport.ContainsKey("byr")
        && passport.ContainsKey("iyr")
        && passport.ContainsKey("eyr")
        && passport.ContainsKey("hgt")
        && passport.ContainsKey("hcl")
        && passport.ContainsKey("ecl")
        && passport.ContainsKey("pid")
        && (passport.ContainsKey("cid") || cidOptional);
    }

    static bool ValidatePassportData(Dictionary<string, string> passport){
        return ValidateYear(passport["byr"], 1920, 2002)
            && ValidateYear(passport["iyr"], 2010, 2020)
            && ValidateYear(passport["eyr"], 2020, 2030)
            && ValidateHeight(passport["hgt"])
            && ValidateHair(passport["hcl"])
            && ValidateEyeColor(passport["ecl"])
            && ValidatePassportID(passport["pid"]);
    }

    static bool ValidateYear(string year, int min, int max){
        if (year.Length != 4) return false;
        if(int.TryParse(year, out int y)){
            if(y >= min && y <= max) return true;
        }
        return false;
    }

    static bool ValidateHeight(string height){
        if(height.Contains("cm")){
            if(int.TryParse(height.Replace("cm", ""), out int h)){
                return h >= 150 && h <= 193;
            }
        }
        else{
            if(int.TryParse(height.Replace("in", ""), out int h)){
                return h >= 59 && h <= 76;
            }
        }
        return false;
    }

    static bool ValidateHair(string hair){
        return Regex.Match(hair, @"^#[a-f0-9]{6}$").Success;
    }

    static bool ValidateEyeColor(string color){
        return validEyeColors.Contains(color);
    }

    static bool ValidatePassportID(string id){
        return id.Length == 9 && int.TryParse(id, out int passportID);
    }

    static List<Dictionary<string, string>> ParseInput(string[] input){
        var result = new List<Dictionary<string, string>>();
        var current = new Dictionary<string, string>();
        foreach (var line in input){
            if (string.IsNullOrEmpty(line)) {
                result.Add(current);
                current = new Dictionary<string, string>();
                continue;
            };

            var parts = line.Split(" ");
            foreach(var part in parts){
                var pair = part.Split(":");
                current.Add(pair[0].Trim(), pair[1].Trim());
            }
        }

        result.Add(current);
        return result;
    }
}
