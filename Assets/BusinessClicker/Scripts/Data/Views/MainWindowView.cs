using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessClicker.Data.Views
{
    public class MainWindowView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _balanceText;
        [SerializeField]
        private ScrollRect _scroll;

        public ScrollRect Scroll => _scroll;

        public void SetBalance(int value)
        {
            _balanceText.text = $"Balance: {value}$";
        }
    }
}
