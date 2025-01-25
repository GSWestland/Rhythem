using Rhythem.Core;
using UnityEngine;

namespace Rhythem
{
    /// <summary>
    /// The main sessions manager that will be used throughout the project.
    /// This manager basically handles all of the "states" of the game, or sessions.
    /// For example, Splash Session, Play Session, etc.
    /// </summary>
    public class SessionsManager : Manager<SessionsManager>
    {
        protected Session currentSession;

        public T GetCurrentSession<T>() where T : Session
        {
            return (T)currentSession;
        }

        public Session GetCurrentSession()
        {
            return currentSession;
        }

        public T LoadSession<T>(bool initialize = true) where T : Session
        {
            var desiredMode = typeof(T);

            if (currentSession != null)
            {
                currentSession.EndSession();
                Destroy(currentSession);
            }

            currentSession = (T)gameObject.AddComponent(desiredMode);

            if (initialize)
            {
                currentSession.Initialize();
            }

            return currentSession as T;
        }
    }
}
