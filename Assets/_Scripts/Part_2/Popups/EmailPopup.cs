using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Part_2
{
    public class EmailPopup : PopupBase
    {
        [SerializeField] private TMP_InputField _emailInput;
        [SerializeField] private Button _btnGetCode;

        public Action<string> OnClickGetCode;

        private void Awake()
        {
            _btnGetCode.onClick.AddListener(GetCode);
        }

        private void GetCode()
        {
            if (string.IsNullOrEmpty(_emailInput.text))
                return;

            OnClickGetCode?.Invoke(_emailInput.text);
        }

        public override void Show()
        {
            _emailInput.text = "";

            base.Show();
        }

        private void OnDestroy()
        {
            _btnGetCode.onClick.RemoveAllListeners();
        }
    }
}