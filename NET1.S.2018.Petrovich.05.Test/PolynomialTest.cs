using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NET1.S._2018.Petrovich._05.Test
{
    [TestFixture]
    public class PolynomialTest
    {
        [TestCase(new[] { 4d, 0, 0, 2, 3, 0, 1}, new[] {3d, 4, 0, 0, 1}, new[] {12d, 16, 0, 6, 21, 12, 3, 6, 3, 0, 1})]
        [TestCase(new[] { 1d, 0, 0, 0, 0, 0, 1 }, new[] { 2d, 0, 0, 1 }, new[] { 2d, 0, 0, 1, 0, 0, 2, 0, 0, 1 })]
        [TestCase(new[] { 3d, 0, 1 }, new[] { 3d }, new[] { 9d, 0, 3 })]
        public void Polynomial_MultiplyTest(double[] firstCoeff, double[] secondCoeff, double[] multily)
        {
            var firstPolynomial = new Polynomial(firstCoeff);
            var secondPolynomial = new Polynomial(secondCoeff);
          
            var expected = new Polynomial(multily);

            var actual = firstPolynomial * secondPolynomial;

            Assert.IsTrue(expected == actual);
        }

        [TestCase(new[] { 4d, 0, 0, 2, 3, 0, 1 }, new[] { 3d, 4, 0, 0, 1 }, new[] { 4d, 0, 3, 6, 3, 0, 2})]
        [TestCase(new[] { 1d, 0, 0, 0, 0, 0, 1 }, new[] { 2d, 0, 0, 1 }, new[] { 1d, 0, 0, 2, 0, 0, 2 })]
        [TestCase(new[] { 3d, 0, 1 }, new[] { 3d }, new[] { 3d, 0, 4 })]
        public void Polynomial_AddTest(double[] firstCoeff, double[] secondCoeff, double[] sum)
        {
            var firstPolynomial = new Polynomial(firstCoeff);
            var secondPolynomial = new Polynomial(secondCoeff);

            var expected = new Polynomial(sum);

            var actual = firstPolynomial + secondPolynomial;

            Assert.IsTrue(expected == actual);
        }

        [TestCase(new[] { 4d, 0, 0, 2, 3, 0, 1 }, new[] { 3d, 4, 0, 0, 1 }, new[] { 4d, 0, -3, -2, 3, 0, 0 })]
        [TestCase(new[] { 1d, 0, 0, 0, 0, 0, 1 }, new[] { 2d, 0, 0, 1 }, new[] { 1d, 0, 0, -2, 0, 0, 0 })]
        [TestCase(new[] { 3d, 0, 1 }, new[] { 3d, 5, 7, 0 ,8 }, new[] { -3d, -5, -4, 0, -7 })]
        public void Polynomial_SubtractionTest(double[] firstCoeff, double[] secondCoeff, double[] sub)
        {   var firstPolynomial = new Polynomial(firstCoeff);
            var secondPolynomial = new Polynomial(secondCoeff);

            var expected = new Polynomial(sub);

            var actual = firstPolynomial - secondPolynomial;

            Assert.IsTrue(expected == actual);
        }

        [Test]
        public void Polynomial_CallConstructorWithNullRef()
            => Assert.Throws<ArgumentNullException>(() => new Polynomial(null));

        [Test]
        public void Polynomial_IndexerWithInvalidIndex()
        {
            Polynomial polynomial = new Polynomial(1d, 2, 3);

            Assert.Throws<ArgumentOutOfRangeException>(() => polynomial[4].ToString());
        }

        [Test]
        public void Polynomial_ICloneableAndIEquatableTest()
        {
            Polynomial polynomial = new Polynomial(1d, 2, 3);
            Polynomial polynomialClone = polynomial.Clone();

            Assert.IsTrue(polynomial.Equals(polynomialClone));
        }      
    }
}
