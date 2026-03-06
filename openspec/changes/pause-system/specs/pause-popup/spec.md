## ADDED Requirements

### Requirement: Pause popup displays with animation
The PopupPause popup SHALL extend PopupBase and display a dark overlay background, a panel, and two buttons (Resume, Home) with entrance animation when instantiated.

#### Scenario: Popup appears on pause
- **WHEN** GameManager.Pause() is called
- **THEN** PopupPause prefab is instantiated with background fade-in, panel scale-up (OutBack ease), and buttons scale-up with delay

### Requirement: Resume button closes popup and resumes game
The Resume button SHALL close the popup with exit animation and call GameManager.Resume().

#### Scenario: Player taps Resume
- **WHEN** player taps the Resume button on PopupPause
- **THEN** the panel scales down (InBack ease), the popup is destroyed, and GameManager.Resume() is called

### Requirement: Home button closes popup and returns to level 0
The Home button SHALL close the popup with exit animation and call GameManager.Home().

#### Scenario: Player taps Home from pause
- **WHEN** player taps the Home button on PopupPause
- **THEN** the panel scales down, the popup is destroyed, and GameManager.Home() is called (resets to level 0 and reloads scene)

### Requirement: Popup animations work at timeScale 0
All DOTween animations in PopupPause SHALL use SetUpdate(true) so they play while the game is paused.

#### Scenario: Animations play during pause
- **WHEN** PopupPause is instantiated while Time.timeScale is 0
- **THEN** all entrance and exit animations play at normal speed
