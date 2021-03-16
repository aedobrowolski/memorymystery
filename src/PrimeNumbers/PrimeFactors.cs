using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeNumbers
{
    public readonly struct PrimeFactors : IComparable<PrimeFactors>, IComparable, IEquatable<PrimeFactors>
    {
        /// <summary>
        /// Store a copy of the list of primes for local private access.  This list may grow as needed
        /// in the future, though currently it is not modified.
        /// </summary>
        private static int[] _primes = Prime.GetPrimes();

        /// <summary>
        /// Array of powers of prime factors of the Value.  The length of the
        /// array is such that the last entry is always non-zero.
        /// </summary>
        private readonly int[] _powers;

        /// <summary>
        /// Return one of the powers of prime factors for the represented value.
        /// </summary>
        public int this[int index] => (index < _powers.Length) ? _powers[index] : 0;

        /// <summary>
        /// Return the length of the array of prime powers.
        /// </summary>
        public int Length => _powers.Length;

        /// <summary>
        /// Return a representation of <paramref name="value"/> as a product of
        /// prime powers.  This array is uniquely determined by the fundamental theorem
        /// of prime numbers.
        /// </summary>
        /// <param name="value">A number to represent.</param>
        /// <exception cref="ArgumentOutOfRangeException">Zero and negative numbers are not represented.</exception>
        public PrimeFactors(int value)
        {
            _powers = Factor(value);
        }

        /// <summary>
        /// Using an array of powers construct a PrimeFactors object.
        /// </summary>
        /// <param name="powers">Powers of primes.  Trialing zeroes are allowed.</param>
        private PrimeFactors(int[] powers)
        {
            // Get the true length, excluding trailing zeroes.
            var length = powers.Length;
            while (length > 0 && powers[length - 1] == 0) length--;

            // Copy in the source array
            _powers = new int[length];
            for (var i = 0; i < length; i++)
                _powers[i] = powers[i];
        }

        /// <summary>
        /// Get the value from which this prime number was created.
        /// </summary>
        /// <returns>An integer representing the seed value.</returns>
        public int Value() => Value(_powers);

        /// <summary>
        /// Given an array of prime factor powers of a value, return the value.
        /// This function assumes the sequential order of primes.  So for example
        /// the array {2, 0, 0, 1} will return value 2^2*3^0*5^0*7^1 or 28.
        /// </summary>
        /// <param name="factors">Prime powers in order of Prime.GetPrimes().</param>
        /// <returns>The product of the given powers of the primes.</returns>
        public static int Value(int[] factors)
        {
            var value = 1;
            for (var i = 0; i < factors.Length; i++)
            {
                if (factors[i] == 0) continue;
                var p = _primes[i];
                for (var j = 0; j < factors[i]; j++)
                    value *= p; // Avoiding Math.Pow() on purpose.
            }
            return value;
        }

        /// <summary>
        /// Factor a number into an array of prime powers based on the primes in the array Primes.
        /// For example, 28 = 2^2 * 7^1.  The Primes array starts off {2, 3, 5, 7, ...}.
        /// So the returned array should consist of {2,0,0,1}.
        /// </summary>
        /// <param name="value">A positive number to factor.</param>
        /// <returns>An array of powers of prime factors, possibly zero length.</returns>
        public static int[] Factor(int value)
        {
            var originalValue = value;
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value to factor must not be less than 1.");

            var powers = new List<int>();

            for (var i = 0; i < _primes.Length && 1 < value; i++)
            {
                var power = 0;
                while (value % _primes[i] == 0)
                {
                    value /= _primes[i];
                    power++;
                }
                powers.Add(power);
            }

            if (value > 1)
            {
                _primes = Prime.IsPrime(value) ? Prime.GetPrimes(value) : Prime.GetPrimes((int)Math.Sqrt(value));
                return Factor(originalValue);
            }

            return powers.ToArray();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that
        /// indicates whether the current instance precedes, follows, or occurs in the same position in the
        /// sort order as the other object.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// The return value has these meanings:
        /// Value               Meaning
        /// Less than zero      This instance is less than <paramref name="obj"/>.
        /// Zero                This instance is equal to <paramref name="obj"/>.
        /// Greater than zero   This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is
        /// not the same type as this instance.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (!(obj is PrimeFactors))
                throw new ArgumentException("object is not a PrimeFactors");

            return CompareTo((PrimeFactors)obj);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// </returns>
        /// <remarks>
        /// The greatest PrimeFactor is the one with the highest prime factor.  In the event of
        /// a tie, the powers of the greatest prime factor are compared.  If those are also
        /// equal then the next highest prime factor of each PrimeFactor is considered.  If the
        /// set of prime factors and their powers is equal then the objects are equal.
        /// </remarks>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(PrimeFactors other)
        {
            if (_powers.Length != other._powers.Length)
                return _powers.Length - other._powers.Length;

            for (var i = _powers.Length - 1; i >= 0; i--)
            {
                var diff = _powers[i] - other._powers[i];
                if (diff != 0)
                    return diff;
            }

            return 0;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.  The
        /// equality comparison uses value semantics.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PrimeFactors other)
        {
            return (_powers.Length == other._powers.Length) && (CompareTo(other) == 0);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of any type.  The
        /// equality comparison uses value semantics.
        /// </summary>
        /// <returns>
        /// true if the <paramref name="obj"/> parameter object is of the same type and
        /// equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">An object to compare with this object.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(PrimeFactors)) return false;
            return Equals((PrimeFactors)obj);
        }

        /// <summary>
        /// Generate a hash code for this PrimeFactors object.
        /// </summary>
        /// <returns>an int32 hash value</returns>
        public override int GetHashCode()
        {
            if (_powers.Length == 0)
                return 0;

            var num = 0;
            foreach (var p in _powers)
            {
                var num3 = (num >> 0x1c) & 0xff;
                num <<= 4;
                num = num ^ p ^ num3;
            }
            return num;
        }

        /// <summary>
        /// Compare two PrimeFactors objects for equality.
        /// </summary>
        /// <param name="left">First object.</param>
        /// <param name="right">Second object.</param>
        /// <returns>true if the two objects are equal</returns>
        public static bool operator ==(PrimeFactors left, PrimeFactors right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compare two PrimeFactors objects for inequality.
        /// </summary>
        /// <param name="left">First object.</param>
        /// <param name="right">Second object.</param>
        /// <returns>true if the two objects are not equal</returns>
        public static bool operator !=(PrimeFactors left, PrimeFactors right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Multiply two PrimeFactors objects using normal multiplication semantics on their values.
        /// </summary>
        /// <param name="left">First object.</param>
        /// <param name="right">Second object.</param>
        /// <returns>A new PrimeFactors object whose value is the product of the values of the two operands.</returns>
        public static PrimeFactors operator *(PrimeFactors left, PrimeFactors right)
        {
            var result = new int[Math.Max(left.Length, right.Length)];
            for (var i = 0; i < result.Length; i++)
                result[i] = left[i] + right[i]; // to multiply, add exponents
            return new PrimeFactors(result);
        }

        /// <summary>
        /// Divide two PrimeFactors objects using normal division semantics on their values.
        /// </summary>
        /// <param name="left">First object.</param>
        /// <param name="right">Second object.</param>
        /// <returns>A new PrimeFactors object whose value is the product of the values of the two operands.</returns>
        public static PrimeFactors operator /(PrimeFactors left, PrimeFactors right)
        {
            var result = new int[Math.Max(left.Length, right.Length)];
            for (var i = 0; i < result.Length; i++)
                result[i] = left[i] - right[i]; // to divide, subtract exponents
            return new PrimeFactors(result);
        }

        /// <summary>
        /// Raise a PrimeFactors to a power using normal power semantics on the value.
        /// </summary>
        /// <param name="left">PrimeFactors object.</param>
        /// <param name="pow">Power to which <paramref name="left"/> is raised.</param>
        /// <returns>A new PrimeFactors object whose value is the first operand raised to the given power.</returns>
        public static PrimeFactors operator ^(PrimeFactors left, int pow)
        {
            var result = new int[left.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = left[i] * pow; // to raise to a power, multiply exponents
            return new PrimeFactors(result);
        }

        /// <summary>
        /// Show the powers of primes as a simple string of the form: 1 * 2^5 * ...
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            var i = -1;
            return _powers.Aggregate("1", (x, exp) =>
            {
                i++;
                if (exp == 0) return x;
                if (exp == 1) return x + " * " + _primes[i];
                return x + " * " + _primes[i] + "^" + exp;
            });
        }
    }
}
