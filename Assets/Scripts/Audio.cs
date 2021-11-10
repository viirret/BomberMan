using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    // the main mixer
    public static AudioMixer mixer;
    static GameObject audioObj;
    // list of already loaded clips
    static Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    static bool created = false;

    void Awake()
    {
        if(!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
            Destroy(this.gameObject);
        
        audioObj = gameObject;
        mixer = Resources.Load<AudioMixer>("myMixer");
    }

    // load sounds in different classes
    public static AudioSource LoadSound(string path, string group, GameObject target)
    {
        // only load clip once
        AudioClip clip;
        if(clips.ContainsKey(path))
            clip = clips[path];
        else
        {
            clip = Resources.Load<AudioClip>(path);
            clips.Add(path, clip);   
        }

        // make the audio
        AudioSource source = target.AddComponent<AudioSource>();
        AudioMixerGroup mixerGroup = mixer.FindMatchingGroups(group)[0];
        source.clip = clip;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = mixerGroup;
        return source;
    }

    // without params loaded to this class
    public static AudioSource LoadSound(string path, string group)
    {
        return LoadSound(path, group, audioObj);
    }
}
