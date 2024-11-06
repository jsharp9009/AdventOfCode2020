using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DockingData;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var program = ParseInput(input);
        var total = Part1(program);
        Console.WriteLine("Part 1: {0}", total);
        total = Part2(program);
        Console.WriteLine("Part 2: {0}", total);
    }

    static long Part1(List<object> input){
        Dictionary<long, long> memory = new Dictionary<long, long>();
        Mask? current = new Mask("");
        foreach(object rule in input){
            if (rule is Mask) current = (Mask)rule;
            else{
                var memSet = rule as SetMemory;
                var stringBinary = Convert.ToString(memSet.ToWrite, 2).ToCharArray();
                stringBinary = Expand(stringBinary, 36);
                
                for(int i = 0; i < current.Changes.Length; i++){
                    var c = current.Changes[i];
                    if (Char.IsDigit(c))
                        stringBinary[i] = c;
                }
                
                if(memory.ContainsKey(memSet.MemoryLocation)){
                    memory[memSet.MemoryLocation] = Convert.ToInt64(new string(stringBinary), 2);
                }
                else{
                    memory.Add(memSet.MemoryLocation, Convert.ToInt64(new string(stringBinary), 2));
                }
            }
        }
        return memory.Aggregate(0L, (a, b) => a + b.Value);
    }

        static long Part2(List<object> input){
        Dictionary<long, long> memory = new Dictionary<long, long>();
        Mask? current = new Mask("");
        foreach(object rule in input){
            if (rule is Mask) current = (Mask)rule;
            else {

                var memSet = rule as SetMemory;
                var stringBinary = Convert.ToString(memSet.MemoryLocation, 2).ToCharArray();
                stringBinary = Expand(stringBinary, 36);

                List<char[]> toWrite = new List<char[]>(){
                    stringBinary
                };

                for (int i = 0; i < current.Changes.Length; i++) {
                    var c = current.Changes[i];
                    if (c == '0') continue;
                    else if (c == '1') {
                        toWrite.ForEach(c => c[i] = '1');
                    }
                    else {
                        List<char[]> newToWrite = new List<char[]>();
                        foreach (var l in toWrite) {
                            l[i] = '1';
                            char[] copy = new char[l.Length];
                            l.CopyTo(copy, 0);
                            newToWrite.Add(copy);

                            l[i] = '0';
                            newToWrite.Add(l);
                            
                        }
                        toWrite = newToWrite;
                    }
                }

                foreach (var w in toWrite)
                {
                    var location = Convert.ToInt64(new string(w), 2);

                    if (memory.ContainsKey(location))
                    {
                        memory[location] = memSet.ToWrite;
                    }
                    else
                    {
                        memory.Add(location, memSet.ToWrite);
                    }
                }
            }
        }
        return memory.Aggregate(0L, (a, b) => a + b.Value);
    }

    static char[] Expand(char[] input, int size){
        var ret = new char[size];
        var remainder = size - input.Length;
        for(int i = 0; i < size; i++){
            if (i - remainder >= 0)
                ret[i] = input[i - remainder];
            else
                ret[i] = '0';
        }
        
        return ret;
    }

    static List<object> ParseInput(string[] input){
        var output = new List<object>();
        foreach(var line in input){
            if(line.StartsWith("mask = ")){
                output.Add(new Mask(line.Substring(7)));
            }
            else{
                var matches = Regex.Matches(line, "([0-9]+)");
                output.Add(new SetMemory(int.Parse(matches[0].Value), int.Parse(matches[1].Value)));
            }
        }
        return output;
    }

}

record Mask{
    public Mask(string changes){
        this.Changes = changes;
    }
    public string Changes { get; set; }        
}

record SetMemory{
    public int MemoryLocation { get; set; }
    public int ToWrite { get; set; }

    public SetMemory(int memLocation, int toWrite){
        MemoryLocation = memLocation;
        ToWrite = toWrite;
    }
}