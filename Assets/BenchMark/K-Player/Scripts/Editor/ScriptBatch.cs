using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ScriptBatch {
    private const string SKB_TVApps_KidsBooks_WorkingPath = "../SKB_TVApps_KidsBooks/";
    private const string SKB_TVApps_KidsBooks_Debug_Apk = "tv-app-debug.apk";
    private const string SKB_TVApps_KidsBooks_Debug_Align_Apk = "tv-app-debug-align.apk";

    [MenuItem("T real Player/Export/ToAndroid")]
    public static void ExportToAndroid()
    {
        CopyToAndroidProject(ExportProject(), "../Android/trealplayer/src/main");
    }

    [MenuItem("T real Player/Export/STB Repack")]
    public static void ExportToTvAppRepack()
    {
        File.Delete(Path.Combine(SKB_TVApps_KidsBooks_WorkingPath, SKB_TVApps_KidsBooks_Debug_Apk));
        File.Delete(Path.Combine(SKB_TVApps_KidsBooks_WorkingPath, SKB_TVApps_KidsBooks_Debug_Align_Apk));

        var buildBaseDir = ExportProject();

        var apks = Directory.GetFiles(SKB_TVApps_KidsBooks_WorkingPath, "*.apk");
        var latestApk = apks.OrderByDescending(f => Regex.Match(f, "r[0-9]*", RegexOptions.None).Value).FirstOrDefault();

        if (latestApk == null)
        {
            Debug.Log("ExportToTvAppRepack: Not found APK");
            return;
        }

        latestApk = Path.GetFileName(latestApk);

        Debug.Log("ExportToTvAppRepack.LatestAPK: " + latestApk);

        //apktool -d apk
        ProcessUtil.RunProcess("../apktool/apktool.bat", "d " + latestApk,
            Path.GetFullPath(SKB_TVApps_KidsBooks_WorkingPath));

        //copy new unity resources
        var targetBaseDir = SKB_TVApps_KidsBooks_WorkingPath + Path.GetFileNameWithoutExtension(latestApk) + "/";
        DeleteDir(targetBaseDir + "/assets/bin");
        ProcessXcopy(buildBaseDir + "/assets/bin", targetBaseDir + "/assets/bin");
        ProcessXcopy(buildBaseDir + "/jniLibs/armeabi-v7a", targetBaseDir + "/jniLibs/armeabi-v7a");

        //apktool -b apk
        ProcessUtil.RunProcess("../apktool/apktool.bat",
            "b " + Path.GetFileNameWithoutExtension(latestApk) +
            " -o " + SKB_TVApps_KidsBooks_Debug_Apk,
            Path.GetFullPath(SKB_TVApps_KidsBooks_WorkingPath));

        DeleteDir(targetBaseDir);

        //Apk Signing
        ProcessUtil.RunProcess("jarsigner", "-keystore " +
            Path.GetFullPath(Environment.ExpandEnvironmentVariables("%USERPROFILE%/.android/debug.keystore")) +
            " -verbose " + SKB_TVApps_KidsBooks_Debug_Apk + " androiddebugkey -storepass android", SKB_TVApps_KidsBooks_WorkingPath);

        //zipalign 
        var buildtoolpath = Directory.GetDirectories(
                Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "build-tools")
            ).OrderByDescending(p=>p).First();

        var zipalign = Path.Combine(buildtoolpath, "zipalign.exe");
        ProcessUtil.RunProcess(zipalign, 
            string.Format("-v 4 {0} {1}", SKB_TVApps_KidsBooks_Debug_Apk, SKB_TVApps_KidsBooks_Debug_Align_Apk),
            SKB_TVApps_KidsBooks_WorkingPath);

        //Install apk
        string fileName = "adb";
        string args = "install " + SKB_TVApps_KidsBooks_Debug_Align_Apk;
        ProcessUtil.RunProcess(fileName, args, SKB_TVApps_KidsBooks_WorkingPath);

    }

    public static void CopyToAndroidProject(string buildBaseDir, string targetBaseDir)
    {
        DeleteDir(targetBaseDir + "/assets/bin");
        ProcessXcopy(buildBaseDir + "/assets/bin", targetBaseDir + "/assets/bin");
        ProcessXcopy(buildBaseDir + "/jniLibs/armeabi-v7a", targetBaseDir + "/jniLibs/armeabi-v7a");
    }

    public static string ExportProject()
    {
        var buildDir = "Export";

        var buildBaseDir = buildDir + "/" + PlayerSettings.productName + "/src/main";
        

        DeleteDir(buildDir);

        var options = new BuildPlayerOptions()
        {
            scenes = new string[] {
                "Assets/Scenes/main.unity"
            },
            locationPathName = buildDir,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            options = BuildOptions.AcceptExternalModificationsToPlayer
        };

        BuildPipeline.BuildPlayer(options);

        return buildBaseDir;
    }

    [MenuItem("T real Player/CopyAAR")]
    public static void CopyAAR()
    {
        ProcessGradlew("copyAAR");
        AssetDatabase.Refresh();
    }


    private static void ProcessGradlew(string command)
    {
        if (string.IsNullOrEmpty(command))
            return;

        ProcessUtil.RunProcess("../Android/gradlew.bat", command, Path.GetFullPath("../Android/"));
    }

    private static void ProcessXcopy(string sourceDir, string destDir)
    {

        if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(destDir))
            return;

        var Arguments = "\"" + sourceDir + "\"" + " " + "\"" + destDir + "\"" + @" /e /y /I";


        ProcessUtil.RunProcess("xcopy", Arguments);
    }

    private static void DeleteDir(string path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, true);
    }
}