using Sirenix.OdinInspector;
using UnityEngine.SpatialTracking;
using System;
using System.Collections.Generic;
using static Unity.Collections.Unicode;
using UnityEditor.Playables;
using UnityEngine.TextCore.Text;
using static UnityEngine.ParticleSystem;
using UnityEngine;

namespace Rhythem.Player
{
    public class Player : MonoBehaviour
    {
        public CollisionModule collisionModule;
        public InputModule inputModule;
        public static Player Instance { get; private set; }

        [Title("Player Specific References")] public TrackedPoseDriver head;
        public PlayerHand leftHand;
        public PlayerHand rightHand;

        private PlayerInputModule playerInputModule;
        public PlayerInputModule PlayerInputModule
        {
            get
            {
                if (playerInputModule != null || (playerInputModule = inputModule as PlayerInputModule) != null)
                {
                    return playerInputModule;
                }

                Debug.LogError("Cannot find the PlayerInputModule on the Player.");
                return null;
            }
        }

        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
        }

        protected void Start()
        {

        }
    }
}