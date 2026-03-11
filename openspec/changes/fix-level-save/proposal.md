## Tại sao

Khi load level, `LevelLoader` spawn prefab gốc (với collider kích thước lớn) rồi mới apply dữ liệu từ JSON (scale, spline, components). Trong khoảng thời gian giữa Instantiate và Apply, player đã va chạm với collider prefab gốc → kích hoạt GroundLanding/HeadCollision → thua ngay lập tức dù chưa thực sự chơi.

## Thay đổi gì

- Tắt collider/physics trên tất cả level elements khi spawn, chỉ bật lại sau khi apply xong toàn bộ dữ liệu JSON
- Freeze player (tắt physics) trong quá trình load, chỉ unfreeze khi level đã sẵn sàng
- Đợi 1 frame sau khi spawn + apply xong rồi mới bật collider và player physics (đảm bảo Unity đã cập nhật mesh/collider)

## Capabilities

### New Capabilities
- `safe-level-spawn`: Đảm bảo level elements được spawn và apply dữ liệu hoàn toàn trước khi bật physics/collision, tránh va chạm sớm

### Modified Capabilities

## Impact

- Sửa file: `Scripts/Level/LevelLoader.cs` — thêm logic disable/enable collider và freeze/unfreeze player khi load
- Không ảnh hưởng đến editor (LevelEditorWindow) vì chỉ xảy ra ở runtime
- Không breaking changes
