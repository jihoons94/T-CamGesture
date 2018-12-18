using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
namespace Treal
{
    public class RolePlayPlayer : MonoBehaviour
    {
        [SerializeField]
        public Animator Root;
        [SerializeField]
        public Animator FaceMaker;
        [SerializeField]
        public Animator FaceOffset;


        public PlayableDirector TimelinePlayer;

        public RolePlayPlayData PlayData = null;

        public void LoadPlayData(RolePlayPlayData data)
        {
            PlayData = new RolePlayPlayData
            {
                RolePlayTimeline = data.RolePlayTimeline
            };

            if (TimelinePlayer.playableAsset != null)
            {
                TimelinePlayer.Stop();
            }

            TimelinePlayer.playableAsset = PlayData.RolePlayTimeline;

            TimelinePlayer.SetGenericBinding(PlayData.RolePlayTimeline.GetOutputTrack(0), Root);
            TimelinePlayer.SetGenericBinding(PlayData.RolePlayTimeline.GetOutputTrack(1), FaceOffset);

            if (PlayData.RolePlayTimeline.GetOutputTracks().Count() > 2)
            {
                TimelinePlayer.SetGenericBinding(PlayData.RolePlayTimeline.GetOutputTrack(2), FaceMaker);
            }
        }

        public void SetTime(float time)
        {
            //if (Mathf.Abs((float)TimelinePlayer.time - time) > (1f/29.97f)) {
                TimelinePlayer.time = time;
                TimelinePlayer.Evaluate();
            //};
        }

        public void Pause()
        {
            TimelinePlayer.Pause();
        }

        public void Stop()
        {
            TimelinePlayer.Stop();
        }

        public void StartPlay()
        {
            TimelinePlayer.Play(PlayData.RolePlayTimeline);
        }

        public void ClearPlayData()
        {
            TimelinePlayer.SetGenericBinding(null, Root);
            TimelinePlayer.SetGenericBinding(null, FaceOffset);

            if (PlayData.RolePlayTimeline != null)
            {
                if (PlayData.RolePlayTimeline.GetOutputTracks().Count() > 2)
                {
                    TimelinePlayer.SetGenericBinding(null, FaceMaker);
                }

                foreach (var track in PlayData.RolePlayTimeline.GetOutputTracks())
                {
                    foreach (var clip in track.GetClips())
                    {
                        DestroyImmediate(clip.animationClip, true);
                    }
                }

                DestroyImmediate(PlayData.RolePlayTimeline, true);
                PlayData.RolePlayTimeline = null;
            }

            if(PlayData != null)
            {
                PlayData = null;
            }
        }
    }
}

