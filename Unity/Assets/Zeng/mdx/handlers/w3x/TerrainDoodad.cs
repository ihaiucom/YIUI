using System;
using UnityEngine;
using Zeng.mdx.parsers;
using DooTerrainDoodad = Zeng.mdx.parsers.w3x.doo.TerrainDoodad;
namespace Zeng.mdx.handlers.w3x
{

    /**
     * A cliff/terrain doodad.  悬崖/地形装饰物。
     */
    [Serializable]
    public class TerrainDoodad
    {
        private object instance;
        [SerializeField]
        public MappedDataRow row;
        [SerializeField]
        public DooTerrainDoodad doodad;
        [SerializeField]
        public string modePath;

        public TerrainDoodad(War3MapViewerMap map, object model, string modePath, MappedDataRow row, DooTerrainDoodad doodad)
        {
            //this.instance = model;
            this.row = row;
            this.doodad = doodad;
            this.modePath = modePath;
        }
    }
}
