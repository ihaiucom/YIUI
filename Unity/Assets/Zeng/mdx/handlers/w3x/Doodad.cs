

using System;
using UnityEngine;
using Zeng.mdx.parsers;
using DooDoodad = Zeng.mdx.parsers.w3x.doo.Doodad;

namespace Zeng.mdx.handlers.w3x
{
    [Serializable]
    public class Doodad : Widget
    {
        [SerializeField]
        public MappedDataRow row;
        [SerializeField]
        public DooDoodad doodad;
        [SerializeField]
        public string modePath;

        public Doodad(War3MapViewerMap map, object model, string modePath, MappedDataRow row, DooDoodad doodad): base(map, model)
        {
            this.row = row;
            this.doodad = doodad;
            this.modePath = modePath;
        }

    }
}
