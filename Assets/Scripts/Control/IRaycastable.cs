namespace RPG.Control
{
    public interface IRaycastable {
        CursorMapping GetCursor(PlayerControls callingControls);
        bool HandleRaycast(PlayerControls callingControls);
    }
}
