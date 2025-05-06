using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Part_1
{
    public class InfoPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtId;
        [SerializeField] private TMP_Text _txtTitle;
        [SerializeField] private TMP_Text _txtDescription;
        [SerializeField] private Button _btnClose;

        private void Awake()
        {
            _btnClose.onClick.AddListener(HidePopup);
            gameObject.SetActive(false);
        }

        public void ShowPopup(string id, string title, string description)
        {
            _txtId.text = $"ID: {id}";
            _txtTitle.text = $"Title: {title}";
            _txtDescription.text = $"Description: {description}";
            gameObject.SetActive(true);
        }

        public void HidePopup()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _btnClose.onClick.RemoveAllListeners();
        }
    }
}