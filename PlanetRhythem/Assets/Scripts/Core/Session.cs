using UnityEngine;

namespace Rhythem
{
    /// <summary>
    /// Sessions are the main overall gameplay states of the game. These sessions
    /// can include things like a Splash Session, a Play Session, etc.
    /// </summary>
    public abstract class Session<T> : Session where T : Session<T>
    {
        public static T Instance { get; private set; }

        protected override void Awake()
        {
            Instance = (T)this;
            base.Awake();
        }
    }

    public class Session : MonoBehaviour
    {
        protected virtual void Awake()
        {

        }

        /// <summary>
        /// Handle any initialization needed for this mode.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// WARNING: Should not be called outside of ModeManager. Removes the mode, cleaning itself up.
        /// </summary>
        public virtual void EndSession()
        {

        }

        /// <summary>
        /// WARNING: Should not be called outside of ModeManager. Trigger the pause or unpause functionality needed for this mode.
        /// </summary>
        public virtual void Pause(bool paused)
        {

        }
    }
}
