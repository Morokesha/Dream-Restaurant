// ******------------------------------------------------------******
// StoveGameObject.cs
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
    public class StoveGameObject : CookingGameObject
    {
        public Transform doorTransform;

        private bool doorIsOpen;
        private bool isAnimating;

        private readonly Vector3 progressHelperOffset = new Vector3(0f, 0.8f, 0f);

        public override void OnMouseDown()
        {
            base.OnMouseDown();
        }

        public override void DoDoorAnimationsIfNeeded()
        {
            base.DoDoorAnimationsIfNeeded();
            if (!isAnimating)
                StartCoroutine(PlayDoorAnim(true, true));
        }

        public override void StartCooking(CookableProduct product)
        {
            base.StartCooking(product);
            m_progressHelper.transform.position += progressHelperOffset;
        }

        private IEnumerator PlayDoorAnim(bool open, bool alsoReverse = false)
        {
            doorIsOpen = open;
            isAnimating = true;
            var totalTime = doorAnimTime;
            var curTime = totalTime;
            float totalAngle = 66;
            var multiplier = 1f;
            float finalAngle = 66;
            if (!open)
            {
                finalAngle = 0;
                multiplier = -1f;
            }

            while (curTime > 0)
            {
                var amount = Time.deltaTime;
                var eulerTemp = doorTransform.rotation.eulerAngles;

                doorTransform.Rotate(new Vector3(multiplier * totalAngle * amount / totalTime, 0f, 0f), Space.Self);
                curTime -= Time.deltaTime;
                yield return null;
            }

            doorTransform.localRotation = Quaternion.Euler(new Vector3(finalAngle, 0f, 0f));
            doorIsOpen = false;

            yield return new WaitForSeconds(.2f);
            if (alsoReverse)
            {
                yield return StartCoroutine(PlayDoorAnim(!open));
                isAnimating = false;
            }
            else
            {
                isAnimating = false;
            }
        }
    }
}