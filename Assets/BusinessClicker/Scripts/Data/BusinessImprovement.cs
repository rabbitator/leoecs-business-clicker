using System;
using UnityEngine;

namespace BusinessClicker.Data
{
    [Serializable]
    public class BusinessImprovement
    {
        [SerializeField]
        private string _name = "Improvement Name";
        [SerializeField, Min(0.0f)]
        private float _price;
        [SerializeField, Min(0.0f)]
        private float _multiplierPercent = 1.0f;

        public string Name => _name;
        public float Price => _price;
        public float MultiplierPercent => _multiplierPercent;
    }
}