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
        public TMP_Text LvlLabel => _lvlLabel;
        public TMP_Text IncomeLabel => _incomeLabel;
        public RectTransform ImproveButtonsRoot => _improveButtonsRoot;

        public void SetName(string newName)
        {
            _name.text = newName;
        }
    }
}