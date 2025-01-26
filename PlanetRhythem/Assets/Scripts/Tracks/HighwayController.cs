using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;

public class HighwayController : MonoBehaviour
{
    public Rhythem.TrackEditor.Beatmap beatmap;



    public Transform ringPivot;
    public int measuresPerRotation = 8;

    public EventReference popSFXEvent;
    public EventReference missSFXEvent;
    public EventReference musicEvent;

    public int score = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetBeatmap(Rhythem.TrackEditor.Beatmap beatmap)
    {
        this.beatmap = beatmap;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        UpdateRing();

    }

    void UpdateRing()
    {
        ringPivot.Rotate(new Vector3(0,  (beatmap.bpm/60f) * 360f / beatmap.beats / measuresPerRotation * Time.fixedDeltaTime, 0));
    }
}
