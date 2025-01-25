using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using UnityEngine.SpatialTracking;
using UnityEditor.Playables;


namespace Rhythem.Player
{
    public class PlayerInputModule : InputModule
    {
        [Title("Player Specific References")] public Player player;
        public TrackedPoseDriver head;
        public PlayerHand leftHand;
        public PlayerHand rightHand;

        private PlayerControls controls;

        public bool CanSnapTurn { get; set; }
        public bool AttemptingTeleport { get; set; }

        protected override void Start()
        {
            base.Start();
            controls ??= new PlayerControls();
            HasRequiredComponents();
            SubscribeToControls();
        }

        protected void OnDestroy()
        {
            UnsubscribeToControls();
        }

        protected override void OnEnable()
        {
            controls ??= new PlayerControls();
            controls.Enable();
        }

        protected override void OnDisable()
        {
            controls?.Disable();
        }

        protected override void Update()
        {
            base.Update();

        }

        private void OnMove_Performed(InputAction.CallbackContext context)
        {
            Moving = true;
        }

        private void OnMove_Canceled(InputAction.CallbackContext context)
        {
            Moving = false;
        }

        private void OnTurn_Performed(InputAction.CallbackContext context)
        {
            Turning = true;
        }

        private void OnTurn_Canceled(InputAction.CallbackContext context)
        {
            Turning = false;
        }

        protected void SubscribeToControls()
        {
            if (controls == null) return;

            // Set up joystick bindings
            controls.Player.Move.performed += OnMove_Performed;
            controls.Player.Move.canceled += OnMove_Canceled;

        }

        protected void UnsubscribeToControls()
        {
            if (controls == null) return;

            // Remove joystick bindings
            controls.Player.Move.performed -= OnMove_Performed;
            controls.Player.Move.canceled -= OnMove_Canceled;
        }

#if UNITY_EDITOR
        private void OnUseHeadDirection(InputAction.CallbackContext context)
        {
            Globals.Instance.UseHeadsetDirection();
        }

        private void OnUseWorldDirection(InputAction.CallbackContext context)
        {
            Globals.Instance.UseWorldDirection();
        }

        private void OnUseControllerDirection(InputAction.CallbackContext context)
        {
            Globals.Instance.UseControllerDirection();
        }
#endif

        protected bool HasRequiredComponents()
        {
            if (head && player && leftHand && rightHand)
            {
                return true;
            }

            Debug.LogError("<color=yellow>The player input module is missing some required components and cannot be used. Desired components:</color>\n" +
                $"TRACKED POSE DRIVER: {(head != null ? "<color=green>Present</color>" : "<color=red>Missing</color>")}\n" +
                $"PLAYER: {(player != null ? "<color=green>Present</color>" : "<color=red>Missing</color>")}\n" +
                $"LEFT HAND: {(leftHand != null ? "<color=green>Present</color>" : "<color=red>Missing</color>")}\n" +
                $"RIGHT HAND: {(rightHand != null ? "<color=green>Present</color>" : "<color=red>Missing</color>")}");
            return false;
        }
    }
}