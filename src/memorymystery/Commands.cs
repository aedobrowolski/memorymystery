using System;
using System.Linq;
using PrimeNumbers;

namespace MemoryMystery
{
    internal static partial class Program
    {
        private const int DefaultTimesToRepeat = 100;

        /// <summary>
        /// Repeat command a number of times.
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>The last result</returns>
        private static object Repeat(string[] args)
        {
            var j = 0;
            var times = args[j++].ToLowerInvariant() switch
            {
                "times" => int.Parse(args[j++]),
                "repeat" => DefaultTimesToRepeat,
            };

            object lastResult = null;
            for (var i = 0; i < times; i++) lastResult = Invoke(args[j..]);

            return lastResult;
        }

        /// <summary>
        /// Test if args[1] is prime
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>true if the value is prime</returns>
        private static object IsPrime(string[] args) => Prime.IsPrime(int.Parse(args[1]));

        /// <summary>
        /// Factor args[1] into prime factors
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>Array of prime factor powers</returns>
        private static object Factor(string[] args) => new PrimeFactors(int.Parse(args[1]));

        /// <summary>
        /// Return all proper divisors of a value
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>An enumerable of all divisors.</returns>
        private static object Divisors(string[] args) => Divisor.Divisors(int.Parse(args[1]));

        /// <summary>
        /// Count the divisors of a number.
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>Number of divisors.</returns>
        private static object CountDivisors(string[] args) => Divisor.CountDivisors(int.Parse(args[1]));

        /// <summary>
        /// Multiply two numbers
        /// </summary>
        /// <param name="args">Command line tokens</param>
        /// <returns>Product</returns>
        private static object Multiply(string[] args) => int.Parse(args[1]) * int.Parse(args[2]);

        /// <summary>
        /// Get the last 100 items from history
        /// </summary>
        /// <returns>History items</returns>
        private static object History(string[] _) => HistoryItems.Where(i => i.CmdKey != "history").Skip(HistoryItems.Count - 100);

        /// <summary>
        /// Clear all history items.
        /// </summary>
        /// <returns>Message showing number of items cleared</returns>
        private static object Clear(string[] _)
        {
            var count = HistoryItems.Count;
            HistoryItems.Clear();
            return $"Cleared {count} history items.";
        }

        private static object MemoryUsed(string[] _)
        {
            var gc = GC.GetGCMemoryInfo();
            var memoryUsed = GC.GetTotalMemory(false);
            var memoryFree = gc.TotalAvailableMemoryBytes;
            return $"{memoryUsed}/{memoryUsed + memoryFree}";
        }

        private static object GarbageCollect(string[] args)
        {
            GC.Collect();
            if (args.Length > 1 && bool.Parse(args[1]))
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
                return "collected with finalizers";
            }

            return "collected";
        }
    }
}