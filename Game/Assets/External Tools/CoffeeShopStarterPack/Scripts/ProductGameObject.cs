// ******------------------------------------------------------******
// ProductGameObject.cs
//
// All product type gameplay objects inherits from this base class
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2020 PuzzledWizard
//
// ******------------------------------------------------------******

using System.Collections;
using UnityEngine;

namespace PW
{
    public class ProductGameObject : MonoBehaviour
    {
        private const float totalTimeGoingToSlot = .4f;

        //Some products can be visually different while they served.
        //This is not available for ReadyToServe objects.
        public GameObject serveAsDifferentGameObject;

        //Product orderID
        public int orderID;

        //This is inherited on ReadyToServe, CookableProduct and HeatableProduct
        //DrinakbleProduct dont use that buy you can use it with a prefab, different than plate,
        //such as a takeaway package.
        public bool AddToPlateBeforeServed;

        //This is usually Vector.zero but someObjects may require that,
        //in our case unfortunaly cookies required that;
        public Vector3 plateOffset;

        public bool RegenerateProduct;

        public virtual IEnumerator AnimateGoingToSlot()
        {
            if (serveAsDifferentGameObject != null)
            {
                for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);

                var go = Instantiate(serveAsDifferentGameObject, transform);
                go.transform.SetAsFirstSibling();
            }

            var curTime = totalTimeGoingToSlot;

            //slot is in the lower center of the camera.
            //which means 0.5f on x axis and 0f on Y axis of viewport
            //we find the world position according to the camera viewport
            //We convert the Vector2 to Vector3 by adding camera pos in Z
            var viewPositionOfSlots = new Vector3(0.5f, 0f, Camera.main.nearClipPlane + 2f);

            //If you want your objects to go to somewhere else in screen or world,
            //change centerPos to another Vector3;
            var centerPos = Camera.main.ViewportToWorldPoint(viewPositionOfSlots);
            var totalDist = centerPos - transform.position;

            while (curTime > 0)
            {
                var timePassed = Time.deltaTime;
                transform.position += timePassed * totalDist / totalTimeGoingToSlot;
                curTime -= timePassed;
                yield return null;
            }

            yield return null;
        }

        public virtual bool CanGoPlayerSlot()
        {
            var PlayerSlots = FindObjectOfType<PlayerSlots>();
            if (PlayerSlots.CanHoldItem(orderID))
            {
                BasicGameEvents.RaiseOnProductAddedToSlot(orderID);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     This is used on heatable and cookable objects
        ///     For moving the object to microwave or stove
        ///     But giving a targetPos you can use it any place
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public virtual IEnumerator MoveToPlace(Vector3 targetPos)
        {
            var totalTime = 1f;
            var curTime = totalTime;
            var totalDist = targetPos - transform.position;
            while (curTime > 0)
            {
                var timePassed = Time.deltaTime;
                transform.position += timePassed * totalDist / totalTime;
                curTime -= timePassed;
                yield return null;
            }

            transform.position = targetPos;
            yield return null;
        }
    }
}