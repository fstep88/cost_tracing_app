using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace CurrencyDataBaseCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "symbol_list.txt");

            var lines = File.ReadAllLines(path);
            var array = new Dictionary<string, string>[lines.Length];
            var index = 0;

            foreach (var l in lines)
            {
                var split = l.Split(',')
                    .Where(snip => !string.IsNullOrEmpty(snip))
                    .ToArray();
                var dict = new Dictionary<string, string>()
                {
                    {"Uri", $"currencies/{split[2]}" },
                    {"Symbol", split[2] },
                    {"Name",  $"{split[0]} {split[1]}" },
                    {"Country", split[0] }
                };
                array[index++] = dict;
            }

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "CurrencyData.json");
            File.WriteAllText(outputPath,
                System.Text.Json.JsonSerializer.Serialize(array, typeof(Dictionary<string, string>[])));
        }
    }
}
