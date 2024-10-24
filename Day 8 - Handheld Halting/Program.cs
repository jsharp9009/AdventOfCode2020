using System;
using System.Collections.Generic;
using System.IO;

namespace HandheldHalting;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var commands = ParseInput(input);
        var acc = GetAccumulatorAtLoop(commands, out var terminated);
        Console.WriteLine($"Accumulator at Loop: {acc}");

        acc = SolvePart2(commands);
        Console.WriteLine($"Correct Accumulator after fix: {acc}");
    }

    static int SolvePart2(List<Tuple<CommandType, int>> commands){
        for(int i = 0; i < commands.Count; i++){
            var command = commands[i];
            if (command.Item1 == CommandType.ACC) continue;
            if (command.Item1 == CommandType.JMP && command.Item2 == 0) continue;

            var copy = new List<Tuple<CommandType, int>>(commands);
            copy[i] = new Tuple<CommandType, int>(copy[i].Item1 == CommandType.NOP ? CommandType.JMP : CommandType.NOP, copy[i].Item2);

            var acc = GetAccumulatorAtLoop(copy, out var terminated);
            if (terminated) return acc;
        }
        return int.MinValue;
    }

    static int GetAccumulatorAtLoop(List<Tuple<CommandType, int>> commands, out bool terminated){
        var accumulator = 0;
        var runCommands = new List<int>();
        var currentPosition = 0;
        terminated = false;

        while(true){
            if(currentPosition >= commands.Count){
                terminated = true;
                return accumulator;
            }
            if(runCommands.Contains(currentPosition)){
                return accumulator;
            }
            runCommands.Add(currentPosition);
            var command = commands[currentPosition];
            switch(command.Item1){
                case CommandType.JMP:
                    currentPosition += command.Item2;
                    break;
                case CommandType.ACC:
                    accumulator += command.Item2;
                    goto default;
                default:
                    currentPosition++;
                    break;
            }            
        }
    }

    static List<Tuple<CommandType, int>> ParseInput(string[] input){
        var commands = new List<Tuple<CommandType,int>>();
        foreach(var line in input){
            var split = line.Split(' ');
            switch(split[0]){
                case "acc":
                    commands.Add(Tuple.Create(CommandType.ACC, int.Parse(split[1])));
                    break;
                case "jmp":
                    commands.Add(Tuple.Create(CommandType.JMP, int.Parse(split[1])));
                    break;
                case "nop":
                    commands.Add(Tuple.Create(CommandType.NOP, int.Parse(split[1])));
                    break;
            }
        }
        return commands;
    }

    enum CommandType{
        ACC,
        JMP,
        NOP
    }
}