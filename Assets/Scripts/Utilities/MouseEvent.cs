using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JTUtility
{
    public class MouseEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] bool raisingEvents = true;
        [SerializeField] private UnityEvent MouseUp;
        [SerializeField] private UnityEvent MouseDown;
        [SerializeField] private UnityEvent MouseEnter;
        [SerializeField] private UnityEvent MouseClick;
        [SerializeField] private UnityEvent MouseOver;
        [SerializeField] private UnityEvent MouseExit;

        public bool RaisingEvents
        {
            get => raisingEvents;
            set => raisingEvents = value;
        }

        public MouseEvent()
        {
            MouseUp = new UnityEvent();
            MouseDown = new UnityEvent();
            MouseEnter = new UnityEvent();
            MouseOver = new UnityEvent();
            MouseExit = new UnityEvent();
        }

        private void OnMouseUp()
        {
            if (MouseUp != null && raisingEvents)
                MouseUp.Invoke();
        }

        private void OnMouseDown()
        {
            if (MouseDown != null && raisingEvents)
                MouseDown.Invoke();
        }

        private void OnMouseEnter()
        {
            if (MouseEnter != null && raisingEvents)
                MouseEnter.Invoke();
        }

        private void OnMouseOver()
        {
            if (MouseOver != null && raisingEvents)
                MouseOver.Invoke();
        }

        private void OnMouseExit()
        {
            if (MouseExit != null && raisingEvents)
                MouseExit.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (MouseUp != null && raisingEvents)
                MouseUp.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (MouseDown != null && raisingEvents)
                MouseDown.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MouseEnter != null && raisingEvents)
                MouseEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (MouseExit != null && raisingEvents)
                MouseExit.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (MouseClick != null && raisingEvents)
                MouseClick.Invoke();
        }
    }
}