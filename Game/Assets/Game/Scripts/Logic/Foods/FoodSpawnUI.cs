using Game.Scripts.Core.StaticDataService;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Logic.Foods
{
    public class FoodSpawnUI : MonoBehaviour
    {
        [SerializeField] private Image _radialScale;
        [SerializeField] private Image _foodImage;

        private void Start()
        {
            _radialScale.fillAmount = 0f;
        }

        public void ActivateDisplay(Sprite sprite ,bool active)
        {
            _foodImage.sprite = sprite;
            gameObject.SetActive(active);
        }
        
        public void DrawSpawnFood(float scale)
        {
            _radialScale.fillAmount = scale;
        }
    }
}