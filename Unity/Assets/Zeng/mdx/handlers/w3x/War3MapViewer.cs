
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Windows;
using Zeng.mdx.commons;
using Zeng.mdx.parsers;
using Zeng.mdx.parsers.w3x.doo;

namespace Zeng.mdx.handlers.w3x
{
    public class War3MapViewer
    {
        public bool isReforged = true;
        public MappedData terrainData = new MappedData();
        public MappedData cliffTypesData = new MappedData();
        public MappedData waterData = new MappedData();
        public MappedData doodadsData = new MappedData();
        public MappedData doodadMetaData = new MappedData();
        public MappedData destructableMetaData = new MappedData();
        public MappedData unitsData = new MappedData();
        public MappedData unitMetaData = new MappedData();
        public MappedData abilityData = new MappedData();






        public bool loadedBaseFiles = false;
        public War3MapViewerMap map = null;

        public IEnumerator Init() { 
        
            yield return loadBaseFiles();
            //yield return loadSplatsSKFiles();
            //yield return loadAbilitySKFiles();
            //yield return loadOtherSKFiles();
            //yield return downloadAllMdxs();

        }


        public MappedData SpawnData = new MappedData();
        public MappedData SplatData = new MappedData();
        public MappedData UberSplatData = new MappedData();
        public MappedData AnimSounds = new MappedData();
        private IEnumerator loadSplatsSKFiles()
        {
            yield return this.loadBaseFile("Splats\\SpawnData.slk", SpawnData);
            yield return this.loadBaseFile("Splats\\SplatData.slk", SplatData);
            yield return this.loadBaseFile("Splats\\UberSplatData.slk", UberSplatData);
            yield return this.loadBaseFile("UI\\SoundInfo\\AnimSounds.slk", AnimSounds);
            //yield return this.loadBaseFile("UI\\SoundInfo\\AnimLookups.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueHumanBase.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueOrcBase.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueUndeadBase.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueNightElfBase.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueNagaBase.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueDemonBase.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogueCreepsBase.slk", AnimSounds);
        }

        public MappedData LightningData = new MappedData();

        private IEnumerator loadOtherSKFiles()
        {
            yield return this.loadBaseFile("Splats/LightningData.slk ", LightningData); 
            yield return this.loadBaseFile("TerrainArt/Weather.slk", LightningData);
        }

        private IEnumerator loadBaseFiles()
        {
            yield return this.loadBaseFile("TerrainArt\\Terrain.slk", terrainData);
            yield return this.loadBaseFile("TerrainArt\\CliffTypes.slk", cliffTypesData);
            yield return this.loadBaseFile("TerrainArt\\Water.slk", waterData);
            yield return this.loadBaseFile("Doodads\\Doodads.slk", doodadsData);
            yield return this.loadBaseFile("Doodads\\DoodadMetaData.slk", doodadMetaData);
            yield return this.loadBaseFile("Units\\DestructableData.slk", doodadsData);
            yield return this.loadBaseFile("Units\\DestructableMetaData.slk", destructableMetaData);
            yield return this.loadBaseFile("Units\\UnitData.slk", unitsData);
            yield return this.loadBaseFile("Units\\unitUI.slk", unitsData);
            yield return this.loadBaseFile("Units\\ItemData.slk", unitsData);
            yield return this.loadBaseFile("Units\\UnitMetaData.slk", unitMetaData);

            if (this.isReforged)
            {
                yield return this.loadBaseFile("Doodads\\doodadSkins.txt", doodadsData);
                yield return this.loadBaseFile("Units\\destructableSkin.txt", doodadsData);
                yield return this.loadBaseFile("Units\\unitSkin.txt", unitsData);
                yield return this.loadBaseFile("Units\\itemSkin.txt", unitsData);
            }

            this.loadedBaseFiles = true;
            UnityEngine.Debug.Log("loadBaseFiles 加载完成");
        }

