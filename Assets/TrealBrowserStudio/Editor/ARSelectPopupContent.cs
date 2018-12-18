using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ARSelectPopupContent : PopupWindowContent
{
    UnityEngine.Object tempObj;
    private bool preLoad;

    public override void OnGUI(Rect rect)
    {
        for (int i = 0; i < child_tro.arraySize; i++)
        {
            UnityEngine.Object troObj = null;
            if (child_tro.arraySize > 0)
            {
                troObj = LoadObject(child_tro.GetArrayElementAtIndex(i).FindPropertyRelative("contents_path").stringValue);
            }
            EditorGUI.BeginChangeCheck();
            UnityEngine.Object obj = EditorGUILayout.ObjectField(troObj, typeof(GameObject), false);
            if (EditorGUI.EndChangeCheck())
            {
                if (obj == null)
                {
                    child_tro.DeleteArrayElementAtIndex(i);
                    //window.ApplyModifiedPropertiesMarker();
                }
                else
                {
                    var e = child_tro.GetArrayElementAtIndex(i);
                    e.FindPropertyRelative("contents_path").stringValue = AssetDatabase.GetAssetPath(obj);
                    //window.ApplyModifiedPropertiesMarker();
                }

            }
        }

        EditorGUI.BeginChangeCheck();
        UnityEngine.Object newobj = EditorGUILayout.ObjectField(tempObj, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            if (newobj == null)
            {

            }
            else
            {
                child_tro.arraySize++;
                var e = child_tro.GetArrayElementAtIndex(child_tro.arraySize - 1);
                e.FindPropertyRelative("contents_path").stringValue = AssetDatabase.GetAssetPath(newobj);
                //window.ApplyModifiedPropertiesMarker();
            }
        }
    }

    private GameObject LoadObject(string fileName)
    {
        return AssetDatabase.LoadAssetAtPath(fileName, typeof(GameObject)) as GameObject;
    }

    public override void OnOpen()
    {
        //Debug.Log("표시할 때에 호출됨");
    }

    public override void OnClose()
    {
        //Debug.Log("닫을때 호출됨");
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(250, (child_tro.arraySize + 1) * (EditorGUIUtility.singleLineHeight + 2));
    }

    SerializedProperty child_tro;
    Treal_ImageDescriptorBuilder window;

    public void Init(Treal_ImageDescriptorBuilder window, SerializedProperty serializedProperty)
    {
        this.window = window;
        child_tro = serializedProperty;
    }

    public void Close()
    {
        if (editorWindow != null)
            editorWindow.Close();
    }
}

