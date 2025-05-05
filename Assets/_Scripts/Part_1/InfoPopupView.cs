using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Part_1
{
    public class InfoPopupView : MonoBehaviour
    {
        [SerializeField] private GameObject goPopup;
        [SerializeField] private TMP_Text txtId;
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private TMP_Text txtDescription;
        [SerializeField] private Button _btnClose;

        private void Awake()
        {
            _btnClose.onClick.AddListener(HidePopup);
            goPopup.SetActive(false);
        }

        public void ShowPopup(string id, string title, string description)
        {
            txtId.text = $"ID: {id}";
            txtTitle.text = $"Title: {title}";
            txtDescription.text = $"Description: {description}";
            goPopup.gameObject.SetActive(true);
        }

        public void HidePopup()
        {
            goPopup.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _btnClose.onClick.RemoveAllListeners();
        }
    }
}