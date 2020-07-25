using UnityEngine;
using UnityEditor;
using System;

public class SplitViewWindow : EditorWindow
{
    public Rect WindowRect
    {
        get
        {
            return new Rect(0, 0, position.width, position.height);
        }
    }

    public enum SplitDirection
    {
        Vertical,
        Horizontal,
    }

    private SplitDirection splitDirection = SplitDirection.Vertical;
    private float resizeHandlePosition = 0.5f;
    private bool isResize;
    private Vector2[] scrollPositions = new Vector2[2];
    private GUIStyle resizeHandleStyle;

    public virtual void OnEnable()
    {
        wantsMouseMove = true;
    }

    public void SetSplitDirection(SplitDirection splitDirection)
    {
        this.splitDirection = splitDirection;
    }

    public void BeginSplit()
    {
        if(splitDirection == SplitDirection.Horizontal)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            float height = WindowRect.height * resizeHandlePosition;
            scrollPositions[0] = EditorGUILayout.BeginScrollView(scrollPositions[0], GUILayout.Height(height));
        }
        if(splitDirection == SplitDirection.Vertical)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            float width = WindowRect.width * resizeHandlePosition;
            scrollPositions[0] = EditorGUILayout.BeginScrollView(scrollPositions[0], GUILayout.Width(width));
        }
    }

    public void Split()
    {
        if (splitDirection == SplitDirection.Horizontal)
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            Rect resizeHandleRect = new Rect(0, WindowRect.height * resizeHandlePosition - 1.5f, WindowRect.width, 3);
            ProcessRisezeHandle(resizeHandleRect);
            DrawResizeHandle(resizeHandleRect);

            EditorGUILayout.BeginHorizontal();
            float height = WindowRect.height * (1 - resizeHandlePosition);
            scrollPositions[1] = EditorGUILayout.BeginScrollView(scrollPositions[1], GUILayout.Height(height));
        }
        if (splitDirection == SplitDirection.Vertical)
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            Rect resizeHandleRect = new Rect(WindowRect.width * resizeHandlePosition - 1.5f, 0, 3, WindowRect.height);
            ProcessRisezeHandle(resizeHandleRect);
            DrawResizeHandle(resizeHandleRect);

            EditorGUILayout.BeginVertical();
            float width = WindowRect.width * (1 - resizeHandlePosition);
            scrollPositions[1] = EditorGUILayout.BeginScrollView(scrollPositions[1], GUILayout.Width(width));
        }
        if (Event.current.type == EventType.MouseMove || isResize == true)
        {
            Repaint();
        }
    }

    public void EndSplit()
    {
        if (splitDirection == SplitDirection.Horizontal)
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        if (splitDirection == SplitDirection.Vertical)
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void ProcessRisezeHandle(Rect handleRect)
    {
        MouseCursor mouseCursor = (splitDirection == SplitDirection.Horizontal) ? MouseCursor.ResizeVertical : MouseCursor.ResizeHorizontal;
        EditorGUIUtility.AddCursorRect(handleRect, mouseCursor);

        if(handleRect.Contains(Event.current.mousePosition) == true)
        {
            if(Event.current.type == EventType.MouseDown)
            {
                isResize = true;
            }
        }
        if(Event.current.type == EventType.MouseUp)
        {
            isResize = false;
        }
        if(isResize == true)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            resizeHandlePosition = (splitDirection == SplitDirection.Horizontal) ? mousePosition.y / WindowRect.height : mousePosition.x / WindowRect.width;
            resizeHandlePosition = Mathf.Clamp(resizeHandlePosition, 0.2f, 0.8f);
        }
    }

    private void DrawResizeHandle(Rect handleRect)
    {
        if(resizeHandleStyle == null)
        {
            LoadResizeHandleStyle();
        }
        GUI.Box(handleRect, GUIContent.none, resizeHandleStyle);
    }

    private void LoadResizeHandleStyle()
    {
        resizeHandleStyle = new GUIStyle();

        resizeHandleStyle.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/ResizeHandleNormal.png");
        resizeHandleStyle.hover.background = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/ResizeHandleHover.png");
        resizeHandleStyle.overflow = new RectOffset(-2, -2, -2, -2);
    }
}
