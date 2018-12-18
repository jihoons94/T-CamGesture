using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MyBuildPostprocessor
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		if (target == BuildTarget.iOS) {
			OnPostprocessBuildIOS (pathToBuiltProject);
		}
	}

	private static void OnPostprocessBuildIOS(string pathToBuiltProject)
	{
		EnableWebCam (pathToBuiltProject);
		SetupXCodeProj (pathToBuiltProject);
	}

	private static void EnableWebCam(string pathToBuiltProject)
	{
		string targetfile = pathToBuiltProject + "/Classes/Preprocessor.h";
		string filecontents = System.IO.File.ReadAllText(targetfile);
		{
			string seed = "#define UNITY_USES_WEBCAM 0";
			string repl = "#define UNITY_USES_WEBCAM 1";

			if (filecontents.Contains(seed))
			{
				Debug.Log("<b>iOSBuildPostProcessor</b> Removing faulty inclusion of CameraCapture");
				filecontents = filecontents.Replace(seed, repl);
			}
			else
			{
				Debug.Log("Seed not found in file: " + seed);
			}
		}
		System.IO.File.WriteAllText (targetfile, filecontents);
	}

	private static void SetupXCodeProj(string pathToBuiltProject)
	{
		string pbxproj = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

		// OpenCV
		string rttiKey = "GCC_ENABLE_CPP_RTTI";
		string rttiValue = "				GCC_ENABLE_CPP_RTTI = YES;";

		List<string> lines = new List<string>();

		foreach (string str in File.ReadAllLines(pbxproj)) {
			if (str.Contains(rttiKey)) { 
				lines.Add(rttiValue);
			} else {
				lines.Add(str);
			}
		}

		// Clear the file
		// http://stackoverflow.com/questions/16212127/add-a-new-line-at-a-specific-position-in-a-text-file
		using (File.Create(pbxproj)) {}

		foreach (string str in lines) {
			File.AppendAllText(pbxproj, str + Environment.NewLine);
		}
	}

}
