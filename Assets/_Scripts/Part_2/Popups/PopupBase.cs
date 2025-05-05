using UnityEngine;


namespace Part_2
{
    public class PopupBase : MonoBehaviour, IPopup
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(true);
        }
    }
}