using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Rhythem.Core
{
    public class UiManager : Manager
    {
        private Canvas _uiBase;

        protected override void Awake()
        {
            _uiBase = GetComponent<Canvas>();
            _uiBase.worldCamera = GameManager.Instance.VRRig.GetComponentInChildren<XROrigin>().Camera;
        }
    }
}