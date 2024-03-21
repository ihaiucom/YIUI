
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zeng.mdx.commons;
using Zeng.mdx.parsers;
using Zeng.mdx.parsers.mpq;
using Zeng.mdx.parsers.w3x;
using Zeng.mdx.parsers.w3x.doo;
using Zeng.mdx.parsers.w3x.unitsdoo;
using Zeng.mdx.parsers.w3x.w3e;
using Zeng.mdx.parsers.w3x.w3i;
using Corner = Zeng.mdx.parsers.w3x.w3e.Corner;
using DooDoodad = Zeng.mdx.parsers.w3x.doo.Doodad;
using DooTerrainDoodad = Zeng.mdx.parsers.w3x.doo.TerrainDoodad;
using DooUnit = Zeng.mdx.parsers.w3x.unitsdoo.Unit;

namespace Zeng.mdx.handlers.w3x
{
    public class SolverParams
    {
        public string tileset;
        public bool reforged;
        public bool hd;
    }

    [Serializable]
    public class War3MapViewerMap : ScriptableObject
    {
        public War3MapViewer viewer;
        public War3Map map;
        public uint buildVersion = 0;
        public SolverParams solverParams;
        [SerializeField]
        public List<List<Corner>> corners;
        public float[] centerOffset = new float[2];
        public int[] mapSize = new int[2];

        public int columns = 0;
        public int rows = 0;

        public List<Texture> tilesetTextures = new List<Texture>();
        public List<Texture> cliffTextures = new List<Texture>();
        public List<Texture> waterTextures = new List<Texture>();

        // 地面
        public List<MappedDataRow> tilesets = new List<MappedDataRow>();
        public int blightTextureIndex = 0;


        // 悬崖
        [SerializeField]
        public List<MappedDataRow> cliffTilesets = new List<MappedDataRow>();
        [SerializeField]
        public List<TerrainModel> cliffModels = new List<TerrainModel>();
        [SerializeField]
        public List<CliffItem> cliffs = new List<CliffItem>();


        // 水
        public float waterHeightOffset = 0;
        public float waterIncreasePerFrame = 0;
        public float[] maxDeepColor = new float[4];
        public float[] minDeepColor = new float[4];
        public float[] maxShallowColor = new float[4];
        public float[] minShallowColor = new float[4];

        public bool anyReady = false;
        public bool terrainReady = false;
        public bool cliffsReady = false;


        // 装饰物
        [SerializeField]
        public List<Doodad> doodads = new List<Doodad>();
        [SerializeField]
        public List<TerrainDoodad> terrainDoodads = new List<TerrainDoodad>();
        public bool doodadsReady = false;

        // 单位
        [SerializeField]
        public List<Unit> units = new List<Unit>();
        public bool unitsReady = false;

        public int instanceCount = 0;
        public float[] cliffHeights; // = new float[columns * rows]; // cliffHeightMap；u_heightMap；[cliffs.vert.ts]  
        public float[] cornerHeights; // = new float[columns * rows]; // heightMap；u_heightMap；[ground.vert.ts, water.vert.ts ]
        public float[] waterHeights; // = new float[columns * rows]; // waterHeightMap； u_waterHeightMap；[water.vert.ts]
        public byte[] cornerTextures; // = new byte[instanceCount * 4]; // a_textures
        public byte[] cornerVariations; // = new byte[instanceCount * 4]; // a_variations
        public byte[] waterFlags; // = new byte[instanceCount]; // a_isWater


        public War3MapViewerMap(War3MapViewer viewer = null)
        {
            this.viewer = viewer;
        }


        public IEnumerator load(string path)
        {

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Position = 0;
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            ms.Dispose();
            stream.Close();

            yield return load(bytes);
        }

        public IEnumerator load(byte[] buffer)
        {

            this.map = new War3Map();
            solverParams = new SolverParams();
            solverParams.tileset = "a";

            // Readonly mode to optimize memory usage.
            this.map.load(buffer, true);

            this.loadMapInformation();

            yield return this.loadTerrainCliffsAndWater();
            yield return this.loadDoodadsAndDestructibles();
            yield return this.loadUnitsAndItems();
        }

        public object pathSolver(object src, Dictionary<string, object> _params)
        {
            if (src is string)
            {
                string path = (string)src;
                path = path.Replace("/", "\\");
                MpqFile file = this.map.get(path);
                if (file != null)
                {
                    return file.arrayBuffer();
                }
            }

