using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeAudioPlayer
{
	private static Dictionary<string, AudioClip> clipCache = new Dictionary<string, AudioClip>();
	
	public static AudioClip GetAndCache(string clip) {
		if (clipCache.ContainsKey(clip)) {
			return clipCache[clip];
		}
		
		AudioClip clipObj = Resources.Load<AudioClip>(string.Format("SFX/{0}", clip));
		clipCache.Add(clip, clipObj);
		if (clipObj == null)
			Debug.LogWarning($"Can't load clip \"SFX/{0}\"");
		return clipObj;
	}

	public static float Play (string clipRes)  {
		AudioClip clip = GetAndCache(clipRes);
		if (clip!=null) {
			GameObject tempAudioSource = new GameObject("TempAudio");
			AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.volume = 1;
			audioSource.spatialBlend = 0.0f;
			audioSource.Play();
			GameObject.Destroy(tempAudioSource, clip.length);
			return clip.length;
		}
		return 0;
	}

}

