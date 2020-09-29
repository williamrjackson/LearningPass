using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MOS
{
    /// <summary>
    /// Executes actions if the GameObject it's attached to is swiped. Assign unique actions for each (of four) swipe direction. Specify timeout, angle allowance, etc.
    /// </summary>
    public class SwipeControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool allowVertical = true;
        [SerializeField]
        private bool allowHorizontal = true;
        [SerializeField]
        private float swipeMaximumDuration = 1f;
        [SerializeField]
        private float maximumAngle = 10f;
        [SerializeField]
        [Range(1f, 75f)]
        private float requiredMaginitude = 25f;
        public UnityEvent OnSwipeUp;
        public UnityEvent OnSwipeDown;
        public UnityEvent OnSwipeLeft;
        public UnityEvent OnSwipeRight;

        private enum SwipeDir { None, Up, Down, Left, Right };
        private SwipeDir swipeDir;
        private float swipeStartTime;
        private Vector2 swipeStartPos;


        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time;
            }       
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Vector2 swipe = (Input.GetTouch(0).position - swipeStartPos);
                if ((Time.time - swipeStartTime) < swipeMaximumDuration && swipe.magnitude > requiredMaginitude)
                {
                    switch (swipeDir)
                    {
                        case SwipeDir.Up:
                            OnSwipeUp.Invoke();
                            break;
                        case SwipeDir.Down:
                            OnSwipeDown.Invoke();
                            break;
                        case SwipeDir.Left:
                            OnSwipeLeft.Invoke();
                            break;
                        case SwipeDir.Right:
                            OnSwipeRight.Invoke();
                            break;
                    }
                }
                // Make sure this swipe is no longer valid for future touch ups
                swipeDir = SwipeDir.None;
                swipeStartTime = Time.time - swipeMaximumDuration;
            } 
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            swipeStartPos = eventData.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Vector2 swipe = (eventData.position - swipeStartPos);

            if (allowVertical)
            {
                if (eventData.position.y < swipeStartPos.y && Mathf.Abs(Mathf.DeltaAngle(Vector2.Angle(Vector2.up, swipe), 180f)) < maximumAngle)
                {
                    swipeDir = SwipeDir.Down;
                }
                else if (Mathf.Abs(Mathf.DeltaAngle(Vector2.Angle(Vector2.up, swipe), 0f)) < maximumAngle)
                {
                    swipeDir = SwipeDir.Up;
                }
            }
            if (allowHorizontal)
            {
                if (eventData.position.x < swipeStartPos.x && Mathf.Abs(Mathf.DeltaAngle(Vector2.Angle(Vector2.up, swipe), 90f)) < maximumAngle)   
                {
                    swipeDir = SwipeDir.Left;
                }
                else if (Mathf.Abs(Mathf.DeltaAngle(Vector2.Angle(Vector2.up, swipe), 90f)) < maximumAngle)
                {
                    swipeDir = SwipeDir.Right;
                }
            }     
        }
    }
}
