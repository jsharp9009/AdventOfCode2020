using System;
using System.IO;
using System.Linq;

namespace BinaryBoarding;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllLines("input.txt");
        var seatIds = input.Select(c => GetSeatID(c)).ToArray();
        Console.WriteLine($"Max Seat ID: {seatIds.Max()}");

        var sortedSeats = seatIds.Order().ToArray();
        for(int i = 0; i < sortedSeats.Length - 1; i++){
            if(sortedSeats[i] + 1 != sortedSeats[i + 1]){
                Console.WriteLine($"Your seat is {sortedSeats[i] + 1}");
                break;
            }
        }
    }

    static int GetSeatID(string ticket){
        var cmin = 0;
        var cmax = 127;
        var rowSequence = ticket.Substring(0, ticket.Length - 3);

        var rmin = 0;
        var rmax = 7;
        var columnSquence = ticket.Substring(ticket.Length - 3, 3);

        foreach(char s in rowSequence){
            
            switch (s)
            {
                case 'F':
                    cmax = cmin + (int)Math.Floor((cmax - cmin) / 2.0);
                    break;
                case 'B':
                    cmin += (int)Math.Ceiling((cmax - cmin) / 2.0);
                    break;
            }
        }

        foreach(char s in columnSquence){
            var temp = (int)Math.Floor(rmax / 2.0);
            switch (s)
            {
                case 'L':
                    rmax = rmin + (int)Math.Floor((rmax - rmin)/ 2.0);;
                    break;
                case 'R':
                    rmin += (int)Math.Ceiling((rmax - rmin)/ 2.0);;;
                    break;
            }
        }

        return cmin * 8 + rmin;
    }
}
