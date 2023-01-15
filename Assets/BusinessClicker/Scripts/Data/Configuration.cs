using UnityEngine;

namespace BusinessClicker.Data
{
    [CreateAssetMenu]
    public class Configuration : ScriptableObject
    {
        [Header("List Window Settings")]
        [SerializeField]
        private GameObject _listWindowPrefab;
        
        [Space]
        [Header("Card Settings")]
        [SerializeField]
        private GameObject _businessCardPrefab;
        [SerializeField]
        private GameObject _improveButtonPrefab;
    
        [Space]
        [Header("Business data Settings")]
        [SerializeField]
        private BusinessData[] _businessesData;

        public GameObject ListWindowPrefab => _listWindowPrefab;
        public GameObject BusinessCardPrefab => _businessCardPrefab;
        public GameObject ImproveButtonPrefab => _improveButtonPrefab;
        public BusinessData[] BusinessesData => _businessesData;
    }
}