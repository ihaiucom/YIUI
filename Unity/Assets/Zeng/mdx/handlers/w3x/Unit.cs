

using System;
using UnityEngine;
using Zeng.mdx.parsers;
using DooUnit = Zeng.mdx.parsers.w3x.unitsdoo.Unit;

namespace Zeng.mdx.handlers.w3x
{
    [Serializable]
    public class Unit : Widget
    {
        [SerializeField]
        public MappedDataRow row;
        [SerializeField]
        public DooUnit unit;
        [SerializeField]
        public string modePath;

        public Unit(War3MapViewerMap map, object model, string modePath, MappedDataRow row, DooUnit unit) : base(map, model)
        {
            this.row = row;
            this.unit = unit;
            this.modePath = modePath;
        }

    }
}
