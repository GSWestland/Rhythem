using Rhythem;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Rhythem.Player
{
    public class PlayerHand : MonoBehaviour
    {
        public DesiredHand desiredHand;
        public RuntimeAnimatorController animator;

        private Animator anim;
        private float time;

        protected void Awake()
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }

            anim.runtimeAnimatorController = animator;
        }

        protected void Update()
        {

        }
    }
}