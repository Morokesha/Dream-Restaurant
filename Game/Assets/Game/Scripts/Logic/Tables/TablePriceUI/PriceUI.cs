using TMPro;
using UnityEngine;

namespace Game.Scripts.Logic.Tables.TablePriceUI
{
    public class PriceUI : MonoBehaviour
    {
        [SerializeField] private SpawnerTables _spawnerTables;
        [SerializeField] private TextMeshProUGUI _priceText;

        private void Start()
        {
            SetDefaultPrice();
        }

        private void OnEnable()
        {
            _spawnerTables.ChangedCounter += OnChangedCounter;
            _spawnerTables.PlayerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            _spawnerTables.ChangedCounter -= OnChangedCounter;
            _spawnerTables.PlayerExited -= OnPlayerExited;
        }

        private void OnPlayerExited()
        {
            SetDefaultPrice();
        }

        private void OnChangedCounter(int count)
        {
            _priceText.SetText(count.ToString());
        }

        private void SetDefaultPrice()
        {
            _priceText.SetText(_spawnerTables.PriceForSpawnTable.ToString());
        }
    }
}