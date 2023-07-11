using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Logic.Visitors
{
    public class VisitorInteractionUI : MonoBehaviour
    {
        [SerializeField] private Image _orderImage;
        
        public void ActivateDisplay(Sprite sprite)
        {
            _orderImage.sprite = sprite;
            gameObject.SetActive(true);
        }

        public void DisableDisplay()
        {
            gameObject.SetActive(false);
        }
    }
}