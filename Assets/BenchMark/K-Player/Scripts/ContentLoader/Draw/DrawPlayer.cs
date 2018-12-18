using UnityEngine;
using UnityEngine.Playables;

namespace Treal
{
    public class DrawPlayer : MonoBehaviour {

        public Animator Root;
        public Animator Draw;

        public PlayableDirector TimelinePlayer;

        public DrawPlayData PlayData = null;

        public void LoadPlayData(DrawPlayData data)
        {
            PlayData = new DrawPlayData();
            PlayData.RolePlayTimeline = data.RolePlayTimeline;
            PlayData.PoseClip = data.PoseClip;

            if (TimelinePlayer.playableAsset != null)
            {
                TimelinePlayer.Stop();
            }

            TimelinePlayer.playableAsset = PlayData.RolePlayTimeline;

            TimelinePlayer.SetGenericBinding(PlayData.RolePlayTimeline.GetOutputTrack(0), Root);
            TimelinePlayer.SetGenericBinding(PlayData.RolePlayTimeline.GetOutputTrack(1), Draw);
        }

        public void StartPlay()
        {
            TimelinePlayer.Play(PlayData.RolePlayTimeline);
        }

        public void SetTime(float time)
        {
            TimelinePlayer.time = time;
        }

        public void Pause()
        {
            TimelinePlayer.Pause();
        }

        public void Stop()
        {
            TimelinePlayer.Stop();
        }

        public void ClearPlayData()
        {
            if (PlayData.RolePlayTimeline != null)
            {
                //DestroyImmediate(PlayData.RolePlayTimeline);
                PlayData.RolePlayTimeline = null;
            }
            if (PlayData.PoseClip != null)
            {
                //DestroyImmediate(PlayData.PoseClip);
                PlayData.PoseClip = null;
            }
            if (PlayData != null)
            {
                //DestroyImmediate(PlayData);
                PlayData = null;
            }
        }
    }

}
