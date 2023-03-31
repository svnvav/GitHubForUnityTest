using System.Linq;
using UnityEditor;
using UnityEngine;

public class MegaBundleFixer
{
    [MenuItem("Assets/_TryFixBundles")]
    public static void TryFixBundles()
    {
        string[] allBundlesNames = AssetDatabase.GetAllAssetBundleNames();
        string[] availableExtensions = { ".prefab", ".asset", ".fbx", ".controller", ".overrideController" };
        Debug.Log("Starting!");

        AssetDatabase.StartAssetEditing();
        foreach (var bundlesName in allBundlesNames)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(bundlesName);

            var hasPrefab = assetPaths.Any(x => x.EndsWith(availableExtensions[0]));
            if(!hasPrefab) continue;

            foreach (var assetPath in assetPaths)
            {
                bool isAssetPrefab = assetPath.EndsWith(availableExtensions[0]);
                if (!isAssetPrefab)
                {
                    AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                    string cachedAssetBundleName = assetImporter.assetBundleName;

                    if (cachedAssetBundleName.EndsWith(".bundle"))
                    {
                        cachedAssetBundleName =
                            cachedAssetBundleName.Substring(0, cachedAssetBundleName.Length - ".bundle".Length) 
                            + "_resources.bundle";
                    }
                    else
                    {
                        cachedAssetBundleName += "_resources";
                    }
                    assetImporter.assetBundleName = cachedAssetBundleName;
                }
            }
        }
        AssetDatabase.StopAssetEditing();
        
        Debug.Log("Fixed!");
    }
}
