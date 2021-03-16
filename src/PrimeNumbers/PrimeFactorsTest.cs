using System;
using NUnit.Framework;

namespace PrimeNumbers
{
    [TestFixture]
    public class PrimeFactorsTest
    {
        [Test]
        public void IsPrimeTest()
        {
            // Very basic initial checks
            Assert.That(Prime.IsPrime(-2), Is.False);
            Assert.That(Prime.IsPrime(1), Is.False);
            Assert.That(Prime.IsPrime(2), Is.True);
            Assert.That(Prime.IsPrime(1999), Is.True);
            Assert.That(Prime.IsPrime(988027), Is.False); // 991*997

            // Check all the known primes
            var primes = Prime.GetPrimes();
            foreach (var i in primes)
            {
                Assert.That(Prime.IsPrime(i), Is.True);
            }
        }

        [Test]
        public void FactorTest()
        {
            var r32 = new[] { 5 };
            var r210 = new[] { 1, 1, 1, 1 };

            Assert.That(PrimeFactors.Factor(32), Is.EqualTo(r32));
            Assert.That(PrimeFactors.Factor(210), Is.EqualTo(r210));
        }

        [Test]
        public void InverseFactorTest()
        {
            for (var i = 1; i < 1000; i++)
            {
                var powers = PrimeFactors.Factor(i);
                Assert.That(PrimeFactors.Value(powers), Is.EqualTo(i));
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(43)]
        [TestCase(9999)]
        [TestCase(9876543)]
        [TestCase(987654321)]
        public void InverseFactorValue(int value)
        {
            var powers = PrimeFactors.Factor(value);
            Assert.That(PrimeFactors.Value(powers), Is.EqualTo(value));
        }

        [Test]
        public void CompareToTest()
        {
            // Higher powers of the same prime are greater
            Assert.That((new PrimeFactors(4)).CompareTo(new PrimeFactors(2)) > 0);
            // Higher primes are greater
            Assert.That((new PrimeFactors(3)).CompareTo(new PrimeFactors(2)) > 0);

            // Equality semantics
            for (var i = 1; i < 1000; i++)
            {
                var iFactors = new PrimeFactors(i);
                for (var j = 1; j < 1000; j++)
                {
                    var jFactors = new PrimeFactors(j);
                    if (i == j)
                        Assert.That(iFactors.CompareTo(jFactors), Is.EqualTo(0), "Prime Factors of equal numbers must compare equal.");
                    else
                        Assert.That(iFactors.CompareTo(jFactors), Is.Not.EqualTo(0), "Prime Factors of distinct numbers must compare not-equal.");
                }
            }
        }
    }
}
