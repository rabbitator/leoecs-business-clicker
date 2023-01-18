using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessClicker.Data.Views
{
    public class BusinessCardView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _name;
        [SerializeField]
        private Slider _progressBar;
        [SerializeField]
        private LevelUpButton _lvlUpButton;
        [SerializeField]
        private TMP_Text _lvlLabel;
        [SerializeField]
        private TMP_Text _incomeLabel;
        [SerializeField]
        private RectTransform _improveButtonsRoot;

        public Slider ProgressBar => _progressBar;
        public LevelUpButton LvlUpButton => _lvlUpButton;
        public RectTransform ImproveButtonsRoot => _improveButtonsRoot;

        public void SetName(string newName)
        {
            _name.text = newName;
        }

        public void SetLevel(int value)
        {
            _lvlLabel.text = $"LVL<br>{value}";
        }

        public void SetIncome(int value)
        {
            _incomeLabel.text = $"Income<br>{value}$";
        }
    }
}