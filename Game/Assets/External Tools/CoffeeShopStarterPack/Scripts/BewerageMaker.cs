// 
// BewerageMaker.cs
//
// Used on interactable products like espresso machine, mocha pot and tea pot.
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
// ******------------------------------------------------------******

using System.Collections;
using UnityEngine;

namespace PW
{
    [RequireComponent(typeof(Collider))]
    public class BewerageMaker : MonoBehaviour
    {
        //Drink prefab created from the product manager
        public GameObject cupType;

        //UI indicator to show progress
        public GameObject progressHelperprefab;

        //the position where the cup will be placed on instantiate
        public Transform fillCupSpot;

        private void Start()
        {
            //find total time to process
            totalProcess = preFillProcess + fillingProcess;

            //Instantiate and set the UI indicator
            if (progressHelperprefab != null)
            {
                m_progressHelper = Instantiate(progressHelperprefab, transform).GetComponent<ProgressHelper>();

                m_progressHelper.ToggleHelper(false);
            }

            //get the collider and enable it 
            m_Collider = GetComponent<Collider>();

            m_Collider.enabled = true;

            //Get the animator from the dummy Target
            if (dummyAnimationTarget != null)
                m_animator = dummyAnimationTarget.GetComponent<Animator>();

            //if you want to use tweening we need to disable the animator.
            //It would override the transforms if we didn't
            if (useTweeningAnimation)
                if (m_animator != null)
                    m_animator.enabled = false;
        }

        private void OnMouseUp()
        {
            if (canFillCup) StartFillingStep();
        }

        private void StartFillingStep()
        {
            canFillCup = false;

            //Instantiate and set the cup 
            SetTheCup();

            //Show the indicator
            if (m_progressHelper != null)
                m_progressHelper.ToggleHelper(true);

            //Start playing the animations or tweening
            if (!string.IsNullOrEmpty(preFillAnimationStateName) || useTweeningAnimation)
                StartPreFillAnimationState();
            else
                StartCoroutine(DoFillAnimation());
        }

        private void StartPreFillAnimationState()
        {
            if (!useTweeningAnimation)
                if (m_animator != null)
                    m_animator.SetTrigger(preFillAnimationStateName);
            StartCoroutine(DoPreFill(dummyAnimationTarget, finalTweenTarget));
        }

        private IEnumerator DoPreFill(Transform target, Transform finalTweenValue)
        {
            //We do the tweening and update UI in this coroutine.

            var curPreFill = preFillProcess;
            var totalDist = Vector3.zero;
            var totalRot = Vector3.zero;
            Vector3 FinalPosition;
            Vector3 FinalRotation;

            if (finalTweenTarget != null && target != null)
            {
                FinalPosition = finalTweenValue.position;
                FinalRotation = finalTweenValue.rotation.eulerAngles;
                totalDist = FinalPosition - target.transform.position;
                totalRot = FinalRotation - target.transform.rotation.eulerAngles;
            }

            while (curPreFill > 0)
            {
                if (useTweeningAnimation)
                {
                    target.transform.position += Time.deltaTime * totalDist / preFillProcess;
                    target.transform.rotation = Quaternion.Euler(target.transform.rotation.eulerAngles +
                                                                 Time.deltaTime * totalRot / preFillProcess);
                }

                curPreFill -= Time.deltaTime;

                var now = preFillProcess - curPreFill;
                m_progressHelper.UpdateProcessUI(now, totalProcess);

                yield return null;
            }

            //Starts Fill ended animation and the particle
            StartCoroutine(DoFillAnimation());
        }

        private IEnumerator DoFillAnimation()
        {
            //If we don't have animation to play just fill the cup
            if (!useTweeningAnimation && (string.IsNullOrEmpty(fillEndedAnimationState) || fillingProcess < 0.001f))
            {
                fillCupHelper.DoFill(0f);
            }
            else
            {
                fillCupHelper.DoFill(fillingProcess);

                if (fillParticle != null)
                    fillParticle.Play();
                var fillCurrent = fillingProcess;

                while (fillCurrent > 0)
                {
                    fillCurrent -= Time.deltaTime;

                    var valNow = preFillProcess + fillingProcess - fillCurrent;

                    if (m_progressHelper != null)
                        m_progressHelper.UpdateProcessUI(valNow, totalProcess);

                    yield return null;
                }
            }

            OnFillEnded();
        }


