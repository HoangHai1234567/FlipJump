## Context

The game uses an orthographic 2D camera (`CameraFollow.cs`) that follows the player horizontally. There are 5 forest background layers (bg_forest_1 to bg_forest_5) under a "BG" parent at different Y positions but with no scroll logic. The camera moves in `LateUpdate`, so parallax offsets should also compute in `LateUpdate`.

## Goals / Non-Goals

**Goals:**
- Each background layer scrolls at a configurable fraction of camera movement (0 = static, 1 = moves with camera)
- Distant layers (sky/mountains) move slower, near layers (bushes/trees) move faster
- Optional infinite horizontal tiling for seamless scrolling

**Non-Goals:**
- Vertical parallax (camera only moves horizontally in this game)
- Dynamic background spawning or procedural generation
- Integration with the level serialization system (backgrounds are scene-level, not level-level)

## Decisions

**1. Single `ParallaxLayer` component per sprite**
Each bg_forest sprite gets its own `ParallaxLayer` with a `scrollSpeed` multiplier (0–1). This is simpler than a manager script and allows per-layer tuning in the Inspector.

Alternative: A single `ParallaxManager` controlling all layers — rejected because it couples layers together and is harder to adjust individually.

**2. Offset-based movement (not transform parenting)**
Calculate parallax offset from camera delta each frame and apply to transform.position. The BG parent stays fixed; each child moves independently.

Alternative: Re-parent layers to camera with scaled local positions — rejected because it complicates scene hierarchy and interferes with sorting.

**3. Infinite tiling via sprite width check**
For layers that need to repeat: duplicate the sprite, place both side by side, and when the camera moves past one sprite width, reset position. Use `SpriteRenderer.bounds.size.x` to determine width.

## Risks / Trade-offs

- [Sprite seams] Tiling may show gaps if sprite edges don't tile perfectly → Use sprites designed for seamless tiling or slight overlap
- [Performance] Multiple large background sprites → Negligible for 5 layers in 2D; SpriteRenderer is lightweight
