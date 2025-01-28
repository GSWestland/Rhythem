using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using System.Collections.Generic;

public class PlayerWand : MonoBehaviour
{

    public DesiredHand desiredHand;
    public ControllerInputActionManager controllerManager;
    public Transform lineRenderStartTransform;

    private LineRenderer _selectionAssistant;
    public bool inMenuMode = true;

    void Start()
    {
        _selectionAssistant = lineRenderStartTransform.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (inMenuMode)
        {
            List<Vector3> linePts = new();
            linePts.Add(lineRenderStartTransform.position);
            linePts.Add(lineRenderStartTransform.position + (lineRenderStartTransform.up * 4f));
            _selectionAssistant.SetPositions(linePts.ToArray());
        }
    }
}
