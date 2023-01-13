using System;
using UnityEngine;

[CreateAssetMenu]
public class Configuration : ScriptableObject
{
    [SerializeField]
    private GameObject _listRootPrefab;
    [SerializeField]
    private GameObject _businessCardPrefab;
    
    [Space]
    [SerializeField]
    private BusinessData[] _businessesData;
}

[Serializable]
internal class BusinessData
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
}

[Serializable]
internal class BusinessImprovement
{
    [SerializeField, Min(0.0f)]
    private float _price;
    [SerializeField, Min(0.0f)]
    private float _multiplierPercent = 1.0f;
}
