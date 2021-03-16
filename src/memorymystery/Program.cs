using System;
using System.Collections.Generic;

namespace MemoryMystery
{
    internal static partial class Program
    {
        private const string Prompt = "\n\n> ";

        private delegate object Command(string[] args);
        private static List<HistoryItem> HistoryItems { get; } = new List<HistoryItem>();

        private static Dictionary<string, Command> Commands { get; }  =
            new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase)
            {
                ["isPrime"] = IsPrime,
                ["factor"] = Factor,
                ["divisors"] = Divisors,
                ["countDivisors"] = CountDivisors,
                ["multiply"] = Multiply,
                ["times"] = Repeat,
                ["repeat"] = Repeat,
                ["history"] = History,
                // ["clear"] = Clear,
                // ["memory"] = MemoryUsed,
                // ["collect"] = GarbageCollect,
            };

        /// <summary>
        /// Implement a REPL loop in the console.
        /// </summary>
        public static void Main()
        {
            Console.Write($"Type a command or enter 'x' to exit (use numbers up to {int.MaxValue})" + Prompt);
            string line;
            while ((line = Console.ReadLine()) != "x") 
                Console.Write(Invoke(line) + Prompt);
        }

        /// <summary>
        /// Invoke a command and return the result
        /// </summary>
        /// <param name="line">Line to invoke</param>
        /// <returns>String representation of the result</returns>
        private static string Invoke(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return "";

            var args = line.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);
            return Show(Invoke(args));
        }

        /// <summary>
        /// Invoke a parsed command line and return the result adding both to history.
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>object representing the result</returns>
        private static object Invoke(string[] args)
        {
            var cmdKey = args[0];
            if (!Commands.ContainsKey(cmdKey))
                return $"Unknown command ({cmdKey}). [{string.Join(", ", Commands.Keys)}]";

            var item = new HistoryItem(args);
            try
            {
                HistoryItems.Add(item);
                var result = Commands[cmdKey](args);
                item.Result = result;
                return result;
            }
            catch (Exception e)
            {
                item.Result = e.Message;
                return e.Message;
            }
        }

        /// <summary>
        /// Convert a result object to a string.
        /// </summary>
        public static string Show(object obj)
        {
            switch (obj)
            {
                case IEnumerable<HistoryItem> history: 
                    return string.Join("\n", history);
                case IEnumerable<int> strings:
                    return string.Join(" ", strings);
                case null:
                    return "";
                default:
                    return obj.ToString();
            }
        }
    }
}
