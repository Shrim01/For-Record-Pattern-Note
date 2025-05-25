using System.Linq;
using UnityEngine;

public class Leasten : MonoBehaviour
{
    public float[] noteTiming;
    public (float start,float end)[] longNoteTiming;
    public (float start,float end)[] spinnerTiming;
    public AudioSource audio;
    public AudioSource click;
    private float dspSongTime;
    private float songPosition;
    private float songLength;
    private string listJson;
    private int[] index = new int[3];

    private void Start()
    {
        listJson = Data.JsonSong;
        LoadFromJson();
        dspSongTime = (float)AudioSettings.dspTime;
        songLength = audio.clip.length;
        audio.Play();
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        if (index[0]<noteTiming.Length && songPosition>noteTiming[index[0]])
        {
            click.Play();
            index[0]++;
        }
        
        if (index[1]<longNoteTiming.Length && songPosition>longNoteTiming[index[1]].start)
        {
            click.Play();
            index[1]++;
        }
        
        if (index[2]<noteTiming.Length && songPosition>spinnerTiming[index[2]].start)
        {
            click.Play();
            index[2]++;
        }
        
        if (songPosition > songLength || !audio.isPlaying)
        {
            Debug.Log(listJson);
        }
    }

    private void LoadFromJson()
    {
        var timing = JsonUtility.FromJson<Class<float>>(listJson);
        noteTiming = timing.Note;
        longNoteTiming = ParsingTiming(timing.LongNote);
        spinnerTiming = ParsingTiming(timing.Spinner);
    }

    private (float, float)[] ParsingTiming(string[] timings)
    {
        return timings.Select(x =>
        {
            var a = x.Split()
                .Select(float.Parse);
            return (a.ElementAt(0), a.ElementAt(1));
        }).ToArray();
    }
}
