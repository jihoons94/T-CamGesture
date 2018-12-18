using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Treal.BrowserCore;


[InitializeOnLoad]
public class ScriptExecutionOrder : Editor
{

    [UnityEditor.Callbacks.DidReloadScripts]
    static void SetScriptExecutionOrder ()
    {
        AssetDatabase.StartAssetEditing();

        //Debug.Log("SetScriptExecutionOrder()");

        // Get the name of the script we want to change it's execution order
        string cTrackingManager = typeof(CTrackingManager).Name;
        string cMain = typeof(CMain).Name;

        // Iterate through all scripts (Might be a better way to do this?)
        foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
        {
            // If found our script
            if (monoScript.name == cTrackingManager)
            {
                // And it's not at the execution time we want already
                // (Without this we will get stuck in an infinite loop)

                //Debug.Log("CTrackingManager order: " + MonoImporter.GetExecutionOrder(monoScript));

                if (MonoImporter.GetExecutionOrder(monoScript) != -12000)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -12000);
                }
            }

            // If found our script
            if (monoScript.name == cMain)
            {
                // And it's not at the execution time we want already
                // (Without this we will get stuck in an infinite loop)

                //Debug.Log("CMain order: " + MonoImporter.GetExecutionOrder(monoScript));

                if (MonoImporter.GetExecutionOrder(monoScript) != -8000)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -8000);
                }
            }
        }

        AssetDatabase.StopAssetEditing();

    }
	
}
