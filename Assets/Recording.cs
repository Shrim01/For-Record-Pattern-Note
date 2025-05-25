using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Recording : MonoBehaviour
{
    public List<float> noteTiming = new();
    public List<string> longNoteTiming = new();
    public List<string> spinnerTiming = new();
    public AudioSource audio;
    public AudioSource click;
    private float dspSongTime;
    private float songPosition;
    private float songLength;
    private bool isStart;
    private string listJson;
    private bool flag;
    private (float start, float end) longNote;
    private (float start, float end) spinner;
    public GameObject listen;
    private (string note, string longNote, string spinner) key;

    private void Start()
    {
        isStart = false;
        key = (note: "x", longNote: "c", spinner: "v"); //Тут можно менять кллавиши отвечаюшие за оегистрацию нажатий
        flag = true;
        songLength = audio.clip.length;
    }

    private void Update()
    {
        if (!isStart && Input.GetKeyDown("s"))
        {
            audio.Play();
            isStart = true;
            dspSongTime = (float)AudioSettings.dspTime;
            Debug.Log("start");
        }
        else if (isStart)
            RegisterKey();
    }

    private void RegisterKey()
    {
        
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
            
        if (Input.GetKeyDown(key.note))
        {
            click.Play();
            {
                noteTiming.Add(songPosition);
            }
        }

        if (Input.GetKeyDown(key.longNote))
            longNote.start = songPosition;

        if (Input.GetKeyUp(key.longNote))
        {
            longNote.end = songPosition;
            longNoteTiming.Add(TupleFloatToSting(longNote));
        }


        if (Input.GetKeyDown(key.spinner))
            spinner.start = songPosition;

        if (Input.GetKeyUp(key.spinner))
        {
            spinner.end = songPosition;
            spinnerTiming.Add(TupleFloatToSting(spinner));
        }

        if (flag && songPosition > songLength)
        {
            flag = false;
            SaveToJson();
        }
    }

    private void SaveToJson()
    {
        listJson = JsonUtility.ToJson(
            new Class<float>(
                noteTiming.ToArray(),
                longNoteTiming.ToArray(),
                spinnerTiming.ToArray()
            ), true);
        Data.JsonSong = listJson;
        Debug.Log(listJson);
    }

    public void ListenSong()
    {
        SaveToJson();
        audio.Stop();
        click.Stop();
        listen.SetActive(true);
        gameObject.SetActive(false);
    }

    private string TupleFloatToSting((float start, float end) tuple)
    {
        return tuple.start + " " + tuple.end;
        
    }
}

public class Class<T>
{
    public T[] Note;
    public string[] LongNote;
    public string[] Spinner;

    public Class(T[] note, string[] longNote, string[] spinner)
    {
        Note = note;
        LongNote = longNote;
        Spinner = spinner;
    }
}