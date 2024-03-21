using System;
using Zeng.mdx.commons;
using Zeng.mdx.parsers.w3x.w3i;

namespace Zeng.mdx.parsers.w3x.unitsdoo
{
    /**
     * A unit.
     */
    [Serializable]
    public class Unit
    {
        public string id = "\0\0\0\0";
        public int variation = 0;
        public float[] location = new float[3];
        public float angle = 0;
        public float[] scale = new float[] { 1, 1, 1 };
        /**
         * @since Game version 1.32
         */
        public string skin = "\0\0\0\0";
        public int flags = 0;
        public int player = 0;
        public int unknown = 0;
        public int hitpoints = -1;
        public int mana = -1;
        /**
         * @since 8
         */
        public int droppedItemTable = 0;
        public DroppedItemSet[] droppedItemSets = new DroppedItemSet[0];
        public int goldAmount = 0;
        public int targetAcquisition = 0;
        public int heroLevel = 0;
        /**
         * @since 8
         */
        public int heroStrength = 0;
        /**
         * @since 8
         */
        public int heroAgility = 0;
        /**
         * @since 8
         */
        public int heroIntelligence = 0;
        public InventoryItem[] itemsInInventory = new InventoryItem[0];
        public ModifiedAbility[] modifiedAbilities = new ModifiedAbility[0];
        public int randomFlag = 0;
        public byte[] level = new byte[3];
        public int itemClass = 0;
        public int unitGroup = 0;
        public int positionInGroup = 0;
        public RandomUnit[] randomUnitTables = new RandomUnit[0];
        public int customTeamColor = 0;
        public int waygate = 0;
        public int creationNumber = 0;

        public void Load(BinaryStream stream, int version, int subversion, int buildVersion)
        {
            id = stream.ReadBinary(4);
            variation = stream.ReadInt32();
            stream.ReadFloat32Array(location);
            angle = stream.ReadFloat32();
            stream.ReadFloat32Array(scale);

            if (buildVersion > 131)
            {
                skin = stream.ReadBinary(4);
            }

            flags = stream.ReadUint8();
            player = stream.ReadInt32();
            unknown = stream.ReadUint16();
            hitpoints = stream.ReadInt32();
            mana = stream.ReadInt32();

            if (subversion > 10)
            {
                droppedItemTable = stream.ReadInt32();
            }

            int droppedItemSetsLength = stream.ReadInt32();
            droppedItemSets = new DroppedItemSet[droppedItemSetsLength];

            for (int i = 0; i < droppedItemSetsLength; i++)
            {
                DroppedItemSet set = new DroppedItemSet();
                set.Load(stream);
                droppedItemSets[i] = set;
            }

            goldAmount = stream.ReadInt32();
            targetAcquisition = stream.ReadInt32();
            heroLevel = stream.ReadInt32();

            if (subversion > 10)
            {
                heroStrength = stream.ReadInt32();
                heroAgility = stream.ReadInt32();
                heroIntelligence = stream.ReadInt32();
            }

            int itemsInInventoryLength = stream.ReadInt32();
            itemsInInventory = new InventoryItem[itemsInInventoryLength];

            for (int i = 0; i < itemsInInventoryLength; i++)
            {
                InventoryItem item = new InventoryItem();
                item.Load(stream);
                itemsInInventory[i] = item;
            }

            int modifiedAbilitiesLength = stream.ReadInt32();
            modifiedAbilities = new ModifiedAbility[modifiedAbilitiesLength];

            for (int i = 0; i < modifiedAbilitiesLength; i++)
            {
                ModifiedAbility modifiedAbility = new ModifiedAbility();
                modifiedAbility.Load(stream);
                modifiedAbilities[i] = modifiedAbility;
            }

            randomFlag = stream.ReadInt32();

            if (randomFlag == 0)
            {
                stream.ReadUint8Array(level); // 24bit number
                itemClass = stream.ReadUint8();
            }
            else if (randomFlag == 1)
            {
                unitGroup = (int)stream.ReadUint32();
                positionInGroup = (int)stream.ReadUint32();
            }
            else if (randomFlag == 2)
            {
                int randomUnitTablesLength = stream.ReadInt32();
                randomUnitTables = new RandomUnit[randomUnitTablesLength];

                for (int i = 0; i < randomUnitTablesLength; i++)
                {
                    RandomUnit randomUnit = new RandomUnit();
                    randomUnit.Load(stream);
                    this.randomUnitTables[i] = randomUnit;
                }
            }


            this.customTeamColor = stream.ReadInt32();
            this.waygate = stream.ReadInt32();
            this.creationNumber = stream.ReadInt32();
        }
    }
}
