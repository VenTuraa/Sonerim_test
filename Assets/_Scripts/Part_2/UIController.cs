using System;
using System.Threading;
using UnityEngine;

namespace Part_2
{
    public enum ViewSate
    {
        None,
        GetCode,
        InsertCode
    }

    public class UIController : MonoBehaviour
    {
        [SerializeField] private EmailPopup _emailPopup;
        [SerializeField] private InsertCodePopup _insertCodePopup;
        [SerializeField] private SharePointDataLoader _sharePointDataLoader;
        [SerializeField] private GraphRenderer _graphRenderer;

        private NetworkRequestController _networkRequestController;
        private readonly CancellationTokenSource _sourceToken = new();
        private PopupBase _currentPopup;
        private string _cachedEmail;
        private string _azurerToken;

        private void Awake()
        {
            SetState(ViewSate.GetCode);
            _networkRequestController = new NetworkRequestController();
            _emailPopup.OnClickGetCode += OnClickGetCode;
            _insertCodePopup.OnLoginButtonClicked += OnLoginButtonClicked;
        }

        private async void OnLoginButtonClicked(string code)
        {
            var response = await _networkRequestController.InsertCodeAsync(_cachedEmail, code, _sourceToken.Token);
            if (!string.IsNullOrEmpty(response))
            {
                _azurerToken = await _networkRequestController.GetAzureTokenAsync(response, _sourceToken.Token);
                var data = await _sharePointDataLoader.LoadAndPrepareSensorData(_azurerToken, _sourceToken.Token);
                
                _graphRenderer.DrawGraph(data);
            }
        }

        private async void OnClickGetCode(string email)
        {
            var response = await _networkRequestController.GetBridgerEmailCodeAsync(email, _sourceToken.Token);
            if (response)
            {
                SetState(ViewSate.InsertCode);
                _cachedEmail = email;
            }
        }

        private void SetState(ViewSate state)
        {
            _currentPopup?.Hide();
            switch (state)
            {
                case ViewSate.None:
                    break;
                case ViewSate.GetCode:
                    _emailPopup.Show();
                    break;
                case ViewSate.InsertCode:
                    _insertCodePopup.Show();
                    break;
            }
        }

        private void OnDestroy()
        {
            _sourceToken?.Cancel();
            _sourceToken?.Dispose();
        }
    }
}