            return src;
        }


        private void loadMapInformation()
        {

            MpqFile mpqFile = this.map.get("war3map.w3i");

            if (mpqFile == null)
            {
                UnityEngine.Debug.LogWarning("Attempted to load war3map.w3i but it is not there. Using default tileset A.");
                return;
            }

            War3MapW3i parser = new War3MapW3i();

            try
            {
                parser.load(mpqFile.bytes());
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning("Failed to correctly parse the map information file");
            }

            this.solverParams.tileset = parser.tileset.ToLower();

            this.buildVersion = parser.getBuildVersion();

            if (this.buildVersion > 131)
            {
                this.solverParams.reforged = true;
            }

        }


        private IEnumerator loadTerrainCliffsAndWater()
        {
            MpqFile mpqFile = this.map.get("war3map.w3e");

            if (mpqFile == null)
            {
                Debug.LogWarning("Attempted to load war3map.w3e, but it is not there.");
                yield break;
            }

            War3MapW3e parser = new War3MapW3e();

            //try
            {
                parser.load(mpqFile.bytes());
            }
            //catch (Exception e)
            //{
            //    Debug.LogWarning($"Failed to load war3map.w3e: { e.ToString()}");
            //    return;
            //}


            float[] centerOffset = parser.centerOffset;
            int[] mapSize = parser.mapSize;

            this.corners = parser.corners;
            this.centerOffset[0] = centerOffset[0];
            this.centerOffset[1] = centerOffset[1];

            this.mapSize[0] = mapSize[0];
            this.mapSize[1] = mapSize[1];


            // Override the grid based on the map.
            //this.worldScene.grid = new Grid(centerOffset[0], centerOffset[1], mapSize[0] * 128 - 128, mapSize[1] * 128 - 128, 16 * 128, 16 * 128);


            string texturesExt = this.solverParams.reforged ? ".dds" : ".blp";
            char tileset = parser.tileset;

            // 地面 贴图
            for (int i = 0, l = parser.groundTilesets.Length; i < l; i++)
            {
                string groundTileset = parser.groundTilesets[i];
                MappedDataRow row = viewer.terrainData.GetRow(groundTileset);
                this.tilesets.Add(row);

                string dir = row.GetString("dir");
                string file = row.GetString("file");
                string path = $"{dir}\\{file}{texturesExt}";

                yield return this.loadTexture(path, tilesetTextures);
            }


            Dictionary<char, string> blights = new Dictionary<char, string>()
            {
                {'A', "Ashen"},
                {'B', "Barrens"},
                {'C', "Felwood"},
                {'D', "Cave"},
                {'F', "Lordf"},
                {'G', "Dungeon"},
                {'I', "Ice"},
                {'J', "DRuins"},
                {'K', "Citadel"},
                {'L', "Lords"},
                {'N', "North"},
                {'O', "Outland"},
                {'Q', "VillageFall"},
                {'V', "Village"},
                {'W', "Lordw"},
                {'X', "Village"},
                {'Y', "Village"},
                {'Z', "Ruins"}
            };

            this.blightTextureIndex = this.tilesetTextures.Count;
            yield return loadTexture($"TerrainArt\\Blight\\{blights[tileset]}_Blight{texturesExt}", tilesetTextures);

            foreach (var cliffTileset in parser.cliffTilesets)
            {
                MappedDataRow row = viewer.cliffTypesData.GetRow(cliffTileset);

                this.cliffTilesets.Add(row);

                string path = $"{row.GetString("texDir")}\\{row.GetString("texFile")}{texturesExt}";

                UnityEngine.Debug.Log(path);
                yield return loadTexture(path, cliffTextures);
            }


            // water 贴图
            MappedDataRow waterRow = viewer.waterData.GetRow($"{tileset}Sha");
            this.waterHeightOffset = waterRow.GetNumber("height");
            this.waterIncreasePerFrame = 1f * waterRow.GetNumber("texRate") / 60;
            this.waterTextures.Clear();
            this.maxDeepColor = new float[] { waterRow.GetNumber("Dmax_R"), waterRow.GetNumber("Dmax_G"), waterRow.GetNumber("Dmax_B"), waterRow.GetNumber("Dmax_A") };
            this.minDeepColor = new float[] { waterRow.GetNumber("Dmin_R"), waterRow.GetNumber("Dmin_G"), waterRow.GetNumber("Dmin_B"), waterRow.GetNumber("Dmin_A") };
            this.maxShallowColor = new float[] { waterRow.GetNumber("Smax_R"), waterRow.GetNumber("Smax_G"), waterRow.GetNumber("Smax_B"), waterRow.GetNumber("Smax_A") };
            this.minShallowColor = new float[] { waterRow.GetNumber("Smin_R"), waterRow.GetNumber("Smin_G"), waterRow.GetNumber("Smin_B"), waterRow.GetNumber("Smin_A") };

            for (int i = 0, l = waterRow.GetNumber("numTex"); i < l; i++)
            {

                string texFile = waterRow.GetString("texFile");
                string numStr = i < 10 ? "0" : "";
                string file = $"{numStr}{i}";
                string path = $"{texFile}{file}{texturesExt}";

                yield return loadTexture(path, waterTextures);
            }


            List<List<Corner>> corners = parser.corners;
            int columns = this.mapSize[0];
            int rows = this.mapSize[1];
            int instanceCount = (columns - 1) * (rows - 1);
            this.instanceCount = instanceCount;
            float[] cliffHeights = new float[columns * rows]; // cliffHeightMap；u_heightMap；[cliffs.vert.ts]  
            float[] cornerHeights = new float[columns * rows]; // heightMap；u_heightMap；[ground.vert.ts, water.vert.ts ]
            float[] waterHeights = new float[columns * rows]; // waterHeightMap； u_waterHeightMap；[water.vert.ts]
            byte[] cornerTextures = new byte[instanceCount * 4]; // a_textures
            byte[] cornerVariations = new byte[instanceCount * 4]; // a_variations
            byte[] waterFlags = new byte[instanceCount]; // a_isWater
            int instance = 0;
            Dictionary<string, CliffItem> cliffs = new Dictionary<string, CliffItem>();

            this.cliffHeights = cliffHeights;
            this.cornerHeights = cornerHeights;
            this.waterHeights = waterHeights;
            this.cornerTextures = cornerTextures;
            this.cornerVariations = cornerVariations;
            this.waterFlags = waterFlags;


            this.columns = columns - 1;
            this.rows = rows - 1;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Corner bottomLeft = corners[y][x];
                    int index = y * columns + x;

                    // 悬崖
                    cliffHeights[index] = bottomLeft.groundHeight;
                    // 拐角
                    cornerHeights[index] = bottomLeft.groundHeight + bottomLeft.layerHeight - 2;
                    // 水
                    waterHeights[index] = bottomLeft.waterHeight;

                    if (y < rows - 1 && x < columns - 1)
                    {
                        // 水可以用于悬崖和正常的角落，所以不管怎样都要储存水的状态。
                        // Water can be used with cliffs and normal corners, so store water state regardless.
                        waterFlags[instance] = (byte)(this.isWater(x, y) ? 1 : 0);


                        // 这是悬崖，还是一个正常的角落？
                        // Is this a cliff, or a normal corner?
                        if (this.isCliff(x, y))
                        {

                            int bottomLeftLayer = bottomLeft.layerHeight;
                            int bottomRightLayer = corners[y][x + 1].layerHeight;
                            int topLeftLayer = corners[y + 1][x].layerHeight;
                            int topRightLayer = corners[y + 1][x + 1].layerHeight;
                            int minLayer = Mathf.Min(bottomLeftLayer, bottomRightLayer, topLeftLayer, topRightLayer);
                            string fileName = this.cliffFileName(bottomLeftLayer, bottomRightLayer, topLeftLayer, topRightLayer, minLayer);
                            //Debug.Log("[cliffFileName] " + fileName);

                            if (fileName != "AAAA")
                            {
                                int cliffTexture = bottomLeft.cliffTexture;

                                /// ?
                                if (cliffTexture == 15)
                                {
                                    cliffTexture = 1;
                                }

                                var cliffRow = this.cliffTilesets[cliffTexture];
                                string dir = (string)cliffRow.GetString("cliffModelDir");
                                string path = $"Doodads\\Terrain\\{dir}\\{dir}{fileName}{CliffVariation.GetCliffVariation(dir, fileName, bottomLeft.cliffVariation)}.mdx";

                                if (!cliffs.ContainsKey(path))
                                {
                                    cliffs[path] = new CliffItem();
                                    cliffs[path].path = path;

                                    //Debug.Log("[cliff path] " + path);
                                }
                                cliffs[path].locations.Add((x + 1) * 128 + centerOffset[0]);
                                cliffs[path].locations.Add(y * 128 + centerOffset[1]);
                                cliffs[path].locations.Add((minLayer - 2) * 128);
                                cliffs[path].textures.Add(cliffTexture);
                            }

                        }
                        else
                        {
                            byte bottomLeftTexture = (byte)cornerTexture(x, y);
                            byte bottomRightTexture = (byte)cornerTexture(x + 1, y);
                            byte topLeftTexture = (byte)cornerTexture(x, y + 1);
                            byte topRightTexture = (byte)cornerTexture(x + 1, y + 1);
                            List<byte> textures = new List<byte> { bottomLeftTexture, bottomRightTexture, topLeftTexture, topRightTexture };
                            textures = textures.Distinct().OrderBy(t => t).ToList();
                            byte texture = textures[0];

                            cornerTextures[instance * 4] = (byte)(texture + 1);
                            cornerVariations[instance * 4] = (byte)getVariation(texture, bottomLeft.groundVariation);

                            textures.RemoveAt(0);

                            for (int i = 0; i < textures.Count; i++)
                            {
                                byte bitset = 0;

                                texture = textures[i];

                                if (bottomRightTexture == texture)
                                {
                                    bitset |= 0b0001;
                                }

                                if (bottomLeftTexture == texture)
                                {
                                    bitset |= 0b0010;
                                }

                                if (topRightTexture == texture)
                                {
                                    bitset |= 0b0100;
                                }

                                if (topLeftTexture == texture)
                                {
                                    bitset |= 0b1000;
                                }

                                cornerTextures[instance * 4 + 1 + i] = (byte)(texture + 1);
                                cornerVariations[instance * 4 + 1 + i] = bitset;
                            }

                        }

                        instance += 1;
                    }
                }
            }


