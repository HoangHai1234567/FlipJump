using UnityEngine;

public static class InputGate
{
    public static bool locked;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Reset()
    {
        locked = false;
    }
}
