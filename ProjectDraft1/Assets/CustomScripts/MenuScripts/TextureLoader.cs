using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load texture files from bundles and set them to all required materials
/// </summary>
public class TextureLoader : MonoBehaviour
{
    public string BundlesRootPath
    {
        get
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath;
#elif UNITY_ANDROID
return Application.persistentDataPath
#endif
        }
    }

    Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();
    public AssetBundle LoadBundle(string bundleName)
    {
        
    }
}
