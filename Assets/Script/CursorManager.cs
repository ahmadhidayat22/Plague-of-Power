using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    // public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        setCustomCursor();
    }
    public void setCustomCursor()
    {
        Vector2 hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);

        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);

    }
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