        private void OnFillEnded()
        {
            //hide the UI indicator
            if (m_progressHelper != null)
                m_progressHelper.ToggleHelper(false);

            //disable collider, because we don't want interaction on the object
            m_Collider.enabled = false;

            //tell the cup to enable its collider and set necessary things 
            fillCupHelper.FillEnded();

            if (fillParticle != null) fillParticle.Stop();

            StartCoroutine(DoFillEnded());
        }


        private void SetTheCup()
        {
            var cup = Instantiate(cupType, fillCupSpot);

            fillCupHelper = cup.GetComponent<FillCupHelper>();

            fillCupHelper.SetMachine(this);

            if (m_progressHelper != null)
                m_progressHelper.ToggleHelper(true);
        }

        public void UnSetTheCup()
        {
            canFillCup = true;

            fillCupHelper = null;

            m_Collider.enabled = true;
        }

        /// <summary>
        ///     Required things to do after fill ended such as reverse movement of a teapot,
        ///     or playing an animation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoFillEnded()
        {
            if (m_animator != null && !string.IsNullOrEmpty(fillEndedAnimationState))
                m_animator.SetTrigger(fillEndedAnimationState);
            var totalDist = Vector3.zero;
            var totalRot = Vector3.zero;
            Vector3 FinalPosition;
            Vector3 FinalRotation;
            if (useTweeningAnimation)
            {
                //make the reverse movement as we did in the prefill

                var reverseMove = preFillProcess;
                if (dummyAnimationTarget != null && finalTweenTarget != null)
                {
                    FinalPosition = transform.position;
                    FinalRotation = transform.rotation.eulerAngles;
                    totalDist = FinalPosition - dummyAnimationTarget.transform.position;
                    totalRot = FinalRotation - dummyAnimationTarget.transform.rotation.eulerAngles;
                }

                while (reverseMove > 0)
                {
                    if (useTweeningAnimation)
                    {
                        dummyAnimationTarget.transform.position += Time.deltaTime * totalDist / preFillProcess;
                        dummyAnimationTarget.transform.rotation = Quaternion.Euler(
                            dummyAnimationTarget.transform.rotation.eulerAngles +
                            Time.deltaTime * totalRot / preFillProcess);
                    }

                    reverseMove -= Time.deltaTime;
                    yield return null;
                }

                if (dummyAnimationTarget != null)
                {
                    dummyAnimationTarget.localPosition = Vector3.zero;
                    dummyAnimationTarget.transform.localRotation = Quaternion.identity;
                }
            }

            yield return null;
        }

        #region AnimationSettings

        [SerializeField] public bool useAnimation = true;

        [SerializeField] public bool useTweeningAnimation;

        //Animation state for prefill.
        public string preFillAnimationStateName;

        //time takes to prefill.
        public float preFillProcess;

        //Animation ending state for fillEnded.
        public string fillEndedAnimationState;

        //time takes to fill the cup with particle.
        public float fillingProcess;

        [SerializeField] public Transform dummyAnimationTarget;

        //optional particle system to show drink getting filled.
        [SerializeField] public ParticleSystem fillParticle;

        //Used with tweening animation, to figure out the end position and rotation of the object.
        //You can duplicate the object, set it's transforms and delete the components like Mesh filter and renderer.
        [SerializeField] public Transform finalTweenTarget;

        #endregion


        #region private variables

        private float totalProcess;

        private bool canFillCup = true;

        private FillCupHelper fillCupHelper;

        private ProgressHelper m_progressHelper;

        private Collider m_Collider;

        private Animator m_animator;

        #endregion
    }
}