using UnityEngine;
using UnityEngine.Events;

public class ResetButton : MonoBehaviour, ITargetable
{

    public UnityEvent OnRest;

    public void OnHit()
    {
        OnRest.Invoke();
    }
}
