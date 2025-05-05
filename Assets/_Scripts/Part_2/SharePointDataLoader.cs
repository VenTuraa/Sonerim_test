using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System;

namespace Part_2
{
    public class SharePointDataLoader : MonoBehaviour
    {
        private string siteId = "5915e86b-3030-4cfc-b75e-98dbbd9f62f3";
        private string listCategoriesId = "4a1b5f21-e5b3-40e9-8a89-8a9976ce2e07";
        private string listValuesId = "79081308-168e-41e4-96fd-7062ae6c9c9a";

        public async UniTask<List<SensorDataPoint>> LoadAndPrepareSensorData(string azurerToken, CancellationToken token)
        {
            var categoriesJson = await GetListItemsAsync(azurerToken, siteId, listCategoriesId, token);
            var valuesJson = await GetListItemsAsync(azurerToken, siteId, listValuesId, token);

            var categories = ParseItems(categoriesJson);
            var values = ParseItems(valuesJson);

            List<SensorDataPoint> result = new();

            foreach (var value in values)
            {
                var fields = value["fields"];
                if (fields == null) continue;

                string measuredAtStr = fields["MeasuredAt"]?.ToString();
                string valueStr = fields["Value"]?.ToString();

                if (!string.IsNullOrEmpty(measuredAtStr) && float.TryParse(valueStr, out float parsedValue))
                {
                    try
                    {
                        DateTime measuredAt = DateTime.Parse(measuredAtStr);
                        result.Add(new SensorDataPoint(parsedValue, measuredAt));
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Failed to parse date: {measuredAtStr} - {ex.Message}");
                    }
                }
            }

            return result;
        }

        private async UniTask<JArray> GetListItemsAsync(string azurerToken, string siteId, string listId, CancellationToken token)
        {
            string url = $"https://graph.microsoft.com/v1.0/sites/{siteId}/lists/{listId}/items?expand=fields";

            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", $"Bearer {azurerToken}");
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = JObject.Parse(request.downloadHandler.text);
                return (JArray)json["value"];
            }
            else
            {
                Debug.LogError("Failed to load data: " + request.error);
                return new JArray();
            }
        }

        private List<JToken> ParseItems(JArray items)
        {
            List<JToken> result = new();
            foreach (var item in items)
                result.Add(item);
            return result;
        }

        [Serializable]
        public class SensorDataPoint
        {
            public float value;
            public DateTime measuredAt;

            public SensorDataPoint(float val, DateTime time)
            {
                value = val;
                measuredAt = time;
            }
        }
    }
}
