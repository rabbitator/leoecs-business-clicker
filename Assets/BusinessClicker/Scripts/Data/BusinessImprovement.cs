using System;
using UnityEngine;

namespace BusinessClicker.Data
{
    [Serializable]
    public class BusinessImprovement
    {
        [SerializeField, Min(0.0f)]
        private float _price;
        [SerializeField, Min(0.0f)]
        private float _multiplierPercent = 1.0f;
    }
}