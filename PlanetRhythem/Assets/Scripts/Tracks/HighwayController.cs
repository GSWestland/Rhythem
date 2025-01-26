using UnityEngine;

public class HighwayController : MonoBehaviour
{
    public Rhythem.TrackEditor.Beatmap beatmap;

    public Transform ringPivot;
    public int measuresPerRotation = 8;


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
        ringPivot.Rotate(new Vector3(0,  (beatmap.bpm/60f) * 360f / beatmap.beatsPerMeasure / measuresPerRotation* Time.fixedDeltaTime,0));
    }
}
