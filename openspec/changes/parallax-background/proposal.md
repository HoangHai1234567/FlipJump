## Why

The game currently has 5 forest background layers (bg_forest_1 through bg_forest_5) positioned at different Y values under a "BG" parent, but there is no parallax scrolling script. The backgrounds are static and don't move relative to the camera, making the game feel flat. A parallax system will add depth and polish to the visual experience.

## What Changes

- Add a `ParallaxLayer` script that offsets each background layer based on camera movement, with configurable speed multipliers per layer
- Add infinite horizontal scrolling support so backgrounds repeat seamlessly as the player moves
- Attach the script to each existing bg_forest layer with appropriate speed values (distant layers move slower)

## Capabilities

### New Capabilities
- `parallax-scrolling`: Parallax background system that moves background layers at different speeds relative to camera movement, with optional infinite horizontal tiling

### Modified Capabilities

## Impact

- New script: `Scripts/Camera/ParallaxLayer.cs`
- Modified scene: `Design.unity` — ParallaxLayer component added to each bg_forest sprite under BG parent
- No breaking changes to existing systems
