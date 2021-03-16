using NUnit.Framework;

namespace PrimeNumbers
{
    [TestFixture]
    public class EratosthenesTest
    {
        [Test]
        public void AllPrimes()
        {
            // Make sure that all primes and only primes are returned
            var upToOneHundred = Eratosthenes.Sieve(10000);
            var primes = Eratosthenes.GetPrimes();
            for (var i = 0; i < upToOneHundred.Length; i++)
            {
                Assert.That(upToOneHundred[i], Is.EqualTo(primes[i]));
            }
        }

        [Test]
        public void IncludesRequest()
        {
            // Make sure the array includes the prime requested
            var p = 5;
            var upToP = Eratosthenes.Sieve(p);
            Assert.That(upToP[^1], Is.GreaterThanOrEqualTo(p));
        }
    }
}
