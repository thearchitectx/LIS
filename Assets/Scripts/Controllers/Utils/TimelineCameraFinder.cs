using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineCameraFinder : MonoBehaviour
{
    void Start()
    {
        var director = this.transform.GetComponent<PlayableDirector>();
        var timelineAsset = director.playableAsset as TimelineAsset;

        foreach (var t in timelineAsset.GetRootTracks())
        {
            var track = t as CinemachineTrack;
            if (track != null)
                director.SetGenericBinding(track, Camera.main.GetComponent<CinemachineBrain>());
        }
    }
}