// ******------------------------------------------------------******
// DeleteProductOnSlot.cs
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
//
// ******------------------------------------------------------******

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PW
{
    public class DeleteProductOnSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] public int SlotIndex;

        private Button deleteButton;

        private bool shownDeleteButton = true;

        private void Start()
        {
            deleteButton = GetComponentInChildren<Button>();
            //here we're adding onClick event to buttons from script.

            deleteButton.onClick.AddListener(delegate
            {
                //This will not be called unless user clicked on the button.
                DeletePlayerSlotImage();
            });

            ToggleDeleteMode();
        }

        //This is for detecting click events on UI objects
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) ToggleDeleteMode();
        }


        private void ToggleDeleteMode()
        {
            shownDeleteButton = !shownDeleteButton;
            ChangeUI();
        }

        private void ChangeUI()
        {
            deleteButton.gameObject.SetActive(shownDeleteButton);
        }

        public void DeletePlayerSlotImage()
        {
            ToggleDeleteMode();
            GetComponent<Image>().sprite = null;
            BasicGameEvents.RaiseOnProductDeletedFromSlot(SlotIndex);
        }
    }
}