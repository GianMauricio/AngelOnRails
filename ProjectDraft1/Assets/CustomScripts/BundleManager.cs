using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BundleManager : MonoBehaviour
{

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public string BundlesRootPath
    {
        get
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath;
#elif UNITY_ANDROID
            return Application.persistentDataPath;
#endif
        }
    }

    Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();

    public AssetBundle LoadBundle(string bundleName)
    {
        if (LoadedBundles.ContainsKey(bundleName))
        {
            return LoadedBundles[bundleName];
        }

        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(BundlesRootPath, bundleName));

        if (bundle == null)
        {
            Debug.LogError($"{bundleName} is missing!");
        }
        else
        {
            LoadedBundles.Add(bundleName, bundle);
        }

        return bundle;
    }

    public T GetAsset<T>(string bundleName, string asset) where T : Object
    {
        T ret = null;

        AssetBundle bundle = LoadBundle(bundleName);

        if (bundle != null)
        {
            ret = bundle.LoadAsset<T>(asset);
        }

        return ret;
    }
}
