using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class AdbBatch {

    //[MenuItem("ADB/runtime")]
    //public static void Test()
    //{
    //    ProcessUtil.RunProcess("dir");
    //    //EditorApplication.OpenProject(Environment.CurrentDirectory, new string[0]);
    //}

    [MenuItem("ADB/Connect")]
    public static void Connect()
    {
        string fileName = "adb";
        string args = "connect 192.168.0.165";
        ProcessUtil.RunProcess(fileName, args);
    }

    [MenuItem("ADB/RunActivity")]
    public static void RunAcitivity()
    {
        string fileName = "adb";
        string args = "shell am start -a android.intent.action.MAIN -n  "
            + PlayerSettings.applicationIdentifier
            + "/com.sktelecom.trealplayer.TestActivity";
        ProcessUtil.RunProcess(fileName, args);
    }

    [MenuItem("ADB/StopActivity")]
    public static void StopAcitivity()
    {
        string fileName = "adb";
        string args = "shell am force-stop " + PlayerSettings.applicationIdentifier;
        ProcessUtil.RunProcess(fileName, args);
    }

    [MenuItem("ADB/Uninstall")]
    public static void Uninstall() {
        string fileName = "adb";
        string args = "uninstall " + PlayerSettings.applicationIdentifier;
        ProcessUtil.RunProcess(fileName, args);
    }

}