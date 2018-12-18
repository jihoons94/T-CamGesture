using System.IO;
using UnityEngine;
using UnityEngine.Timeline;

namespace Treal
{
    public class RolePlayLoader
    {
        public static RolePlayPlayData LoadRolePlayPlayData(string path)
        {
            path = Path.GetFullPath(path);
            
            AssetBundle AssetBundledata = AssetBundle.LoadFromFile(path);

            if (AssetBundledata == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return null;
            }

            RolePlayPlayData PlayData = new RolePlayPlayData();
            Object[] resource = AssetBundledata.LoadAllAssets();

            for (int index = 0; index < resource.Length; index++)
            {
                if(resource[index].GetType().Equals(typeof(TimelineAsset)))
                {
                    PlayData.RolePlayTimeline = (TimelineAsset)resource[index];
                }
            }

            AssetBundledata.Unload(false);

            return PlayData;
        }
    }

}
