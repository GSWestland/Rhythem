using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

#region Resharper Comments
// ReSharper disable MemberCanBeProtected.Global
#endregion

namespace Rhythem.Play
{
    public class InputModule : Module
    {
        [Title("General Input Module References")]
        public bool Moving { get; protected set; }
        public bool Turning { get; protected set; }
        public float CurrentMaxMoveSpeed { get; protected set; } = 100;
        public float DefaultMaxMoveSpeed { get; protected set; } = 100;

        public float SetCurrentMaxMoveSpeed(float newMax)
        {
            return CurrentMaxMoveSpeed = newMax;
        }

        public float ResetMaxMoveSpeed()
        {
            return CurrentMaxMoveSpeed = DefaultMaxMoveSpeed;
        }

        protected override void Update()
        {
            base.Update();
            OnMoving(Moving);
            OnTurning(Turning);
        }

        protected virtual void OnMoving(bool currentlyMoving) { }
        protected virtual void OnTurning(bool currentlyTurning) { }
    }
}