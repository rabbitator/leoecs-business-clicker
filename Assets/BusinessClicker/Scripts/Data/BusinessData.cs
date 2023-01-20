using System;
using UnityEngine;

namespace BusinessClicker.Data
{
    [Serializable]
    public class BusinessData
    {
        [Header("Main settings")]
        [SerializeField]
        private string _name;
        [SerializeField, Min(0.0f)]
        private double _incomeDelay;
        [SerializeField, Min(0.0f)]
        private double _basePrice;
        [SerializeField, Min(0.0f)]
        private double _baseIncome;

        [Space, Header("Improvements")]
        [SerializeField]
        private BusinessImprovement[] _businessImprovements;

        public string Name => _name;
        public double IncomeDelay => _incomeDelay;
        public double BasePrice => _basePrice;
        public double BaseIncome => _baseIncome;
        public BusinessImprovement[] BusinessImprovements => _businessImprovements;
    }
}