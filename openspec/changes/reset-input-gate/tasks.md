## 1. Update InputGate

- [x] 1.1 Add `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]` method to `InputGate` that sets `locked = false`
- [x] 1.2 Add a public static `Reset()` method to `InputGate` that sets `locked = false`
