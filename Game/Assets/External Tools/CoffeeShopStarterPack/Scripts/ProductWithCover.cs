// ******------------------------------------------------------******
// ProductWithCover.cs
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
    public class ProductWithCover : MonoBehaviour
    {
        private const float k_AutoCloseCoverTime = 0.5f;

        public Transform coverObject;

        public Vector3 openCoverOffset;

        public bool autoCloseCover;

        private bool IsAnimating;

        private Collider m_collider;

        private void OnEnable()
        {
            m_collider = GetComponent<Collider>();
        }


        private void OnMouseDown()
        {
            if (IsAnimating)
                return;

            //Open the cover
            StartCoroutine(OpenCloseDisplay(true, autoCloseCover));
        }


        public void HandleCoverCloseClick()
        {
            if (IsAnimating)
                return;
            StartCoroutine(OpenCloseDisplay(false));
        }

        private IEnumerator OpenCloseDisplay(bool open, bool alsoReverse = false)
        {
            IsAnimating = true;
            var totalTime = 1f;
            var curTime = totalTime;
            var totalDist = openCoverOffset;
            var finalPos = coverObject.position + openCoverOffset;

            if (!open)
            {
                totalDist = -openCoverOffset;
                finalPos = coverObject.position - openCoverOffset;
            }

            while (curTime > 0)
            {
                var amount = Time.deltaTime;
                var eulerTemp = coverObject.transform.rotation.eulerAngles;

                coverObject.transform.position += totalDist * amount / totalTime;
                curTime -= Time.deltaTime;
                yield return null;
            }

            m_collider.enabled = !open;

            coverObject.transform.position = finalPos;

            yield return new WaitForSeconds(.2f);

            if (alsoReverse)
            {
                if (autoCloseCover)
                    //If auto closing enable wait for relevant time before closing.
                    yield return new WaitForSeconds(k_AutoCloseCoverTime);
                yield return StartCoroutine(OpenCloseDisplay(!open));
                yield break;
            }

            m_collider.enabled = !open;

            if (open)
                coverObject.GetComponent<OnClickCoverHelper>().ActivateCollider();
            IsAnimating = false;
        }
    }
}