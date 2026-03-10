## 1. Fix runtime spline restoration

- [x] 1.1 In `LevelLoader.cs`, restore inline `ApplySplinePoints` method: disable `SpriteShapeController` before modifying spline, re-enable after. Remove `SplineApplier` component usage.
- [x] 1.2 Delete `Scripts/Level/SplineApplier.cs` (no longer needed)

## 2. Fix editor spline restoration

- [x] 2.1 In `LevelEditorWindow.cs` `ApplySplinePoints`: add `Undo.RecordObject(ssc, ...)` before spline modification so changes register as prefab overrides
- [x] 2.2 In `LevelEditorWindow.cs` `SpawnEditorElement`: ensure `EditorUtility.SetDirty` is called on the `SpriteShapeController` component specifically (not just the GameObject)

## 3. Verify

- [ ] 3.1 Manual test: edit terrain spline in editor, save level, load level — terrain shape matches
- [ ] 3.2 Manual test: load level in editor, press Level Editor "Play Level" — terrain shape matches in play mode
- [ ] 3.3 Manual test: load level in editor, press Unity Play button — terrain shape matches in play mode
