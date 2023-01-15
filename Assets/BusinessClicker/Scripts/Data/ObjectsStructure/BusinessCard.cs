using UnityEngine;

namespace BusinessClicker.Data.ObjectsStructure
{
    public class BusinessCard : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _improveButtonsRoot;

        public RectTransform ImproveButtonsRoot => _improveButtonsRoot;
    }
}