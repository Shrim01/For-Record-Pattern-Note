using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leasten : MonoBehaviour
{
    public float[] timingList;
    public AudioSource audio;
    public AudioSource click;
    private float dspSongTime;
    private float songPosition;
    private float songLength;
    private string listJson;
    private int index;

    void Start()
    {
        listJson = Data.JsonSong;
        LoadFromJson();
        dspSongTime = (float)AudioSettings.dspTime;
        songLength = audio.clip.length;
        audio.Play();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        if (index<timingList.Length && songPosition>timingList[index])
        {
            click.Play();
            Debug.Log(timingList[index]);
            index++;
        }

        if (songPosition > songLength || !audio.isPlaying)
        {
            Debug.Log(listJson);
        }
    }


    private void LoadFromJson()
    {
        timingList = JsonUtility.FromJson<Class<float>>(listJson).Item;
        foreach (var item in timingList)
            Debug.Log(item);
    }
}
