using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorVideoPlayer))]
public class EditorVideoController : Editor
{


    public override void OnInspectorGUI(){
        var t = (EditorVideoPlayer)target;

        var video = t.mVideoPlayer;
        //var plugin = t.mContentPlayer;
        EditorGUILayout.BeginHorizontal();

        var curFrame = video == null ? 0 : (int)video.frame;
        var maxFrame = video == null ? 0 : (int)video.frameCount;
        var targetFrame = EditorGUILayout.IntSlider( curFrame, 0, maxFrame);

        if (curFrame != targetFrame)
        {
            video.frame = targetFrame;
            //plugin.AndroidGoFrame((int)video.frame);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("▶"))
        {
            //plugin.AndroidPlay();
            video.Play();
        }

        if (GUILayout.Button("■"))
        {
            //plugin.AndroidStop();
            video.Stop();
        }

        EditorGUILayout.EndHorizontal();
    }
}