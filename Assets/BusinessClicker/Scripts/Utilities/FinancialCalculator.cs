using System;
using System.Linq;
using BusinessClicker.Data;

namespace BusinessClicker.Utilities
{
    public static class FinancialCalculator
    {
        /// <summary>
        /// Calculation of money count needed for purchasing business
        /// </summary>
        /// <param name="level"></param>
        /// <param name="basePrice"></param>
        /// <returns></returns>
        public static double GetPriceForBusinessLevel(int level, double basePrice)
        {
            return (level + 1) * basePrice;
        }

        /// <summary>
        /// Calculation of business income with improvements
        /// </summary>
        /// <param name="level"></param>
        /// <param name="baseIncome"></param>
        /// <param name="improveValues">Percent values of improvements</param>
        /// <returns></returns>
        public static double GetBusinessIncome(int level, double baseIncome, double[] improveValues)
        {
            var improvementsFactor = improveValues.Length == 0 ? 1.0 : improveValues.Select(PercentToMultiplier).Aggregate((a, b) => a * b);
            return level * baseIncome * improvementsFactor;
        }

        /// <summary>
        /// Used to get array of percent values of improvements considering
        /// purchase state (mask element true means improvement purchased)
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double[] GetMaskedImprovements(bool[] mask, BusinessImprovement[] data)
        {
            if (mask.Length != data.Length) throw new Exception("Different arrays length!");

            var values = new double[mask.Length];

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = mask[i] ? data[i].MultiplierPercent : 0.0f;
            }

            return values;
        }

        private static double PercentToMultiplier(double value)
        {
            return value * 0.01 + 1.0;
        }
    }
}