        private IEnumerator loadAbilitySKFiles()
        {
            yield return this.loadBaseFile("Units/AbilityData.slk", abilityData);
            yield return this.loadBaseFile("Units/AbilitySkin.txt", abilityData);

            yield return this.loadBaseFile("Units/UnitAbilities.slk", unitsData);
            yield return this.loadBaseFile("Units/UnitBalance.slk", unitsData);
            yield return this.loadBaseFile("Units/UnitWeapons.slk", unitsData);
            yield return this.loadBaseFile("Units/UpgradeData.slk", unitsData);
            yield return this.loadBaseFile("UI\\SoundInfo\\AbilitySounds.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\AmbienceSounds.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\UnitAckSounds.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\UnitCombatSounds.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\DialogSounds.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\PortraitAnims.slk", AnimSounds);
            yield return this.loadBaseFile("UI\\SoundInfo\\EnvironmentSounds.slk", AnimSounds);
            //yield return this.loadBaseFile("UI\\SoundInfo\\MIDISounds.slk", AnimSounds);
        }

        private IEnumerator loadBaseFile(string path,  MappedData mappedData)
        {
            string savePath = MdxUnityResPathDefine.GetPath(path);
            if (!System.IO.File.Exists(savePath))
            {
                IEnumerator it = downloadSK(path);
                yield return it;

                if (it.Current == null)
                {
                    yield break;
                }
            }
            if (System.IO.File.Exists(savePath))
            {
                //UnityEngine.Debug.Log(savePath);
                string buffer = System.IO.File.ReadAllText(savePath);
                mappedData.Load(buffer);
            }
        }

