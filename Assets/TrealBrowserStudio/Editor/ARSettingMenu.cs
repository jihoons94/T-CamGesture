using UnityEngine;
using UnityEditor;

public class ARSettingMenu
{

    [MenuItem("GameObject/T real/AR Setting", false, 11)]
    static void ARSetting()
    {
        string path = "Assets/Treal/Framework/ARCamera/Prefab/TCamManager.prefab";

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (prefab != null)
        {
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        }
        else
        {
            Debug.Log("T Cam Manager Prefab Missing!");
        }
    }
}
