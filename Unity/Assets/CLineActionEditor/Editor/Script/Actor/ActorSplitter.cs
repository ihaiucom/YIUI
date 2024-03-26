/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Actor\ActorSplitter.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-10-22      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine.Editor
{
    using UnityEngine;
    using UnityEditor;

    internal sealed class ActorSplitter : Actor
    {
        // header splitter
        private readonly float headerSplitterWidth = 6f;
        private readonly float headerSplitterWidthVisual = 2f;

        [SerializeReference] private System.Action<float> setter = null;
        [SerializeReference] private System.Func<float> getter = null;

        public void Init(System.Action<float> setter, System.Func<float> getter)
        {
            this.setter = setter;
            this.getter = getter;

            rect = new Rect(getter() - headerSplitterWidth * 0.5f,
                window.toobarHeight,
                headerSplitterWidthVisual,
                window.rectWindow.height);

            AddManipulator(new ActorSplitterManipulator(this));
        }

        public override void Draw()
        {
            if (getter != null)
            {
                rect.x = getter() - headerSplitterWidth * 0.5f;
                rect.height = window.rectWindow.height;
                EditorGUI.DrawRect(rect, window.editorResources.colorTopOutline3);

                if (GUIUtility.hotControl == 0)
                    EditorGUIUtility.AddCursorRect(rect, MouseCursor.SplitResizeLeftRight);
            }
        }

        public void OnMouseDrag(Event evt)
        {
            setter?.Invoke(evt.mousePosition.x);
        }

    }
}