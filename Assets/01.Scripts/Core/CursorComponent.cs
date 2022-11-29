using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType
{
    Default,
}

[System.Serializable]
public class CursorInfo
{
    public CursorType cursorType;
    public Texture2D cursorTexture; 
}


public class CursorComponent : MonoBehaviour
{
    [SerializeField]
    private List<CursorInfo> _cursorList = new List<CursorInfo>();

    private void Start()
    {
        SetCursor(CursorType.Default); 
    }
    public void SetCursor(CursorType cursorType)
    {
        _cursorList.ForEach((x) =>
        {
            if(x.cursorType == cursorType)
            {
                Cursor.SetCursor(x.cursorTexture, new Vector2(x.cursorTexture.width / 5, 0), CursorMode.Auto);
            }
        });
    }
}
