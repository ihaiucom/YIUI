
using UnityEngine;

namespace Zeng.mdx.handlers.w3x
{
    public class Widget
    {
        public War3MapViewerMap map;
        public object instance;
        [SerializeField]
        public WidgetState state = WidgetState.IDLE;

        public Widget(War3MapViewerMap map, object instance) {
            this.map = map;
            //this.instance = instance;
        }
    }
}
