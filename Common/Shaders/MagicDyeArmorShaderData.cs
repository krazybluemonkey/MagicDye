using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria.ID;

namespace MagicDye.Common.Shaders
{
    public class MagicDyeArmorShaderData : ArmorShaderData
    {
        private int dyeSlot;
        private Vector3 tempColorP = Vector3.One;
        private Vector3 tempColorS = Vector3.One;
        private float tempSat = 1f;
        private float tempOpacity = 1f;
        private string tempPass = "ArmorColored";

        public MagicDyeArmorShaderData(Ref<Effect> shader, string passName)
        : base(shader, passName)
        {}

        public MagicDyeArmorShaderData(Asset<Effect> shader, string passName)
        : base(shader, passName)
        { }

        public override void Apply(Entity entity, DrawData? drawData)
        {
            MagicDyePlayer MDPlayer;
            Player CurrPlayer = entity as Player;
            Projectile Pet;
            if (CurrPlayer != null && !CurrPlayer.isDisplayDollOrInanimate && !CurrPlayer.isHatRackDoll)
            {
                MDPlayer  = CurrPlayer.GetModPlayer<MagicDyePlayer>();
            }
            else if (entity is Projectile)
            {
                Pet = entity as Projectile;
                MDPlayer = Main.player[Pet.owner].GetModPlayer<MagicDyePlayer>();
            }
            else
            {
                return;
            }

            if (MDPlayer != null)
            {
                tempColorP = MDPlayer.MagicDyePrimaryColors[dyeSlot];
                tempColorS = MDPlayer.MagicDyeSecondaryColors[dyeSlot];
                tempSat = MDPlayer.MagicDyeSaturation[dyeSlot];
                tempOpacity = MDPlayer.MagicDyeOpacity[dyeSlot];
                tempPass = MDPlayer.MagicDyePasses[dyeSlot];
                var parentShader = GameShaders.Armor.GetShaderFromItemId(MDPlayer.MagicDyeItem[dyeSlot].type);
                // basic sanity checks just in case
                if (MDPlayer.MagicDyeItem[dyeSlot].type != ItemID.None && parentShader != null)
                {
                    Ref<Effect> tempShader = null;
                    Asset<Effect> tempAsset = null;

                    if (parentShader.Shader != null && this.Shader != parentShader.Shader)
                    {
                        tempShader = (Ref<Effect>)typeof(ShaderData).GetField("_shader", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parentShader);
                        tempAsset = (Asset<Effect>)typeof(ShaderData).GetField("_asset", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(parentShader);

                        // determine what to swap or null out. based on how Terraria.Graphics.Shaders.ShaderData.Shader does it
                        if (tempShader != null)
                        {
                            FieldInfo shaderInfo = typeof(ShaderData).GetField("_shader", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                            shaderInfo?.SetValue(this, tempShader);
                            FieldInfo assetInfo = typeof(ShaderData).GetField("_asset", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                            assetInfo?.SetValue(this, null);
                        }
                        if (tempAsset != null)
                        {
                            FieldInfo shaderInfo = typeof(ShaderData).GetField("_shader", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                            shaderInfo?.SetValue(this, null);
                            FieldInfo assetInfo = typeof(ShaderData).GetField("_asset", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                            assetInfo?.SetValue(this, tempAsset);
                        }


                        // if the pass doesn't exist in this shader, default to ArmorColored and the default Shader.
                        // seems to come up most often when closing the UI with temporary chnages and reopening it.
                        // but can happen with unloaded modded dyes on the player
                        if (this.Shader.CurrentTechnique.Passes[tempPass] == null)
                        {
                            tempPass = "ArmorColored";
                            var BailOutParent = GameShaders.Armor.GetShaderFromItemId(ItemID.RedDye);
                            BailOutParent.Apply(entity, drawData);
                            tempShader = (Ref<Effect>)typeof(ShaderData).GetField("_shader", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(BailOutParent);
                            if (tempShader != null)
                            {
                                FieldInfo shaderInfo = typeof(ShaderData).GetField("_shader", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                                shaderInfo?.SetValue(this, tempShader);
                                FieldInfo assetInfo = typeof(ShaderData).GetField("_asset", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                                assetInfo?.SetValue(this, null);
                            }
                        }
                    }

                    // this applies the "parent" shaders' shaderdata. allows dyes with images and other data to function properly
                    parentShader.Apply(entity, drawData);

                    UseTargetPosition((Vector2)(parentShader.Shader.Parameters["uTargetPosition"]?.GetValueVector2())); // fixes twilight dye
                }

                if (tempPass != null && this.Shader.CurrentTechnique.Passes[tempPass] != null)
                {
                    SwapProgram(tempPass);
                }

                UseColor(tempColorP);

                UseSecondaryColor(tempColorS);

                UseSaturation(tempSat);

                UseOpacity(tempOpacity);

                ColorModifier tempMod1 = MagicDye.instance.colorModifiers.Find(x => x.name == MDPlayer.MagicDyeColorMod1[dyeSlot]);

                if (tempMod1 != null)
                {
                    var colorMod1 = tempMod1.colorFunction(entity, tempColorP, tempColorS, tempSat, tempOpacity);

                    if (colorMod1.primaryColor.X != -100) // if a color modifier returns a -100 primaryColor red, don't apply shaderdata
                    {
                        tempColorP = colorMod1.primaryColor;
                        tempColorS = colorMod1.secondaryColor;
                        tempSat = colorMod1.saturation;
                        tempOpacity = colorMod1.opacity;

                        UseColor(colorMod1.primaryColor);

                        UseSecondaryColor(colorMod1.secondaryColor);

                        UseSaturation(colorMod1.saturation);

                        UseOpacity(colorMod1.opacity);
                    }
                }

                ColorModifier tempMod2 = MagicDye.instance.colorModifiers.Find(x => x.name == MDPlayer.MagicDyeColorMod2[dyeSlot]);

                if (tempMod2 != null)
                {
                    var colorMod2 = tempMod2.colorFunction(entity, tempColorP, tempColorS, tempSat, tempOpacity);

                    if (colorMod2.primaryColor.X != -100) // if a color modifier returns a -100 primaryColor red, don't change colors
                    {
                        tempColorP = colorMod2.primaryColor;
                        tempColorS = colorMod2.secondaryColor;
                        tempSat = colorMod2.saturation;
                        tempOpacity = colorMod2.opacity;

                        UseColor(colorMod2.primaryColor);

                        UseSecondaryColor(colorMod2.secondaryColor);

                        UseSaturation(colorMod2.saturation);

                        UseOpacity(colorMod2.opacity);
                    }
                }

                base.Apply(entity, drawData);
            }
        }

        public MagicDyeArmorShaderData UseDyeSlot(int slot)
        {
            dyeSlot = slot;
            return this;
        }
    }
}