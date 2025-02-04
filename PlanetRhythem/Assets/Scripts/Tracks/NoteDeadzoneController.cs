using Rhythem.Songs;
using UnityEngine;
using UnityEngine.Events;

namespace Rhythem.Tracks
{
    public class NoteDeadzoneController : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<ScorableNote> OnNoteMissed;

        void Start()
        {

        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Note")
            {
                ScorableNote scorable = other.gameObject.GetComponent<ScorableNote>();
                OnNoteMissed.Invoke(scorable);
                //scorable.DisableNote();
            }
        }
    }
}