        private IEnumerator downloadSK(string path)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path) ;
            yield return MdxUnityDownload.I.Load(url, savePath);
        }

        public IEnumerator loadMap(string path) {
            this.map = new War3MapViewerMap(this);
            yield return this.map.load(path);
        }

        public IEnumerator downloadAllMdxs()
        {
            //yield return downloadAllUnits();
            //yield return downloadAllDoodads();
            //yield return downloadAllTerrainDatas();
            //yield return downloadAllcCliffTypesData();
            //yield return downloadAllcWaterData();
            //yield return downloadAllSpawnData();
            //yield return downloadAllAbilityData();
            yield return downloadAllLightingDatas();

        }


        public IEnumerator downloadAllLightingDatas()
        {
            string texturesExt = ".blp";
            foreach (var kv in LightningData.Map)
            {
                var row = kv.Value;

                string dir = row.GetString("dir");
                string file = row.GetString("file");
                string path = $"{dir}\\{file}";
                yield return loadTexture(path);
            }
        }

        public IEnumerator downloadAllcWaterData()
        {
            string texturesExt = ".blp";
            foreach (var kv in waterData.Map)
            {
                var waterRow = kv.Value;

                for (int i = 0, l = waterRow.GetNumber("numTex"); i < l; i++)
                {

                    string texFile = waterRow.GetString("texFile");
                    string numStr = i < 10 ? "0" : "";
                    string file = $"{numStr}{i}";
                    string path = $"{texFile}{file}{texturesExt}";

                    yield return loadTexture(path);
                }

            }
        }

        public IEnumerator downloadAllcCliffTypesData()
        {
            string texturesExt = ".blp";
            foreach (var kv in cliffTypesData.Map)
            {
                var row = kv.Value;

                string path = $"{row.GetString("texDir")}\\{row.GetString("texFile")}{texturesExt}";

                yield return loadTexture(path);
            }
        }

        public IEnumerator downloadAllTerrainDatas()
        {
            string texturesExt =  ".blp";
            foreach (var kv in terrainData.Map)
            {
                var row = kv.Value;

                string dir = row.GetString("dir");
                string file = row.GetString("file");
                string path = $"{dir}\\{file}{texturesExt}";
                yield return loadTexture(path);
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

            foreach (var kv in blights)
            {
                yield return loadTexture($"TerrainArt\\Blight\\{kv.Value}_Blight{texturesExt}");
            }



        }

        public IEnumerator downloadAllUnits()
        {
            foreach (var kv in unitsData.Map)
            {
                var row = kv.Value;
                string path = row.GetString("file");

                if (path != null)
                {
                    if (path.EndsWith(".mdl"))
                    {
                        path = path.Substring(0, path.Length - 4);
                    }

                    path += ".mdx";

                    yield return downloadMdx(path);
                }
            }
        }


        public IEnumerator downloadAllDoodads()
        {
            foreach (var kv in doodadsData.Map)
            {
                var row = kv.Value;
                string path = row.GetString("file");
                if (!string.IsNullOrEmpty(path))
                {

                    if (path.EndsWith(".mdl") || path.EndsWith(".mdx"))
                    {
                        path = path.Substring(0, path.Length - 4);
                    }


                    int numVar = row.GetNumber("numVar");
                    if (numVar > 1)
                    {
                        for (int i = 0; i < numVar; i ++) {

                            string fileVar = path;
                            fileVar += i;
                            fileVar += ".mdx";
                            yield return downloadMdx(fileVar);
                        }
                    }
                    else {
                        path += ".mdx";
                        yield return downloadMdx(path);
                    }




                }
            }
        }

        public IEnumerator downloadAllAbilityData()
        {

            foreach (var kv in abilityData.Map)
            {
                var row = kv.Value;
                string path = row.GetString("Art");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return loadTexture(path);
                }

                path = row.GetString("Researchart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return loadTexture(path);
                }

                path = row.GetString("Unart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return loadTexture(path);
                }

                path = row.GetString("Buffart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return loadTexture(path);
                }

                path = row.GetString("Specialart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return downloadMdx(path.Replace(".mdl", ".mdx"));
                }

                path = row.GetString("Missileart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return downloadMdx(path.Replace(".mdl", ".mdx"));
                }

                path = row.GetString("Effectart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return downloadMdx(path.Replace(".mdl", ".mdx"));
                }

                path = row.GetString("Targetart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return downloadMdx(path.Replace(".mdl", ".mdx"));
                }

                path = row.GetString("Casterart");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return downloadMdx(path.Replace(".mdl", ".mdx"));
                }
            }
        }

        public IEnumerator downloadAllSpawnData()
        {
            foreach (var kv in SpawnData.Map)
            {
                var row = kv.Value;
                string path = row.GetString("Model").Replace(".mdl", ".mdx");
                if (!string.IsNullOrEmpty(path))
                {
                    yield return downloadMdx(path);
                }
            }


            string texturesExt = ".blp";
            foreach (var kv in SplatData.Map)
            {
                var row = kv.Value;
                string path = $"ReplaceableTextures\\Splats\\{row.GetString("file")}{texturesExt}";

                yield return loadTexture(path);
            }

            foreach (var kv in UberSplatData.Map)
            {
                var row = kv.Value;
                string path = $"ReplaceableTextures\\Splats\\{row.GetString("file")}{texturesExt}";

                yield return loadTexture(path);
            }



            foreach (var kv in AnimSounds.Map)
            {
                var row = kv.Value;
                string FileNames = row.GetString("FileNames");
                if (!string.IsNullOrEmpty(FileNames)) {
                    foreach (var fileName in FileNames.Split(','))
                    {
                        if (Path.GetExtension(fileName) == ".flac")
                        {
                            yield return downloadSound(fileName);
                        }
                    }
                }
            }
        }


        private IEnumerator downloadMdx(string path, bool isReload = false)
        {
            if (path.IndexOf(",") != -1) {
                string[] arr = path.Split(",");
                foreach (var itemPath in arr)
                {
                    yield return downloadMdx(itemPath, isReload);
                }
                yield break;
            }

            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            if (System.IO.File.Exists(savePath)) {
                yield break;
            }
            UnityEngine.Debug.Log(url);

            IEnumerator it = MdxUnityMdxManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is byte[])
            {
                byte[] mdx = (byte[])it.Current;
                yield return mdx;
            }
            else
            {
                if (!isReload)
                {
                    string path2 = path.Replace("0.mdx", ".mdx");
                    if (path2 != path) {
                        yield return downloadMdx(path2, true);
                    }
                }
            }

            yield return null;
        }

        private IEnumerator loadTexture(string path)
        {

            if (path.IndexOf(",") != -1)
            {
                string[] arr = path.Split(",");
                foreach (var itemPath in arr)
                {
                    yield return loadTexture(itemPath);
                }
                yield break;
            }

            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);

            UnityEngine.Debug.Log(savePath);
            if (System.IO.File.Exists(savePath))
            {
                yield break;
            }

            IEnumerator it = MdxUnityTextureManager.I.Load(url, savePath, path);
            yield return it;

            if (it.Current != null && it.Current is Texture)
            {
                Texture texture = (Texture)it.Current;
                yield return texture;
            }

            yield return null;

        }


        private IEnumerator downloadSound(string path)
        {
            string url = UrlUtils.localOrHive(path);
            string savePath = MdxUnityResPathDefine.GetPath(path);
            if (System.IO.File.Exists(savePath))
            {
                yield break;
            }
            UnityEngine.Debug.Log(url);

            IEnumerator it = MdxUnityMdxManager.I.Load(url, savePath, path);
            yield return it;
        }
    }
}
