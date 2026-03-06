## ADDED Requirements

### Requirement: HUD displays current level name
The in-game HUD SHALL show the current level name (e.g., "Level 1") at the top of the screen.

#### Scenario: Level name shown on start
- **WHEN** the game scene loads
- **THEN** a text element at the top-center of the screen SHALL display "Level X" where X is the current level number

### Requirement: HUD has a restart button
The HUD SHALL include a restart button that allows the player to restart the current level at any time during gameplay.

#### Scenario: Restart button tapped during gameplay
- **WHEN** the player taps the restart button while GameManager.State is Playing
- **THEN** GameManager.Replay() SHALL be called
- **AND** the scene SHALL reload

#### Scenario: Restart button hidden when popup is showing
- **WHEN** GameManager.State is Won or Lost
- **THEN** the restart button SHALL be hidden or non-interactable

### Requirement: HUD uses screen-space overlay Canvas
The HUD SHALL be rendered on a Canvas set to Screen Space - Overlay mode, ensuring it renders on top of all game elements.

#### Scenario: HUD visible regardless of camera position
- **WHEN** the camera moves to follow the player
- **THEN** the HUD elements SHALL remain fixed at their screen positions
