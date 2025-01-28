using Sirenix.OdinInspector;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SpatialTracking;

namespace Rhythem.Play
{
    public class InputModule : Module
    {
        [Title("Player-Specific References")]
        public TrackedPoseDriver head;
        public PlayerWand leftHand;
        public PlayerWand rightHand;

        private PlayerControls _controls;
        private Ray _selectionRay;
        private RaycastHit _selectionHit;

        protected override void Start()
        {
            base.Start();
            _controls ??= new PlayerControls();
            SubscribeToControls();
        }

        protected void OnDestroy()
        {
            UnsubscribeToControls();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _controls ??= new PlayerControls();
            _controls.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _controls?.Disable();
        }

        protected override void Update()
        {
            base.Update();
        }
        private void OnConfirmPerformed(InputAction.CallbackContext context)
        {

        }

        private void OnBackPerformed(InputAction.CallbackContext context)
        {

        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            GameManager.Instance.PauseGame(!GameManager.Instance.Paused);
        }

        private void OnSelectionChangePerformed(InputAction.CallbackContext context)
        {

        }

        protected void SubscribeToControls()
        {
            if (_controls == null) { return; }
            _controls.Player.Interact.performed += OnConfirmPerformed;
            _controls.Player.Back.performed += OnBackPerformed;
            _controls.Player.Pause.performed += OnPausePerformed;
            _controls.Player.ChangeSelection.performed += OnSelectionChangePerformed;
        }

        protected void UnsubscribeToControls()
        {
            if (_controls == null) { return; }
            _controls.Player.Interact.performed -= OnConfirmPerformed;
            _controls.Player.Back.performed -= OnBackPerformed;
            _controls.Player.Pause.performed -= OnPausePerformed;
            _controls.Player.ChangeSelection.performed -= OnSelectionChangePerformed;
        }
    }
}