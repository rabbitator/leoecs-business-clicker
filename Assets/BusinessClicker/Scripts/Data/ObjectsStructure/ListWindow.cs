using UnityEngine;
using UnityEngine.UI;

namespace BusinessClicker.Data.ObjectsStructure
{
    public class ListWindow : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect _scroll;

        public ScrollRect Scroll => _scroll;
    }
}
