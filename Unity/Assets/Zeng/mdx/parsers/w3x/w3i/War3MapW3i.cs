using MdxLib.ModelFormats.Mdx;
using System.IO;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class War3MapW3i
    {
        public int version = 0;
        public int saves = 0;
        public int editorVersion = 0;
        public uint[] buildVersion = new uint[4];
        public string name = "";
        public string author = "";
        public string description = "";
        public string recommendedPlayers = "";
        public float[] cameraBounds = new float[8];
        public int[] cameraBoundsComplements = new int[4];
        public int[] playableSize = new int[2];
        public uint flags = 0;
        public string tileset = "a";
        public int campaignBackground = 0;
        public string loadingScreenModel = "";
        public string loadingScreenText = "";
        public string loadingScreenTitle = "";
        public string loadingScreenSubtitle = "";
        public int loadingScreen = 0;
        public string prologueScreenModel = "";
        public string prologueScreenText = "";
        public string prologueScreenTitle = "";
        public string prologueScreenSubtitle = "";
        public int useTerrainFog = 0;
        public float[] fogHeight = new float[2];
        public float fogDensity = 0;
        public byte[] fogColor = new byte[4];
        public int globalWeather = 0;
        public string soundEnvironment = "";
        public string lightEnvironmentTileset = "\0";
        public byte[] waterVertexColor = new byte[4];
        public uint scriptMode = 0;
        public uint graphicsMode = 0;
        public Player[] players = new Player[] { };
        public Force[] forces = new Force[] { };
        public UpgradeAvailabilityChange[] upgradeAvailabilityChanges = new UpgradeAvailabilityChange[] { };
        public TechAvailabilityChange[] techAvailabilityChanges = new TechAvailabilityChange[] { };
        public RandomUnitTable[] randomUnitTables = new RandomUnitTable[] { };
        public RandomItemTable[] randomItemTables = new RandomItemTable[] { };
        public uint unknown1 = 0;


        public void load(byte[] buffer)
        {

            MemoryStream ms = new MemoryStream(buffer);
            ms.Position = 0;
            CLoader cloader = new CLoader(name, ms);


            this.version = cloader.ReadInt32();
            this.saves = cloader.ReadInt32();
            this.editorVersion = cloader.ReadInt32();

            if (this.version > 27)
            {
                cloader.readUint32Array(this.buildVersion);
            }

            this.name = cloader.ReadNull();
            this.author = cloader.ReadNull();
            this.description = cloader.ReadNull();
            this.recommendedPlayers = cloader.ReadNull();
            cloader.readFloat32Array(this.cameraBounds);
            cloader.readInt32Array(this.cameraBoundsComplements);
            cloader.readInt32Array(this.playableSize);
            this.flags = cloader.ReadUInt32();
            this.tileset = cloader.ReadString(1);
            this.campaignBackground = cloader.ReadInt32();

            if (this.version > 24)
            {
                this.loadingScreenModel = cloader.ReadNull();
            }

            this.loadingScreenText = cloader.ReadNull();
            this.loadingScreenTitle = cloader.ReadNull();
            this.loadingScreenSubtitle = cloader.ReadNull();
            this.loadingScreen = cloader.ReadInt32();

            if (this.version > 24)
            {
                this.prologueScreenModel = cloader.ReadNull();
            }

            this.prologueScreenText = cloader.ReadNull();
            this.prologueScreenTitle = cloader.ReadNull();
            this.prologueScreenSubtitle = cloader.ReadNull();

            if (this.version > 24)
            {
                this.useTerrainFog = cloader.ReadInt32();
                cloader.readFloat32Array(this.fogHeight);
                this.fogDensity = cloader.ReadFloat();
                cloader.readUint8Array(this.fogColor);
                this.globalWeather = cloader.ReadInt32();
                this.soundEnvironment = cloader.ReadNull();
                this.lightEnvironmentTileset = cloader.ReadString(1);
                cloader.readUint8Array(this.waterVertexColor);
            }

            if (this.version > 27)
            {
                this.scriptMode = cloader.ReadUInt32();
            }


            if (this.version > 30)
            {
                this.graphicsMode = cloader.ReadUInt32();
                this.unknown1 = cloader.ReadUInt32();
            }

            this.players = new Player[cloader.ReadInt32()];
            for (int i = 0, l = this.players.Length; i < l; i++)
            {
                Player player = new Player();

                player.load(cloader, this.version);

                this.players[i] = player;
            }

            this.forces = new Force[cloader.ReadInt32()];
            for (int i = 0, l = this.forces.Length; i < l; i++)
            {
                Force force = new Force();

                force.load(cloader);

                this.forces[i] = force;
            }

            this.upgradeAvailabilityChanges = new UpgradeAvailabilityChange[cloader.ReadInt32()];
            for (int i = 0, l = this.upgradeAvailabilityChanges.Length; i < l; i++)
            {
                UpgradeAvailabilityChange upgradeAvailabilityChange = new UpgradeAvailabilityChange();

                upgradeAvailabilityChange.load(cloader);

                this.upgradeAvailabilityChanges[i] = upgradeAvailabilityChange;
            }

            this.techAvailabilityChanges = new TechAvailabilityChange[cloader.ReadInt32()];
            for (int i = 0, l = this.techAvailabilityChanges.Length; i < l; i++)
            {
                TechAvailabilityChange techAvailabilityChange = new TechAvailabilityChange();

                techAvailabilityChange.load(cloader);

                this.techAvailabilityChanges[i] = techAvailabilityChange;
            }

            this.randomUnitTables = new RandomUnitTable[cloader.ReadInt32()];
            for (int i = 0, l = this.randomUnitTables.Length; i < l; i++)
            {
                RandomUnitTable randomUnitTable = new RandomUnitTable();

                randomUnitTable.load(cloader);

                this.randomUnitTables[i] = randomUnitTable;
            }

            if (this.version > 24)
            {
                this.randomItemTables = new RandomItemTable[cloader.ReadInt32()];
                for (int i = 0, l = this.randomItemTables.Length; i < l; i++)
                {
                    RandomItemTable randomItemTable = new RandomItemTable();

                    randomItemTable.load(cloader);

                    this.randomItemTables[i] = randomItemTable;
                }
            }





            ms.Dispose();
        }


        /**
         * Returns the build version as major+minor.
         * 
         * For example version 1.31.X will return 131.
         * 
         * Note that this will always return 0 for any version below 1.31.
         */
        public uint getBuildVersion() {
            return this.buildVersion[0] * 100 + this.buildVersion[1];
          }
    }
}
