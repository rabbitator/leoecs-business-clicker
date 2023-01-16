using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessClicker.Data.Views
{
    public class ImprovementButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private TMP_Text _nameText;
        [SerializeField]
        private TMP_Text _featureText;
        [SerializeField]
        private TMP_Text _priceText;

        public Button Button => _button;

        public void SetFeatureName(string featureName)
        {
            _nameText.text = featureName;
        }

        public void SetFeatureValue(int value)
        {
            _featureText.text = $"Income: +{value}%";
        }

        public void SetPrice(int value, bool bought = false)
        {
            _priceText.text = bought ? "Purchased." : $"Price: {value}";
        }
    }
}