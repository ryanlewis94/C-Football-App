using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Utility
{
    public class PoissonEvaluator
    {

        public decimal CummulitiveDistributionFunction(int k, decimal lambda)
        {
            double e = Math.Pow(Math.E, (double)-lambda);
            int i = 0;
            double sum = 0.0;
            while (i <= k)
            {
                double n = Math.Pow((double)lambda, i) / Factorial(i);
                sum += n;
                i++;
            }
            decimal cdf = (decimal)e * (decimal)sum;
            return cdf;
        }

        private int Factorial(int k)
        {
            int count = k;
            int factorial = 1;
            while (count >= 1)
            {
                factorial = factorial * count;
                count--;
            }
            return factorial;
        }
    }
}
