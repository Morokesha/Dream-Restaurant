// ******------------------------------------------------------******
// OnClickCoverHelper.cs
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
//
// ******------------------------------------------------------******

using UnityEngine;
using UnityEngine.Events;

namespace PW
{
    [RequireComponent(typeof(Collider))]
    public class OnClickCoverHelper : MonoBehaviour
    {
        [SerializeField] public UnityEvent methodToCall;

        private Collider m_collider;

        private void OnEnable()
        {
            m_collider = GetComponent<Collider>();
            m_collider.enabled = false;
        }

        private void OnMouseDown()
        {
            if (methodToCall != null)
            {
                methodToCall.Invoke();
                m_collider.enabled = false;
            }
        }

        public void ActivateCollider()
        {
            m_collider.enabled = true;
        }
    }
}