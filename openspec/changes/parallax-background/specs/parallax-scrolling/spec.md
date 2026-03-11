## ADDED Requirements

### Requirement: Parallax layer scrolls relative to camera movement
Each background layer with a `ParallaxLayer` component SHALL move horizontally at a fraction of the camera's movement. The fraction is defined by a `scrollSpeed` field (0 = no movement/fixed, 1 = moves at full camera speed).

#### Scenario: Distant layer moves slowly
- **WHEN** the camera moves 10 units to the right and a layer has `scrollSpeed = 0.2`
- **THEN** the layer SHALL move 2 units to the right (10 * 0.2)

#### Scenario: Near layer moves faster
- **WHEN** the camera moves 10 units to the right and a layer has `scrollSpeed = 0.8`
- **THEN** the layer SHALL move 8 units to the right (10 * 0.8)

#### Scenario: Layer with zero speed stays fixed
- **WHEN** the camera moves and a layer has `scrollSpeed = 0`
- **THEN** the layer SHALL not move

### Requirement: Parallax offset is calculated from camera start position
The parallax offset SHALL be calculated as `(cameraCurrentX - cameraStartX) * scrollSpeed`. The camera's start X position is captured on `Start()`.

#### Scenario: Parallax resets correctly on scene load
- **WHEN** the scene loads and the camera is at X = 0
- **THEN** all parallax layers SHALL be at their initial positions with zero offset

### Requirement: Infinite horizontal tiling
When `infiniteScroll` is enabled on a `ParallaxLayer`, the system SHALL duplicate the sprite and reposition it to create seamless infinite scrolling. The sprite width is determined from `SpriteRenderer.bounds.size.x`.

#### Scenario: Background repeats when camera moves past sprite boundary
- **WHEN** `infiniteScroll = true` and the camera moves beyond one sprite width
- **THEN** the layer SHALL reposition to maintain seamless coverage of the viewport

#### Scenario: Infinite scroll disabled
- **WHEN** `infiniteScroll = false`
- **THEN** the layer SHALL only apply parallax offset without any repositioning or duplication
