using System;

namespace PrimeNumbers
{
    public static class Prime
    {
        /// <summary>
        /// Return an array of primes.  The array is statically allocated with all primes less
        /// than MaxPrime.  More can be added dynamically if needed.
        /// </summary>
        /// <remarks>The getter function returns a clone of the backing field for safety.</remarks>
        public static int[] GetPrimes()
        {
            return (int[])Eratosthenes.GetPrimes().Clone();
        }

        /// <summary>
        /// Return an array of primes up to a given upper limit.
        /// </summary>
        /// <remarks>The getter function returns a clone of the backing field for safety.</remarks>
        public static int[] GetPrimes(int maxPrime)
        {
            return (int[])Eratosthenes.GetPrimes(maxPrime).Clone();
        }

        /// <summary>
        /// Test if a number is prime.  For now this function is limited by the length of
        /// the array of known primes (inputs up to maxPrime squared).
        /// </summary>
        /// <param name="value">Number to test.</param>
        /// <returns>True if the number is a prime. </returns>
        public static bool IsPrime(int value)
        {
            var primes = Eratosthenes.GetPrimes();
            var maxPrime = primes[^1];
            if (value > maxPrime * maxPrime)
                primes = Eratosthenes.GetPrimes((int)Math.Sqrt(value));

            if (value < 2)
                return false;

            // Check the array of known primes for small values
            if (value <= maxPrime)
                return 0 <= Array.BinarySearch(primes, value);

            // Look for a factor up to the sqrt of the values.
            var sqrt = (int)Math.Sqrt(value);
            foreach (var p in primes)
            {
                if (p > sqrt)
                    return true; // no prime factor will be found

                if (value % p == 0)
                    return value == p; // found a factor, but it may be the prime itself
            }

            return false;
        }
    }
}
