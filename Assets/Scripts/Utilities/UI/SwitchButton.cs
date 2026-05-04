using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JTUtility.UI
{
    [System.Serializable]
    public class SwitchButtonData
    {
        public string Label = string.Empty;
        public Color ButtonColor = Color.white;
        public Color LabelColor = Color.black;
        public bool UseButtonSprite = false;
        public Sprite ButtonSprite = null;
        public StringEvent OnSwitchOn = new StringEvent();
    }

    public class SwitchButton : MonoBehaviour
    {
        [System.Serializable]
        private class IntEvent : UnityEvent<int>
        { }

        [SerializeField]
        private SwitchButtonData[] data = new SwitchButtonData[0];

        [SerializeField]
        private bool skipSameIndexEvent = false;

        [SerializeField]
        private int currentIndex = 0;

        [SerializeField]
        private IntEvent OnSwitch = null;

        [SerializeField]
        private Image ButtonImage = null;

        [SerializeField]
        private Text ButtonLabel = null;

        [SerializeField]
        private TMPro.TMP_Text ButtonTMPLabel = null;

        private bool disabledEvent;

        public int CurrentIndex => currentIndex;
        public int DataLength => data.Length;

        private void Awake()
        {
            disabledEvent = true;
            SetTo(currentIndex);
            disabledEvent = false;
        }

        public void Switch()
        {
            setButton((currentIndex + 1) % data.Length);
        }

        public void SetTo(int index)
        {
            if (skipSameIndexEvent && currentIndex == index)
            {
                return;
            }

            setButton(index);
        }

        public void SetToNoEvent(int index)
        {
            if (skipSameIndexEvent && currentIndex == index)
            {
                return;
            }

            setButton(index, false);
        }

        private void setButton(int index, bool triggerEvent = true)
        {
            if (index < 0 || index >= data.Length)
            {
                Debug.LogError("Index out of range: " + index);
                return;
            }

            triggerEvent = triggerEvent && currentIndex != index;

            currentIndex = index;
            if (ButtonImage != null) ButtonImage.color = data[index].ButtonColor;
            if (ButtonImage != null && data[index].UseButtonSprite) ButtonImage.sprite = data[index].ButtonSprite;
            if (ButtonLabel != null) ButtonLabel.color = data[index].LabelColor;
            if (ButtonLabel != null) ButtonLabel.text = data[index].Label;
            if (ButtonTMPLabel != null) ButtonTMPLabel.color = data[index].LabelColor;
            if (ButtonTMPLabel != null) ButtonTMPLabel.text = data[index].Label;

            if (disabledEvent || !triggerEvent)
                return;

            if (data[index].OnSwitchOn != null)
                data[index].OnSwitchOn.Invoke(data[index].Label);

            OnSwitch.Invoke(index);
        }
    }
}