            this.terrainReady = true;
            this.anyReady = true;

            this.cliffs = new List<CliffItem>();
            foreach (var kv in cliffs) {
                string path = kv.Key;
                CliffItem cliff = kv.Value;
                this.cliffs.Add(cliff);
                yield return loadMdxCliff(path, cliff);
            }


        }

        private IEnumerator loadDoodadsAndDestructibles()
        {
            MpqFile mpqFile = this.map.get("war3map.doo");

            if (mpqFile == null)
            {
                Debug.Log("Attempted to load war3map.doo but it is not there");
                yield break;
            }


            War3MapDoo parser = new War3MapDoo();

            try
            {
                parser.Load(mpqFile.bytes(), (int)this.buildVersion);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load war3map.doo: " + e);
                yield break;
            }

            // 物品和可破坏物。
            // Doodads and destructibles.
            foreach (var doodad in parser.doodads)
            {
                var row = this.viewer.doodadsData.GetRow(doodad.id);

                if (row != null)
                {
                    string file = row.GetString("file");

                    if (!string.IsNullOrEmpty(file))
                    {
                        int numVar = row.GetNumber("numVar");

                        if (file.EndsWith(".mdl"))
                        {
                            file = file.Substring(0, file.Length - 4);
                        }

                        string fileVar = file;

                        file += ".mdx";

                        if (numVar > 1)
                        {
                            fileVar += Math.Min(doodad.variation, numVar - 1);
                        }

                        fileVar += ".mdx";


                        // First see if the model is local.
                        // Doodads referring to local models may have invalid variations, so if the variation doesn't exist, try without a variation.
                        MpqFile doodadMpqFile = this.map.get(fileVar) ?? this.map.get(file);
                           

                        if (doodadMpqFile != null)
                        {
                            //Debug.Log("loadDoodadsAndDestructibles: " + doodadMpqFile.name);
                            yield return loadMdxDoodad(doodadMpqFile.name, doodad, row) ;
                        }
                        else
                        {
                            //Debug.Log("loadDoodadsAndDestructibles: " + fileVar);
                            yield return loadMdxDoodad(fileVar, doodad, row);
                        }
                    }
                    else
                    {
                        Debug.Log("Unknown doodad ID " + doodad.id + " " + doodad);
                    }
                }
            }




            // 悬崖/地形装饰物。
            // Cliff/Terrain doodads.
            foreach (var doodad in parser.terrainDoodads)
            {
                var row = (MappedDataRow)this.viewer.doodadsData.GetRow(doodad.id);
                string path = $"{row.GetString("file")}.mdx";

                yield return loadMdxTerrainDoodad(path, doodad, row);


                // var pathTexture = (Texture)this.load(row.pathTex);

                // pathTexture.whenLoaded(() =>
                // {
                //     var startx = doodad.location[0];
                //     var starty = doodad.location[1];
                //     var endx = startx + pathTexture.width / 4;
                //     var endy = starty + pathTexture.height / 4;

                //     for (var x = startx; x < endx; x++)
                //     {
                //         for (var y = starty; y < endy; y++)
                //         {

                //         }
                //     }
                // });
            }



            this.doodadsReady = true;
            this.anyReady = true;

        }
        private IEnumerator loadUnitsAndItems()
        {
            var mpqFile = this.map.get("war3mapUnits.doo");

            if (mpqFile == null)
            {
                Console.WriteLine("Attempted to load war3mapUnits.doo but it is not there");
                yield break;
            }

            var parser = new War3MapUnitsDoo();

            //try
            {
                parser.Load(mpqFile.bytes(), (int)this.buildVersion);
            }
            //catch (Exception e)
            //{
            //    Debug.LogError($"Failed to load war3mapUnits.doo: {e}");
            //    yield break;
            //}

            // Collect the units and items data.
            foreach (var unit in parser.units)
            {
                MappedDataRow row = null;
                string path = null;

                // Hardcoded?
                if (unit.id == "sloc")
                {
                    path = "Objects\\StartLocation\\StartLocation.mdx";
                }
                else
                {
                    row = this.viewer.unitsData.GetRow(unit.id);

                    if (row != null)
                    {
                        path = row.GetString("file");

                        if (path != null)
                        {
                            if (path.EndsWith(".mdl"))
                            {
                                path = path.Substring(0, path.Length - 4);
                            }

                            path += ".mdx";
                        }
                    }
                }

                if (path != null)
                {
                    Debug.Log("loadUnitsAndItems: " + path);

                    yield return loadMdxUnit(path, unit, row);
                }
                else
                {
                    Debug.LogError("Unknown unit ID " + unit.id + " " + unit);
                }
            }

            this.unitsReady = true;
            this.anyReady = true;

            yield return null;
        }

        private IEnumerator loadMdxUnit(string path, DooUnit dooUnit, MappedDataRow row)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            //UnityEngine.Debug.Log(url);

            if (File.Exists(savePath))
            {
                var d = new Unit(this, null, path, row, dooUnit);
                this.units.Add(d);
                yield break;
            }

            IEnumerator it = MdxUnityMdxManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is byte[])
            {
                byte[] mdx = (byte[])it.Current;
                var d = new Unit(this, mdx, path, row, dooUnit);
                this.units.Add(d);
                yield return mdx;
            }

            yield return null;
        }


        private IEnumerator loadMdxTerrainDoodad(string path, DooTerrainDoodad dooDoodad, MappedDataRow row)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            //UnityEngine.Debug.Log(url);

            if (File.Exists(savePath))
            {
                var d = new TerrainDoodad(this, null, path, row, dooDoodad);
                this.terrainDoodads.Add(d);
                yield break;
            }

            IEnumerator it = MdxUnityMdxManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is byte[])
            {
                byte[] mdx = (byte[])it.Current;
                var d = new TerrainDoodad(this, mdx, path, row, dooDoodad);
                this.terrainDoodads.Add(d);
                yield return mdx;
            }

            yield return null;
        }

        private IEnumerator loadMdxDoodad(string path, DooDoodad dooDoodad, MappedDataRow row)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            //UnityEngine.Debug.Log(url);

            if (File.Exists(savePath)) {
                var d = new Doodad(this, null, path, row, dooDoodad);
                this.doodads.Add(d);
                yield break;
            }

            IEnumerator it = MdxUnityMdxManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is byte[])
            {
                byte[] mdx = (byte[])it.Current;
                var d = new Doodad(this, mdx, path, row, dooDoodad);
                this.doodads.Add(d);
                yield return mdx;
            }

            yield return null;
        }

        private IEnumerator loadMdxCliff(string path, CliffItem cliff)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            //UnityEngine.Debug.Log("loadMdxCliff:" + url);

            if (File.Exists(savePath)) {
                var m = new TerrainModel(this, cliff, path, null);
                this.cliffModels.Add(m);
                yield break;
            }

            IEnumerator it = MdxUnityMdxManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is byte[])
            {
                byte[] mdx = (byte[])it.Current;
                var m = new TerrainModel(this, cliff, path, mdx);
                //var m = new TerrainModel(this, cliff, path, null);
                this.cliffModels.Add(m);
                yield return mdx;
            }

            yield return null;
        }

        private IEnumerator loadTexture(string path, List<Texture> list = null)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            //UnityEngine.Debug.Log(url);
            IEnumerator it = MdxUnityTextureManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is Texture)
            {
                Texture texture = (Texture)it.Current;
                if (list != null)
                {
                    list.Add(texture);
                }
                yield return texture;
            }

            yield return null;

        }

        string cliffFileName(int bottomLeftLayer, int bottomRightLayer, int topLeftLayer, int topRightLayer, int min)
        {
            return Convert.ToChar(65 + bottomLeftLayer - min).ToString() +
              Convert.ToChar(65 + topLeftLayer - min).ToString() +
              Convert.ToChar(65 + topRightLayer - min).ToString() +
              Convert.ToChar(65 + bottomRightLayer - min).ToString();
        }


        /**
         * 给定的列和行的拐角是悬崖吗？
         */
        private bool isCliff(int column, int row)
        {
            if (column < 1 || column > this.columns - 1 || row < 1 || row > this.rows - 1)
            {
                return false;
            }

            var bottomLeft = corners[row][column].layerHeight;
            var bottomRight = corners[row][column + 1].layerHeight; // 右
            var topLeft = corners[row + 1][column].layerHeight; // 上
            var topRight = corners[row + 1][column + 1].layerHeight; // 右上

            return bottomLeft != bottomRight || bottomLeft != topLeft || bottomLeft != topRight;
        }


        /**
         * 给定列和行的瓷砖是水吗？
         * Is the tile at the given column and row water?
         */
        private bool isWater(int column, int row)  {

            return corners[row][column].water != 0 
                || corners[row][column + 1].water != 0  // 右
                || corners[row + 1][column].water != 0  // 上
                || corners[row + 1][column + 1].water != 0; // 右上
        }

        /**
         * 获取一个角落的地面纹理，无论是正常的地面、悬崖还是破败的角落。
         * Get the ground texture of a corner, whether it's normal ground, a cliff, or a blighted corner.
         */
        private int cornerTexture(int column, int row)
        {
            var corners = this.corners;
            var columns = this.columns;
            var rows = this.rows;

            for (int y = -1; y < 1; y++)
            {
                for (int x = -1; x < 1; x++)
                {
                    if (column + x > 0 && column + x < columns - 1 && row + y > 0 && row + y < rows - 1)
                    {
                        if (this.isCliff(column + x, row + y))
                        {
                            int texture = corners[row + y][column + x].cliffTexture;

                            if (texture == 15)
                            {
                                texture = 1;
                            }

                            return this.cliffGroundIndex(texture);
                        }
                    }
                }
            }

            var corner = corners[row][column];

            // 这个角落被破坏了吗？
            // Is this corner blighted?
            if (corner.blight != 0)
            {
                return this.blightTextureIndex;
            }

            return corner.groundTexture;
        }


        /**
         * 给定一个悬崖索引，获取其地面纹理索引。
         * 这是一个指向图块纹理的索引。
         * 
         */
        private int cliffGroundIndex(int whichCliff)
        {
            string whichTileset = this.cliffTilesets[whichCliff].GetString("groundTile");
            var tilesets = this.tilesets;

            for (int i = 0, l = tilesets.Count; i < l; i++)
            {
                if (tilesets[i].GetString("tileID") == whichTileset)
                {
                    return i;
                }
            }

            return 0;
        }

        private int getVariation(int groundTexture, int variation)
        {
            Texture texture = this.tilesetTextures[groundTexture];

            // Extended?
            if (texture.width > texture.height)
            {
                if (variation < 16)
                {
                    return 16 + variation;
                }
                else if (variation == 16)
                {
                    return 15;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (variation == 0)
                {
                    return 0;
                }
                else
                {
                    return 15;
                }
            }
        }


    }
}
