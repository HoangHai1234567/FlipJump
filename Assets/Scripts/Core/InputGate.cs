using UnityEngine;
using UnityEngine.EventSystems;

public static class InputGate
{
    public static bool locked;

    public static bool IsBlocked =>
        locked || (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject());

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Reset()
    {
        locked = false;
    }
}
