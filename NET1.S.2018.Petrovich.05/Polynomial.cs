using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1.S._2018.Petrovich._05
{
    /// <summary>
    /// Class for working with polynomials.
    /// </summary>
    public class Polynomial
    {
        private const double COMPARE_PRECISION = 1.0E-24d;

        private readonly double[] coeffArray;
        public int Pow { get; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="coeffArray">
        /// Takes coefficients of polynomial.
        /// First coefficient is greatest degree.
        /// </param>
        public Polynomial(params double[] coeffArray)
        {
            if(coeffArray == null)
                throw new ArgumentNullException(nameof(coeffArray));

            this.coeffArray = coeffArray;
            Pow = coeffArray.Length - 1;
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        /// <param name="index">
        /// Index of coefficient. The lowest index correspond to highest degree.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if index out of bounds array.
        /// </exception>
        /// <returns>
        /// Coefficient at the specified degree.
        /// </returns>
        public double this[int index]
        {
            get
            {
                if (index < 0 || index > this.coeffArray.Length - 1)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return this.coeffArray[Pow - index];
            }
        }

        /// <summary>
        /// Return true if objects are equals. 
        /// </summary>
        /// <param name="obj">
        /// Object for comparing.
        /// </param>
        /// <returns>
        /// Bool result.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return this == (Polynomial)obj;
        }

        /// <summary>
        /// Return objects HashCode.
        /// </summary>
        /// <returns>
        /// HashCode value.
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 0;

            for (int i = 0; i < this.coeffArray.Length; i++)
            {
                hash += (int)this[i] * hash + Pow;
            }

            return hash;
        }

        /// <summary>
        /// Return string representation of object.
        /// </summary>
        /// <returns>
        /// String representation.
        /// </returns>
        public override string ToString()
        {
            StringBuilder tempString = new StringBuilder();

            for (int i = 0; i < this.coeffArray.Length; i++)
            {
                tempString.Append($"{this[Pow - i]}*x^{Pow - i} + ");
            }

            return tempString.ToString(0, tempString.Length - 3);
        }

        /// <summary>
        /// Compare two polynomial.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial for comparing.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial for comparing.
        /// </param>
        /// <returns>
        /// Bool result.
        /// </returns>
        public static bool operator ==(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            if (ReferenceEquals(firstPolynomial, null) && ReferenceEquals(secondPolynomial, null))
                return true;

            if (ReferenceEquals(firstPolynomial, null) || ReferenceEquals(secondPolynomial, null))
                return false;

            if (ReferenceEquals(firstPolynomial, secondPolynomial))
                return true;

            if (firstPolynomial.Pow != secondPolynomial.Pow)
                return false;

            for (int i = 0; i < firstPolynomial.Pow; i++)
            {
                if (Math.Abs(firstPolynomial.coeffArray[i] - secondPolynomial.coeffArray[i]) > COMPARE_PRECISION)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compare two polynomial.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial for comparing.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial for comparing.
        /// </param>
        /// <returns>
        /// Bool result.
        /// </returns>
        public static bool operator !=(Polynomial firstPolynomial, Polynomial secondPolynomial) => !(firstPolynomial == secondPolynomial);

        /// <summary>
        /// Calculate sum of two polynomials.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial for calculating.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial for calculating.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws if at least one of two arguments is null.
        /// </exception>
        /// <returns>
        /// Polynomial-result.
        /// </returns>
        public static Polynomial operator +(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            if (firstPolynomial == null)
                throw new ArgumentNullException(nameof(firstPolynomial));

            if (secondPolynomial == null)
                throw new ArgumentNullException(nameof(secondPolynomial));

            bool isPowFirstGreater = firstPolynomial.Pow > secondPolynomial.Pow;
            double[] tempCoeffArray =
                isPowFirstGreater?CreateArrayCopy(firstPolynomial.coeffArray):CreateArrayCopy(secondPolynomial.coeffArray);

            if (isPowFirstGreater)
            {
                for (int i = 0; i < secondPolynomial.coeffArray.Length; i++)
                {
                    tempCoeffArray[tempCoeffArray.Length - 1 - i] += secondPolynomial.coeffArray[secondPolynomial.Pow - i];
                }

                return new Polynomial(tempCoeffArray);
            }
            
            for (int i = 0; i < firstPolynomial.coeffArray.Length; i++)
            {
                tempCoeffArray[tempCoeffArray.Length - 1 - i] += firstPolynomial.coeffArray[firstPolynomial.Pow - i];
            }

            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// CLR-equivalent of operator+.
        /// Calculate sum of two polynomials.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Polynomial-result.
        /// </returns>
        public static Polynomial Add(Polynomial firstPolynomial, Polynomial secondPolynomial) => firstPolynomial + secondPolynomial;

        /// <summary>
        /// Return inverse polynomial.
        /// </summary>
        /// <param name="polynomial">
        /// Polynomial.
        /// </param>
        /// <returns>
        /// Inverse polynomial.
        /// </returns>
        public static Polynomial operator -(Polynomial polynomial)
        {
            if (polynomial == null)
                throw new ArgumentNullException(nameof(polynomial));

            double[] tempCoeffArray = CreateArrayCopy(polynomial.coeffArray);
            for (int i = 0; i < polynomial.coeffArray.Length; i++)
            {
                tempCoeffArray[i] *= -1.0;
            }

            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// Subtraction of two polynomial.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of subtraction.
        /// </returns>
        public static Polynomial operator -(Polynomial firstPolynomial, Polynomial secondPolynomial) => firstPolynomial + (-secondPolynomial);

        /// <summary>
        /// CLR-equivalent of operator-.
        /// Subtraction of two polynomial.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of subtraction.
        /// </returns>
        public static Polynomial Substract(Polynomial firstPolynomial, Polynomial secondPolynomial) => firstPolynomial - secondPolynomial;

        /// <summary>
        /// Multiplying of two polynomial.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial operator *(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            if (firstPolynomial == null)
                throw new ArgumentNullException(nameof(firstPolynomial));

            if (secondPolynomial == null)
                throw new ArgumentNullException(nameof(secondPolynomial));

            double[] tempCoeffArray = new double[firstPolynomial.Pow + secondPolynomial.Pow + 1];
            for (int i = 0; i < secondPolynomial.coeffArray.Length; i++)
            {
                double[] tempArray = (firstPolynomial * secondPolynomial.coeffArray[i]).coeffArray;

                for (int j = 0; j < tempArray.Length; j++)
                {
                    tempCoeffArray[i + j] += tempArray[j];
                }
            }
            
            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// CLR-equivalent of operator*.
        /// Multiplying of two polynomial.
        /// </summary>
        /// <param name="firstPolynomial">
        /// 1st polynomial.
        /// </param>
        /// <param name="secondPolynomial">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial Multiply(Polynomial firstPolynomial, Polynomial secondPolynomial) => firstPolynomial * secondPolynomial;

        /// <summary>
        /// Multiply polynomial by double value.
        /// </summary>
        /// <param name="polynomial">
        /// Polynomial.
        /// </param>
        /// <param name="value">
        /// Double value.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial operator *(Polynomial polynomial, double value)
        {
            if (polynomial == null)
                throw new ArgumentNullException(nameof(polynomial));

            double[] tempCoeffArray = CreateArrayCopy(polynomial.coeffArray);

            for (int i = 0; i < polynomial.coeffArray.Length; i++)
            {
                tempCoeffArray[i] *= value;
            }

            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// CLR-equivalent of operator*.
        /// Multiply polynomial by double value.
        /// </summary>
        /// <param name="polynomial">
        /// Polynomial.
        /// </param>
        /// <param name="value">
        /// Double value.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial Multiply(Polynomial polynomial, double value) => polynomial * value;

        private static double[] CreateArrayCopy(double[] sourceArray)
        {
            double[] newCopy = new double[sourceArray.Length];
            sourceArray.CopyTo(newCopy, 0);

            return newCopy;
        }
    }
}
