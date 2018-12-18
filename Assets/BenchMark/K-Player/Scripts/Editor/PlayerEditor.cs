using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Editor에서 Player 제어를 위한 Custom Inspector
/// - 필요 기능
/// 1. Sample 목록 확인
/// 2. Sample Load 요청
/// 3. Play / Stop / Pause 등
/// 4. Unload
/// </summary>
[CustomEditor(typeof(KidsPlayer))]
public class PlayerEditor : Editor {
    //private KidsPlayer mPlayer;
    private string mSamplePath;
    private List<string> mSampleStories;

    private void OnEnable()
    {
        //mPlayer = (KidsPlayer)target;
        if (string.IsNullOrEmpty(mSamplePath))
        {
            mSamplePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../SampleStories"));
        }

        var dirs = Directory.GetDirectories(mSamplePath);
        mSampleStories = dirs.Select(dir => dir.Split(new char[]{ '\\', '/' }).Last()).ToList();
    }

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        if (!EditorApplication.isPlaying)
            return;

        foreach (var story in mSampleStories)
        {
            if (GUILayout.Button(story))
            {
                load(story);
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("▶")) { play(); }
            if (GUILayout.Button("||")) { MessageBroker.Default.Publish(new PlayerPauseCommand()); }
            if (GUILayout.Button("■")) { MessageBroker.Default.Publish(new ContentUnloadCommand()); } 
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("show")) { MessageBroker.Default.Publish(new ContentShowCommand { Show = true }); }
            if (GUILayout.Button("hide")) { MessageBroker.Default.Publish(new ContentShowCommand { Show = false }); }
        }
    }

    private void load(string story)
    {

        var path = Path.Combine(mSamplePath, story);

        var files = Directory.GetFiles(path);

        var content = new KContent
        {
            path = path
        };

        if (files.Any(f => f.EndsWith(RolePlayContent.ROLEPLAY_PLAYDATA)))
        {
            content.type = KidsPlayer.TYPE_ROLEPLAY;
            content.sticker = files.FirstOrDefault(f => f.EndsWith(".tro") 
                                        && !f.EndsWith(RolePlayContent.ROLEPLAY_PLAYDATA));
        }
        else if (files.Any(f => f.EndsWith(DrawContent.DRAW_PLAYDATA)))
        {
            content.type = KidsPlayer.TYPE_DRAW;
        }
        else {
            content.type = "";
        }

        MessageBroker.Default.Publish(new ContentLoadCommand {
            Content = content
        });

        //mPlayer.LoadContent(content);
    }

    private void play()
    {
        //1. video Setting
        //2. KidsPlayer play
        MessageBroker.Default.Publish(new PlayerStartCommand());
    }
}
