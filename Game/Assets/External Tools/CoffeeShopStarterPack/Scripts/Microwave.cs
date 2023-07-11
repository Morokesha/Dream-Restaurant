// ******------------------------------------------------------******
// Microwave.cs
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
//
// ******------------------------------------------------------******

using System.Collections;
using UnityEngine;

namespace PW
{
    public class Microwave : MonoBehaviour
    {
        public Transform door;
        public Transform beginEnteringSpot;
        public Transform cookingSpot;

        public GameObject progressHelperprefab;
        private HeatableProduct currentProduct; // The product we currently heating inside
        private bool doorIsOpen;

        private float heatingProcess; //how much time 

        private ProgressHelper m_progressHelper;

        public bool isEmpty => !doorIsOpen && currentProduct == null;

        private void Start()
        {
            //Instantiate and set the UI indicator
            if (progressHelperprefab != null)
            {
                m_progressHelper = Instantiate(progressHelperprefab, transform).GetComponent<ProgressHelper>();
                //dont show the indicator now
                m_progressHelper.ToggleHelper(false);
            }
        }

        private void OnMouseDown()
        {
            if (!doorIsOpen && !isEmpty)
            {
                var PlayerSlots = FindObjectOfType<PlayerSlots>();
                if (PlayerSlots.CanHoldItem(currentProduct.orderID))
                {
                    BasicGameEvents.RaiseOnProductAddedToSlot(currentProduct.orderID);
                    StartCoroutine(currentProduct.AnimateGoingToSlot());
                    currentProduct = null;
                    StartCoroutine(PlayDoorAnim(true, true));
                }
                else
                {
                }
            }
            else if (isEmpty)
            {
                StartCoroutine(PlayDoorAnim(true, true));
            }
        }

        public void SetProduct(HeatableProduct product, float heatingAmount)
        {
            if (doorIsOpen || currentProduct != null)
                return;
            currentProduct = product;
            heatingProcess = heatingAmount;
            StartCoroutine(OpenMicrowaveAndHeatProduct());
        }

        private IEnumerator OpenMicrowaveAndHeatProduct()
        {
            yield return StartCoroutine(PlayDoorAnim(true, true));
            yield return StartCoroutine(Heating());
        }


        private IEnumerator PlayDoorAnim(bool open, bool alsoReverse = false)
        {
            doorIsOpen = open;
            var totalTime = 1f;
            var curTime = totalTime;
            float totalAngle = 90;
            var multiplier = 1f;
            float finalAngle = 90;
            if (!open)
            {
                finalAngle = 0;
                multiplier = -1f;
            }

            while (curTime > 0)
            {
                var amount = Time.deltaTime;

                door.transform.Rotate(new Vector3(0f, multiplier * totalAngle * amount / totalTime, 0f), Space.Self);
                curTime -= Time.deltaTime;
                yield return null;
            }

            door.transform.localRotation = Quaternion.Euler(new Vector3(0f, finalAngle, 0f));
            doorIsOpen = false;

            yield return new WaitForSeconds(.2f);
            if (alsoReverse) yield return StartCoroutine(PlayDoorAnim(!open));
        }

        private IEnumerator Heating()
        {
            m_progressHelper.ToggleHelper(true);

            var curProcess = heatingProcess;

            while (curProcess > 0)
            {
                curProcess -= Time.deltaTime;
                m_progressHelper.UpdateProcessUI(curProcess, heatingProcess);
                yield return null;
            }

            m_progressHelper.ToggleHelper(false);
        }
    }
}