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
        /// <param name="pol1">
        /// 1st polynomial for comparing.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial for comparing.
        /// </param>
        /// <returns>
        /// Bool result.
        /// </returns>
        public static bool operator ==(Polynomial pol1, Polynomial pol2)
        {
            if (ReferenceEquals(pol1, null) && ReferenceEquals(pol2, null))
                return true;

            if (ReferenceEquals(pol1, null) || ReferenceEquals(pol2, null))
                return false;

            if (ReferenceEquals(pol1, pol2))
                return true;

            if (pol1.Pow != pol2.Pow)
                return false;

            for (int i = 0; i < pol1.Pow; i++)
            {
                if (Math.Abs(pol1.coeffArray[i] - pol2.coeffArray[i]) > COMPARE_PRECISION)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compare two polynomial.
        /// </summary>
        /// <param name="pol1">
        /// 1st polynomial for comparing.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial for comparing.
        /// </param>
        /// <returns>
        /// Bool result.
        /// </returns>
        public static bool operator !=(Polynomial pol1, Polynomial pol2) => !(pol1 == pol2);

        /// <summary>
        /// Calculate sum of two polynomials.
        /// </summary>
        /// <param name="pol1">
        /// 1st polynomial for calculating.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial for calculating.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws if at least one of two arguments is null.
        /// </exception>
        /// <returns>
        /// Polynomial-result.
        /// </returns>
        public static Polynomial operator +(Polynomial pol1, Polynomial pol2)
        {
            if (pol1 == null)
                throw new ArgumentNullException(nameof(pol1));

            if (pol2 == null)
                throw new ArgumentNullException(nameof(pol2));

            bool isPowFirstGreater = pol1.Pow > pol2.Pow;
            double[] tempCoeffArray =
                isPowFirstGreater?CreateArrayCopy(pol1.coeffArray):CreateArrayCopy(pol2.coeffArray);

            if (isPowFirstGreater)
            {
                for (int i = 0; i < pol2.coeffArray.Length; i++)
                {
                    tempCoeffArray[tempCoeffArray.Length - 1 - i] += pol2.coeffArray[pol2.Pow - i];
                }

                return new Polynomial(tempCoeffArray);
            }
            
            for (int i = 0; i < pol1.coeffArray.Length; i++)
            {
                tempCoeffArray[tempCoeffArray.Length - 1 - i] += pol1.coeffArray[pol1.Pow - i];
            }

            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// CLR-equivalent of operator+.
        /// Calculate sum of two polynomials.
        /// </summary>
        /// <param name="pol1">
        /// 1st polynomial.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Polynomial-result.
        /// </returns>
        public static Polynomial Add(Polynomial pol1, Polynomial pol2) => pol1 + pol2;

        /// <summary>
        /// Return inverse polynomial.
        /// </summary>
        /// <param name="pol">
        /// Polynomial.
        /// </param>
        /// <returns>
        /// Inverse polynomial.
        /// </returns>
        public static Polynomial operator -(Polynomial pol)
        {
            if (pol == null)
                throw new ArgumentNullException(nameof(pol));

            double[] tempCoeffArray = CreateArrayCopy(pol.coeffArray);
            for (int i = 0; i < pol.coeffArray.Length; i++)
            {
                tempCoeffArray[i] *= -1.0;
            }

            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// Subtraction of two polynomial.
        /// </summary>
        /// <param name="pol1">
        /// 1st polynomial.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of subtraction.
        /// </returns>
        public static Polynomial operator -(Polynomial pol1, Polynomial pol2) => pol1 + (-pol2);

        /// <summary>
        /// CLR-equivalent of operator-.
        /// Subtraction of two polynomial.
        /// </summary>
        /// <param name="pol1">
        /// 1st polynomial.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of subtraction.
        /// </returns>
        public static Polynomial Substract(Polynomial pol1, Polynomial pol2) => pol1 - pol2;

        /// <summary>
        /// Multiplying of two polynomial.
        /// </summary>
        /// <param name="pol1">
        /// 1st polynomial.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial operator *(Polynomial pol1, Polynomial pol2)
        {
            if (pol1 == null)
                throw new ArgumentNullException(nameof(pol1));

            if (pol2 == null)
                throw new ArgumentNullException(nameof(pol2));

            double[] tempCoeffArray = new double[pol1.Pow + pol2.Pow + 1];
            for (int i = 0; i < pol2.coeffArray.Length; i++)
            {
                double[] tempArray = (pol1 * pol2.coeffArray[i]).coeffArray;

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
        /// <param name="pol1">
        /// 1st polynomial.
        /// </param>
        /// <param name="pol2">
        /// 2nd polynomial.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial Multiply(Polynomial pol1, Polynomial pol2) => pol1 * pol2;

        /// <summary>
        /// Multiply polynomial by double value.
        /// </summary>
        /// <param name="pol">
        /// Polynomial.
        /// </param>
        /// <param name="value">
        /// Double value.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial operator *(Polynomial pol, double value)
        {
            if (pol == null)
                throw new ArgumentNullException(nameof(pol));

            double[] tempCoeffArray = CreateArrayCopy(pol.coeffArray);

            for (int i = 0; i < pol.coeffArray.Length; i++)
            {
                tempCoeffArray[i] *= value;
            }

            return new Polynomial(tempCoeffArray);
        }

        /// <summary>
        /// CLR-equivalent of operator*.
        /// Multiply polynomial by double value.
        /// </summary>
        /// <param name="pol">
        /// Polynomial.
        /// </param>
        /// <param name="value">
        /// Double value.
        /// </param>
        /// <returns>
        /// Result of multiplication.
        /// </returns>
        public static Polynomial Multiply(Polynomial pol, double value) => pol * value;

        private static double[] CreateArrayCopy(double[] sourceArray)
        {
            double[] newCopy = new double[sourceArray.Length];
            sourceArray.CopyTo(newCopy, 0);

            return newCopy;
        }
    }
}
