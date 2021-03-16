using System.Linq;
using NUnit.Framework;

namespace PrimeNumbers
{
    [TestFixture]
    public class DivisorTest
    {
        [Test]
        public void CountDivisors()
        {
            Assert.That(Divisor.CountDivisors(28), Is.EqualTo(6));
            Assert.That(Divisor.CountDivisors(45360), Is.EqualTo(100));
        }

        [Test]
        public void Divisors()
        {
            var divisors = Divisor.Divisors(28);
            Assert.That(divisors, Is.EquivalentTo(new[] { 1, 2, 4, 7, 14, 28 }));
        }

        [Test]
        public void ProperDivisors()
        {
            var divisors = Divisor.ProperDivisors(28);
            Assert.That(divisors, Is.EquivalentTo(new[] { 1, 2, 4, 7, 14 }));
        }

        [Test]
        public void DivisorConsistency()
        {
            for (var i = 1; i < 1000; i++)
            {
                var count = Divisor.Divisors(i).Count();
                Assert.That(Divisor.CountDivisors(i), Is.EqualTo(count));
            }
        }

        [Test]
        public void Perfection()
        {
            Assert.That(Divisor.Perfection(7), Is.EqualTo(-1));
            Assert.That(Divisor.Perfection(28), Is.EqualTo(0));
            Assert.That(Divisor.Perfection(12), Is.EqualTo(1));
        }
    }
}
