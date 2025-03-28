using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Script : MonoBehaviour
{
    public List<float> timingList = new();
    public AudioSource audio;
    public AudioSource click;
    private float dspSongTime;
    private float songPosition;
    private float songLength;
    private bool start;
    private string listJson;
    private bool flag;
    public GameObject Listen;

    private void Start()
    {
        flag = true;
        songLength = audio.clip.length;
    }

    private void Update()
    {
        if (start)
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        if (Input.GetKeyDown("x"))
        {
            click.Play();
            if (!start)
            {
                audio.Play();
                start = true;
                dspSongTime = (float)AudioSettings.dspTime;
            }
            else
            {
                timingList.Add(songPosition);
                Debug.Log(songPosition);
            }
        }

        if (flag && songPosition > songLength)
        {
            flag = false;
            SaveToJson();
        }
    }


    private void SaveToJson()
    {
        listJson = JsonUtility.ToJson(new Class<float>(timingList.ToArray()), true);
        Data.JsonSong = listJson;
        Debug.Log(listJson);
    }

    public void ListenSong()
    {
        SaveToJson();
        audio.Stop();
        click.Stop();
        Listen.SetActive(true);
        gameObject.SetActive(false);
    }
}

public class Class<T>
{
    public T[] Item;

    public Class(T[] item)
    {
        Item = item;
    }
}