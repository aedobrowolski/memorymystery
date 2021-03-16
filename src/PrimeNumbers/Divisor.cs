using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeNumbers
{
    /// <summary>
    /// The methods in this class deal with the divisors of a number.
    /// A proper divisor must be less than the number itself.  One is
    /// the first proper divisor of every number.  For example the proper
    /// divisors of 28 are 1, 2, 4, 7, 14.  These happen to add up to
    /// 28 so it is called a perfect number.
    /// </summary>
    public static class Divisor
    {
        /// <summary>
        /// Given an array of powers of prime factors, like the one returned by Factor(),
        /// return the number of possible divisors of the original number, including
        /// one and the number itself.
        /// </summary>
        /// <param name="factors"></param>
        /// <returns>the number of possible divisors of the original number.</returns>
        public static int CountDivisors(int[] factors)
        {
            var div = 1;
            foreach (var f in factors)
                div *= f + 1;

            return div;
        }
        /// <summary>
        /// Count the number of possible divisors of a number, including 1 and the number itself.
        /// </summary>
        /// <param name="value">The number to test.</param>
        /// <returns>the number of possible divisors of the original number.</returns>
        public static int CountDivisors(int value)
        {
            return CountDivisors(PrimeFactors.Factor(value));
        }

        /// <summary>
        /// Iterate over the divisors of the number whose prime
        /// factorization has been passed in.  Iteration will start with
        /// powers of the lowest prime factor and increase by adding
        /// powers of higher prime factors.  For example since 28 = 2^2 * 7^1
        /// the iteration will return: 1, 2, 4, 7, 14, 28.
        /// </summary>
        /// <param name="factors">Array of exponents for corresponding primes of GetPrimes()</param>
        /// <returns>The set of divisors of the number.</returns>
        public static IEnumerable<int> Divisors(int[] factors)
        {
            var allPrimes = Prime.GetPrimes();

            // Compress the factors into a tighter array for iteration.
            // Consider only primes whose exponent is positive.
            var count = factors.Count(exp => exp > 0);
            var maxexp = new int[count];
            var maxpow = new int[count];
            var primes = new int[count];
            for (int i = 0, j = 0; i < count; i++, j++)
            {
                while (j < factors.Length && factors[j] <= 0) j++; // skip zeros
                maxexp[i] = factors[j];
                primes[i] = allPrimes[j];
                maxpow[i] = (int)Math.Pow(primes[i], maxexp[i]);
            }

            var divisor = 1;
            var exponent = new int[count]; // Prime exponents in the divisor.
            yield return divisor;

        restart:
            for (var i = 0; i < count; i++)
            {
                var p = primes[i];
                if (exponent[i] < maxexp[i])
                {
                    exponent[i]++;
                    divisor *= p;
                    yield return divisor;
                    goto restart;
                }
                // reduce exponent to zero
                exponent[i] = 0;
                divisor /= maxpow[i];
            }
        }

        /// <summary>
        /// Generate an iterator over the divisors of the number passed in.
        /// Iteration will proceed as for <see cref="Divisors(int[])"/>.
        /// </summary>
        /// <param name="value">Value whose divisors are desired</param>
        /// <returns>An enumerator over all divisors of the number.</returns>
        public static IEnumerable<int> Divisors(int value)
        {
            return Divisors(PrimeFactors.Factor(value));
        }

        /// <summary>
        /// Generate an iterator over the divisors of the number whose prime
        /// factorization has been passed in, other than the number itself.
        /// Iteration will proceed as for <see cref="Divisors(int[])"/>.
        /// </summary>
        /// <param name="factors">Prime factors of the value whose divisors are desired</param>
        /// <returns>An enumerator over all proper divisors of the number.</returns>
        public static IEnumerable<int> ProperDivisors(int[] factors)
        {
            var value = PrimeFactors.Value(factors);
            foreach (var i in Divisors(factors))
                if (i < value) yield return i;
        }

        /// <summary>
        /// Generate an iterator over the proper divisors of the number passed in,
        /// other than the number itself.
        /// Iteration will proceed as for <see cref="Divisors(int[])"/>.
        /// </summary>
        /// <param name="value">Value whose divisors are desired</param>
        /// <returns>An enumerator over all proper divisors of the number.</returns>
        public static IEnumerable<int> ProperDivisors(int value)
        {
            var factors = PrimeFactors.Factor(value);
            foreach (var i in Divisors(factors))
                if (i < value) yield return i;
        }

        /// <summary>
        /// Get the perfection of a number.  A number whose proper divisors
        /// add up to the number is called "perfect".  If the sum is less then the
        /// number it is called "deficient".  Otherwise it is called "abundant".
        /// </summary>
        /// <param name="factors">Prime factor powers of the number</param>
        /// <returns>-1 if deficient, 0 if perfect, 1 if abundant</returns>
        public static int Perfection(int[] factors)
        {
            var sum = 0;
            var value = PrimeFactors.Value(factors);
            foreach (var i in ProperDivisors(factors))
            {
                sum += i;
                if (sum > value) return 1;
            }
            return (sum < value) ? -1 : 0;
        }

        /// <summary>
        /// Get the perfection of a number.  A number whose proper divisors
        /// add up to the number is called "perfect".  If the sum is less then the
        /// number it is called "deficient".  Otherwise it is called "abundant".
        /// </summary>
        /// <param name="value">The number</param>
        /// <returns>-1 if deficient, 0 if perfect, 1 if abundant</returns>
        public static int Perfection(int value)
        {
            return Perfection(PrimeFactors.Factor(value));
        }
    }
}
