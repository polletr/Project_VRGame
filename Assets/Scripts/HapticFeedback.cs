using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    [Range(0, 1)]
    public float intensity;
    public float duration;

    private void Start()
    {
        XRBaseInteractable interactable = FindAnyObjectByType<XRBaseInteractable>();
        interactable.activated.AddListener(OnTriggerHaptic);
    }

    private void OnTriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controller)
            OnTriggerHaptic(controller.xrController);
    }

    public void OnTriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
            controller.SendHapticImpulse(intensity, duration);
    }
}
