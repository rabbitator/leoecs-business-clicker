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
        private float _incomeDelay;
        [SerializeField, Min(0.0f)]
        private float _basePrice;
        [SerializeField, Min(0.0f)]
        private float _baseIncome;

        [Space, Header("Improvements")]
        [SerializeField]
        private BusinessImprovement[] _businessImprovements;

        public string Name => _name;
        public float IncomeDelay => _incomeDelay;
        public float BasePrice => _basePrice;
        public float BaseIncome => _baseIncome;
        public BusinessImprovement[] BusinessImprovements => _businessImprovements;
    }
}