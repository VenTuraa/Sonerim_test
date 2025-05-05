using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Part_2
{
    public class InsertCodePopup : PopupBase
    {
        [SerializeField] private TMP_InputField _codeInput;
        [SerializeField] private Button _loginButton;

        public Action<string> OnLoginButtonClicked;

        private void Awake()
        {
            _loginButton.onClick.AddListener(LoginPressed);
        }

        private void LoginPressed()
        {
            if (string.IsNullOrEmpty(_codeInput.text))
                return;

            OnLoginButtonClicked?.Invoke(_codeInput.text);
        }

        public override void Show()
        {
            base.Show();
            _codeInput.text = "";
        }
    }
}