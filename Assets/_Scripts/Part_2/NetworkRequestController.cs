using System;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;

public class NetworkRequestController
{
    private const string APPLICATION_ID = "817c5034-4cfa-419e-a996-602d7f830b25";
    private const string EMAIL_APPLICATION_NAME = "Virtual Viewing Test";
    private const string BASE_URL = "https://bridge10development.mihub.ai/api/Connectors/";
    private const string GET_BRIDGER_EMAIL_CODE = "GetBridgerEmailCode";
    private const string GET_BRIDGER_TOKEN = "GetBridgerToken";
    private const string GET_AZURER_TOKEN = "GetAzurerToken";

    public async UniTask<bool> GetBridgerEmailCodeAsync(string email, CancellationToken token)
    {
        string url =
            $"{BASE_URL + GET_BRIDGER_EMAIL_CODE}?bridgerEmail={UnityWebRequest.EscapeURL(email)}&emailApplicationName={UnityWebRequest.EscapeURL(EMAIL_APPLICATION_NAME)}";

        using UnityWebRequest request = UnityWebRequest.Get(url);

        try
        {
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                return true;
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
                return false;
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Request was cancelled.");
            return false;
        }
    }

    public async UniTask<string> InsertCodeAsync(string email, string code, CancellationToken token)
    {
        string url =
            $"{BASE_URL + GET_BRIDGER_TOKEN}?bridgerEmail={UnityWebRequest.EscapeURL(email)}&bridgerEmailCode={UnityWebRequest.EscapeURL(code)}";
        using UnityWebRequest request = UnityWebRequest.Get(url);

        try
        {
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Bridger Token Response: " + request.downloadHandler.text);
                return request.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Request was cancelled.");
        }

        return string.Empty;
    }

    public async UniTask<string> GetAzureTokenAsync(string bridgerToken, CancellationToken token)
    {
        string url = $"{BASE_URL + GET_AZURER_TOKEN}?applicationId={UnityWebRequest.EscapeURL(APPLICATION_ID)}";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", $"Bearer {bridgerToken}");

        try
        {
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Azure Token Response: " + request.downloadHandler.text);
                return request.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Request was cancelled.");
        }

        return string.Empty;
    }
}