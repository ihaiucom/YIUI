
using System.Collections.Generic;
using UnityEngine;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A texture.
     */
    public class Texture : IMdxStaticObject
    {
        public enum WrapMode
        {
            RepeatBoth = 0,
            WrapWidth = 1,
            WrapHeight = 2,
            WrapBoth = 3,
        }

        public int replaceableId = 0;
        public string path = "";
        public WrapMode wrapMode = WrapMode.RepeatBoth;



        public static Dictionary<int, string> replaceableIds = new Dictionary<int, string> {

            {1, "TeamColor/TeamColor00" },
            {2, "TeamGlow/TeamGlow00"},
            {11, "Cliff/Cliff0"},
            {21, ""}, // Used by all cursor models (HumanCursor, OrcCursor, UndeadCursor, NightElfCursor)
            {31, "LordaeronTree/LordaeronSummerTree"},
            {32, "AshenvaleTree/AshenTree"},
            {33, "BarrensTree/BarrensTree"},
            {34, "NorthrendTree/NorthTree"},
            {35, "Mushroom/MushroomTree"},
            {36, "RuinsTree/RuinsTree"},
            {37, "OutlandMushroomTree/MushroomTree"},

        };

        public static string GetTexturePath(string path, int replaceableId)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }
            else if (replaceableId != 0 && replaceableIds.ContainsKey(replaceableId) && !string.IsNullOrEmpty(replaceableIds[replaceableId]))
            {
                string tpath = $"ReplaceableTextures/{replaceableIds[replaceableId]}.blp";
                return tpath;
            }
            return null;
        }

        public string TexturePath {
            get {
                if (!string.IsNullOrEmpty(path))
                {
                    return path;
                }
                else if (replaceableId != 0 && replaceableIds.ContainsKey(replaceableId) && !string.IsNullOrEmpty(replaceableIds[replaceableId]))
                {
                    string tpath = $"ReplaceableTextures/{replaceableIds[replaceableId]}.blp";
                    return tpath;
                }
                return null;
            }
        }

        public void ReadMdx(BinaryStream stream)
        {
            this.replaceableId = (int)stream.ReadUint32();
            this.path = stream.Read(260);
            this.wrapMode = (WrapMode)stream.ReadUint32();
            UnityEngine.Debug.Log($"Texture: path={path}, replaceableId={replaceableId}");
        }

        public void WriteMdx(BinaryStream stream)
        {
            stream.WriteUint32((uint)this.replaceableId);
            stream.Skip(260 - stream.Write(this.path));
            stream.WriteUint32((uint)this.wrapMode);
        }
    }


}
