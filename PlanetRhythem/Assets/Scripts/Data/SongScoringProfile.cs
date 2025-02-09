using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SongScoringProfile", menuName = "Rhythem/SongScoringProfile")]
public class SongScoringProfile : ScriptableObject
{
    [Title("Energy")]
    public int energyStartValue;
    public int energyLossFromMiss;
    public int EnergyLossFromObstacle;
    public int energyRegain;

    [Title("Score")]
    public int scoreNoteStellar;
    public int scoreNoteGreat;
    public int scoreNoteGood;
    public int scoreNoteClose;
    public int scoreNoteMiss;
    public int scoreNoteObstacle;
}
