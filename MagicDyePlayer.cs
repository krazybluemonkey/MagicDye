using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Default;
using Terraria.Localization;
using Terraria.ID;

namespace MagicDye
{
    public class MagicDyePlayer : ModPlayer
    {
        public int MagicDyeVersion;
        public Vector3[] MagicDyePrimaryColors;
        public Vector3[] MagicDyeSecondaryColors;
        public float[] MagicDyeSaturation;
        public float[] MagicDyeOpacity;
        public string[] MagicDyePasses;
        public string[] MagicDyeColorMod1;
        public string[] MagicDyeColorMod2;
        public Item[] MagicDyeItem;

        public override void Initialize()
        {
            MagicDyeVersion = 1;
            MagicDyePrimaryColors = new Vector3[MagicDye.dyeLookUp.Length];
            MagicDyeSecondaryColors = new Vector3[MagicDye.dyeLookUp.Length];
            MagicDyeSaturation = new float[MagicDye.dyeLookUp.Length];
            MagicDyeOpacity = new float[MagicDye.dyeLookUp.Length];
            MagicDyePasses = new string[MagicDye.dyeLookUp.Length];
            MagicDyeColorMod1 = new string[MagicDye.dyeLookUp.Length];
            MagicDyeColorMod2 = new string[MagicDye.dyeLookUp.Length];
            MagicDyeItem = new Item[MagicDye.dyeLookUp.Length];
            for (int i = 0; i < MagicDye.dyeLookUp.Length; i++)
            {
                MagicDyePrimaryColors[i] = new Vector3(1f, 1f, 1f);
                MagicDyeSecondaryColors[i] = new Vector3(1f, 1f, 1f);
                MagicDyeSaturation[i] = 1f;
                MagicDyeOpacity[i] = 1f;
                MagicDyePasses[i] = "ArmorColored";
                MagicDyeColorMod1[i] = "None";
                MagicDyeColorMod2[i] = "None";
                MagicDyeItem[i] = new Item();
                MagicDyeItem[i].SetDefaults(ItemID.None, true);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("MagicDyeVersion", 1);
            tag.Add("MagicDyePrimaryColors", MagicDyePrimaryColors);
            tag.Add("MagicDyeSecondaryColors", MagicDyeSecondaryColors);
            tag.Add("MagicDyeSaturation", MagicDyeSaturation);
            tag.Add("MagicDyeOpacity", MagicDyeOpacity);
            tag.Add("MagicDyePasses", MagicDyePasses);
            tag.Add("MagicDyeColorMod1", MagicDyeColorMod1);
            tag.Add("MagicDyeColorMod2", MagicDyeColorMod2);
            tag.Add("MagicDyeItem", MagicDyeItem.Select(ItemIO.Save).ToList());
        }

        public override void LoadData(TagCompound tag)
        {
            MagicDyeVersion = tag.Get<int>("MagicDyeVersion");
            MagicDyePrimaryColors = tag.Get<Vector3[]>("MagicDyePrimaryColors");
            MagicDyeSecondaryColors = tag.Get<Vector3[]>("MagicDyeSecondaryColors");
            MagicDyeSaturation = tag.Get<float[]>("MagicDyeSaturation");
            MagicDyeOpacity = tag.Get<float[]>("MagicDyeOpacity");
            MagicDyePasses = tag.Get<string[]>("MagicDyePasses");
            MagicDyeColorMod1 = tag.Get<string[]>("MagicDyeColorMod1");
            MagicDyeColorMod2 = tag.Get<string[]>("MagicDyeColorMod2");
            tag.GetList<TagCompound>("MagicDyeItem").Select(ItemIO.Load).ToList().CopyTo(MagicDyeItem);
            for (int i = 0; i < MagicDyeItem.Length; i++)
            {
                if (MagicDyeItem[i].ModItem is UnloadedItem)
                    ModContent.GetInstance<MagicDye>().Logger.WarnFormat(Language.GetTextValue("Mods.MagicDye.Logs.UnloadedWarn"), i, (this.Player.isDisplayDollOrInanimate) ? Language.GetTextValue("Mods.MagicDye.Logs.DummyPlayer") : this.Player.name);
            }
        }
    }
}
