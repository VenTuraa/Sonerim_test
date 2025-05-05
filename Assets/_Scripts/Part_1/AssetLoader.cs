using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Part_1
{
    [Serializable]
    public class HotObject
    {
        public string Id;
        public string Title;
        public string Description;
    }

    [Serializable]
    public class MiHubModel
    {
        public string AssetBundleIosUrl;
        public string AssetBundleAndroidUrl;
        public string AssetBundleWindowsUrl;
        public string AssetBundleWebGLUrl;
    }

    [Serializable]
    public class Model
    {
        public string FileName;

        [JsonProperty("MiHub Creator Model 0")]
        public MiHubModel MiHubCreatorModel0;

        public HotObject[] HotObjects;
    }

    [Serializable]
    public class ModelsContainer
    {
        public Model[] Models;
    }

    public class AssetLoader : MonoBehaviour
    {
        private ModelsContainer modelsContainer;
        [SerializeField] private HotObjectHighlighter highlighter;
        [TextArea(5, 20)] public string json;

        public string baseUrl = "https://appdev.virtualviewing.co.uk/developer_test_2025/";

        void Start()
        {
            LoadJSON(json);
        }

        private void LoadJSON(string jsonText)
        {
            modelsContainer = JsonConvert.DeserializeObject<ModelsContainer>(jsonText);
            highlighter.SetModels(modelsContainer);
            StartCoroutine(LoadAssetBundles());
        }

        private IEnumerator LoadAssetBundles()
        {
            foreach (var model in modelsContainer.Models)
            {
                if (model.HotObjects.Length > 0)
                {
                    string assetBundleUrl = null;

#if UNITY_IOS
                assetBundleUrl = model.MiHubCreatorModel0.AssetBundleIosUrl;
#elif UNITY_ANDROID
                assetBundleUrl = model.MiHubCreatorModel0.AssetBundleAndroidUrl;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                    assetBundleUrl = model.MiHubCreatorModel0.AssetBundleWindowsUrl;
#elif UNITY_WEBGL
                assetBundleUrl = model.MiHubCreatorModel0.AssetBundleWebGLUrl;
#else
                Debug.LogWarning("Unknown platform");
#endif

                    if (string.IsNullOrEmpty(assetBundleUrl))
                    {
                        Debug.LogWarning($"Url is empty {model.FileName}");
                        continue;
                    }

                    string fullUrl = baseUrl + assetBundleUrl;

                    using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(fullUrl))
                    {
                        yield return uwr.SendWebRequest();

                        if (uwr.result != UnityWebRequest.Result.Success)
                        {
                            Debug.LogError($"Load error: {uwr.error}");
                        }
                        else
                        {
                            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                            LoadAssetsFromBundle(bundle);
                        }
                    }
                }
            }
        }

        private void LoadAssetsFromBundle(AssetBundle bundle)
        {
            if (bundle == null)
            {
                return;
            }

            GameObject[] prefabs = bundle.LoadAllAssets<GameObject>();

            if (prefabs.Length > 0)
            {
                var prefab = prefabs[0];
                if (prefab)
                {
                    GameObject go = Instantiate(prefab);
                    go.name = prefabs[0].name;
                    go.transform.position = Vector3.zero;
                }
            }

            bundle.Unload(false);
        }
    }
}