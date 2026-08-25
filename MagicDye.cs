using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.Reflection;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using MagicDye.Content.Items.Dyes;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace MagicDye
{
    public class MagicDye : Mod
    {
        internal static MagicDye instance;
        internal List<ColorModifier> colorModifiers = new List<ColorModifier>();
        public static int[] dyeLookUp = new int[32];

        public override void Load()
        {
            instance = this;
            InitDyeLookUp();
        }

        private void InitDyeLookUp()
        {
            dyeLookUp[0] = ModContent.ItemType<MagicDyeSlot1>();
            dyeLookUp[1] = ModContent.ItemType<MagicDyeSlot2>();
            dyeLookUp[2] = ModContent.ItemType<MagicDyeSlot3>();
            dyeLookUp[3] = ModContent.ItemType<MagicDyeSlot4>();
            dyeLookUp[4] = ModContent.ItemType<MagicDyeSlot5>();
            dyeLookUp[5] = ModContent.ItemType<MagicDyeSlot6>();
            dyeLookUp[6] = ModContent.ItemType<MagicDyeSlot7>();
            dyeLookUp[7] = ModContent.ItemType<MagicDyeSlot8>();
            dyeLookUp[8] = ModContent.ItemType<MagicDyeSlot9>();
            dyeLookUp[9] = ModContent.ItemType<MagicDyeSlot10>();
            dyeLookUp[10] = ModContent.ItemType<MagicDyeSlot11>();
            dyeLookUp[11] = ModContent.ItemType<MagicDyeSlot12>();
            dyeLookUp[12] = ModContent.ItemType<MagicDyeSlot13>();
            dyeLookUp[13] = ModContent.ItemType<MagicDyeSlot14>();
            dyeLookUp[14] = ModContent.ItemType<MagicDyeSlot15>();
            dyeLookUp[15] = ModContent.ItemType<MagicDyeSlot16>();
            dyeLookUp[16] = ModContent.ItemType<MagicDyeSlot17>();
            dyeLookUp[17] = ModContent.ItemType<MagicDyeSlot18>();
            dyeLookUp[18] = ModContent.ItemType<MagicDyeSlot19>();
            dyeLookUp[19] = ModContent.ItemType<MagicDyeSlot20>();
            dyeLookUp[20] = ModContent.ItemType<MagicDyeSlot21>();
            dyeLookUp[21] = ModContent.ItemType<MagicDyeSlot22>();
            dyeLookUp[22] = ModContent.ItemType<MagicDyeSlot23>();
            dyeLookUp[23] = ModContent.ItemType<MagicDyeSlot24>();
            dyeLookUp[24] = ModContent.ItemType<MagicDyeSlot25>();
            dyeLookUp[25] = ModContent.ItemType<MagicDyeSlot26>();
            dyeLookUp[26] = ModContent.ItemType<MagicDyeSlot27>();
            dyeLookUp[27] = ModContent.ItemType<MagicDyeSlot28>();
            dyeLookUp[28] = ModContent.ItemType<MagicDyeSlot29>();
            dyeLookUp[29] = ModContent.ItemType<MagicDyeSlot30>();
            dyeLookUp[30] = ModContent.ItemType<MagicDyeSlot31>();
            dyeLookUp[31] = ModContent.ItemType<MagicDyeSlot32>();
        }

        public static Color ClampColor(Vector3 color)
        {
            byte R;
            byte G;
            byte B;

            if (color.X > 0)
            {
                R = (Byte)Math.Clamp(255 * color.X, 0, 255);
            }
            else
            {
                R = (Byte)Math.Clamp(255 + (255 * color.X), 0, 255);
            }

            if (color.Y > 0)
            {
                G = (Byte)Math.Clamp(255 * color.Y, 0, 255);
            }
            else
            {
                G = (Byte)Math.Clamp(255 + (255 * color.Y), 0, 255);
            }

            if (color.Z > 0)
            {
                B = (Byte)Math.Clamp(255 * color.Z, 0, 255);
            }
            else
            {
                B = (Byte)Math.Clamp(255 + (255 * color.Z), 0, 255);
            }


            return new Color(R, G, B);
        }

        public static int GetItemIDFromPass(string pass)
        {
            List<ArmorShaderData> shaderDataList = (List<ArmorShaderData>)typeof(ArmorShaderDataSet).GetField("_shaderData", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(GameShaders.Armor);
            Dictionary<int, int> shaderDictionary = (Dictionary<int, int>)typeof(ArmorShaderDataSet).GetField("_shaderLookupDictionary", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(GameShaders.Armor);

            for (int i = 0; i < shaderDataList.Count; i++)
            {
                string passName = (string)typeof(ShaderData).GetField("_passName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(shaderDataList[i]);
                if (passName == pass)
                {
                    return shaderDictionary.FirstOrDefault(x => x.Value == i+1).Key;
                }
            }

            return 0;
        }

        public static (SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect customEffect, Matrix transformMatrix) GetSpriteBatchInformation(SpriteBatch spriteBatch)
        {

            SpriteSortMode sortMode = (SpriteSortMode)typeof(SpriteBatch).GetField("sortMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            BlendState blendState = (BlendState)typeof(SpriteBatch).GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)typeof(SpriteBatch).GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)typeof(SpriteBatch).GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)typeof(SpriteBatch).GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect customEffect = (Effect)typeof(SpriteBatch).GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix transformMatrix = (Matrix)typeof(SpriteBatch).GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);


            return (sortMode, blendState, samplerState, depthStencilState, rasterizerState, customEffect, transformMatrix);
        }

        public override object Call(params object[] args)
        {
            try
            {
                string message = args[0] as string;
                if (message == "AddColorMod")
                {
                    string modName = args[1] as string;
                    int itemType = Convert.ToInt32(args[2]);
                    Func<Entity, Vector3, Vector3, float, float, (Vector3, Vector3, float, float)> colorFunc = args[3] as Func<Entity, Vector3, Vector3, float, float, (Vector3, Vector3, float, float)>;
                    if (!Main.dedServ)
                    {
                        colorModifiers.Add(new ColorModifier(modName, itemType, colorFunc));
                    }

                    return "Success";
                }
                else
                {
                    Logger.Error("Call Error: Unknown Message: " + message);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Call Error: " + e.StackTrace + e.Message);
            }
            return "Failure";
        }
    }
}