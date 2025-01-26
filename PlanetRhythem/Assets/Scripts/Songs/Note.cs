using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythem.Songs
{
    [System.Serializable]
    public class Note
    {
        [JsonProperty] public NoteType noteType;
        [JsonProperty] public DesiredHand hand;
        [JsonProperty] public float notePositionX;
        [JsonProperty] public float notePositionY;

        public void SetNoteInfo(NoteType newType, Vector2 newPosition, DesiredHand newHand)
        {
            noteType = newType;
            notePositionX = newPosition.x;
            notePositionY = newPosition.y;
            hand = newHand;
        }

    }
}