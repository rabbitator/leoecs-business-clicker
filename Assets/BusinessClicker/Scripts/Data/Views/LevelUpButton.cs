using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessClicker.Data.Views
{
    public class LevelUpButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private TMP_Text _improvementName;
        [SerializeField]
        private TMP_Text _price;

        public Button Button => _button;

        public void SetPrice(int value)
        {
            _price.text = $"{value}$";
        }
    }
}