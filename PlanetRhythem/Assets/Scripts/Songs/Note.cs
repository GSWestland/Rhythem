using Newtonsoft.Json;

namespace Rhythem.Songs
{
    [System.Serializable]
    public class Note
    {
        [JsonProperty] public NoteType noteType;
        [JsonProperty] public DesiredHand hand;
        [JsonProperty] public int noteTime = 0;
        [JsonProperty] public float notePositionX;
        [JsonProperty] public float notePositionY;

        public override string ToString()
        {
            string strout = $"NOTE TYPE: {noteType.ToString()}--HAND: {hand.ToString()}--NOTE TIME: {noteTime}--POSITION: ({notePositionX}, {notePositionY})";
            return strout;
        }

    }
}