using UnityEngine;
using UnityEngine.Serialization;

namespace BusinessClicker.Data
{
    [CreateAssetMenu]
    public class Configuration : ScriptableObject
    {
        [Header("List Window Settings")]
        [SerializeField]
        private GameObject _mainWindowPrefab;
        
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

        public GameObject MainWindowPrefab => _mainWindowPrefab;
        public GameObject BusinessCardPrefab => _businessCardPrefab;
        public GameObject ImproveButtonPrefab => _improveButtonPrefab;
        public BusinessData[] BusinessesData => _businessesData;
    }
}