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
    }
}