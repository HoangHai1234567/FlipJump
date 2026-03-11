## 1. Create ParallaxController Script

- [ ] 1.1 Create `Scripts/Camera/ParallaxController.cs` — single script on BG parent, uses material texture offset for parallax
- [ ] 1.2 Auto-calculate speed per child based on Z-distance from camera
- [ ] 1.3 Use `SetTextureOffset("_MainTex", ...)` in LateUpdate for smooth scrolling

## 2. Setup Scene

- [ ] 2.1 Add `ParallaxController` component to BG parent in Design.unity
- [ ] 2.2 Ensure background sprite textures have Wrap Mode = Repeat

## 3. Test

- [ ] 3.1 Play the game and verify background layers scroll at different speeds
