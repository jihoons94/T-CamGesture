using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Timeline;


namespace Treal
{
    public class DrawPlayLoader
    {
        public static DrawPlayData LoadDrawPlayData(string path)
        {
            AssetBundle AssetBundledata = AssetBundle.LoadFromFile(path);

            Debug.Log(AssetBundledata);
            if (AssetBundledata == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return null;
            }

            DrawPlayData PlayData = new DrawPlayData();
            Object[] resource = AssetBundledata.LoadAllAssets();

            for (int index = 0; index < resource.Length; index++)
            {
                if (resource[index].GetType().Equals(typeof(TimelineAsset)))
                {
                    PlayData.RolePlayTimeline = (TimelineAsset)resource[index];
                }
                else if (resource[index].GetType().Equals(typeof(AnimationClip)))
                {
                    if (resource[index].name.Equals("RolePlay"))
                    {
                        PlayData.PoseClip = (AnimationClip)resource[index];
                    }
                }
            }

            AssetBundledata.Unload(false);

            return PlayData;
        }

    }
}

