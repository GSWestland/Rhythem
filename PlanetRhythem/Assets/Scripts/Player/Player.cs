using Rhythem.Songs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhythem.Player
{
    public class Player : MonoBehaviour
    {
        [Title("Input Actions")]
        InputAction confirm;
        InputAction back;
        InputAction navigate;

        [Title("Events")]
        public delegate void OnNoteHit(ScorableNote note);
        void Start()
        {
            confirm = InputSystem.actions.FindAction("");
        }

        void Update()
        {

        }
    }
}