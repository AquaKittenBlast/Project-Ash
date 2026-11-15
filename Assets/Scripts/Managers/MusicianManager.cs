using System.Collections.Generic;
using UnityEngine;

public class MusicianManager : MonoBehaviour
{
    [Header("Musicians")]
    public GameObject clarinet;
    public GameObject drums;
    public GameObject guitar;
    public GameObject bass;
    [Header("Bools")]
    [SerializeField] bool clarinetPlaying = true;
    [SerializeField] bool drumsPlaying = true;
    [SerializeField] bool guitarPlaying = true;
    [SerializeField] bool bassPlaying = true;
    [SerializeField] bool pianoPlaying = false;
    [Header("Stems")]
    public AudioSource guitarAudio;
    public AudioSource bassAudio;
    public AudioSource drumsAudio;
    public AudioSource clarinetAudio;
    public AudioSource pianoAudio1;
    public AudioSource pianoAudio2;
    public AudioSource rev;
    public AudioSource firePlace;
    [Header("Variables")]
    public float volume;

    MusicianScript clarinetScript;
    MusicianScript drumsScript;
    MusicianScript guitarScript;
    MusicianScript bassScript;
    private List<AudioSource> stems = new List<AudioSource>();



    void Awake()
    {
        clarinetScript = clarinet.GetComponent<MusicianScript>();
        drumsScript = drums.GetComponent<MusicianScript>();
        guitarScript = guitar.GetComponent<MusicianScript>();
        bassScript = bass.GetComponent<MusicianScript>();
        stems.AddRange(new AudioSource[] { guitarAudio, bassAudio, drumsAudio, clarinetAudio, pianoAudio1, pianoAudio2, rev, firePlace });
        foreach (AudioSource x in stems)
        {
            x.Play();
            x.volume = 0;
            x.loop = true;
        }
        rev.volume = volume;
        firePlace.volume = volume;
    }

    void Update()
    {
        if (guitarPlaying) { guitarAudio.volume = volume; } else { guitarAudio.volume = 0; }
        if (bassPlaying) { bassAudio.volume = volume; } else { bassAudio.volume = 0; }
        if (drumsPlaying) { drumsAudio.volume = volume; } else { drumsAudio.volume = 0; }
        if (clarinetPlaying) { clarinetAudio.volume = volume; } else { clarinetAudio.volume = 0; }
        if (pianoPlaying) { pianoAudio1.volume = volume; pianoAudio2.volume = volume; } else { pianoAudio1.volume = 0; pianoAudio2.volume = 0;}
        #region Animator Variable Setting
        if (clarinetPlaying) { clarinetScript.SetPlayingState(true); }
        else { clarinetScript.SetPlayingState(false); }

        if (drumsPlaying) { drumsScript.SetPlayingState(true); }
        else { drumsScript.SetPlayingState(false); }

        if (guitarPlaying) { guitarScript.SetPlayingState(true); }
        else { guitarScript.SetPlayingState(false); }

        if (bassPlaying) { bassScript.SetPlayingState(true); }
        else { bassScript.SetPlayingState(false); }
        MusicianScript.audioTime = firePlace.time;
        #endregion
    }
}
