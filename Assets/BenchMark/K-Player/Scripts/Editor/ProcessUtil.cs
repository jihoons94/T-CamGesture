using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ProcessUtil {
    public static bool RunProcess(string FileName, string Args = "", string workingDirectory = "")
    {
        Process process = new Process();
        try
        {
            process.StartInfo.FileName = FileName;
            process.StartInfo.Arguments = Args;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }


    private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
            UnityEngine.Debug.Log(e.Data);

    }

    private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
            UnityEngine.Debug.LogError(e.Data);
    }
}