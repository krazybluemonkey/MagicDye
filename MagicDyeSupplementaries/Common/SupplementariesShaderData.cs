using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using MagicDyeSupplementaries.ColorModifiers;
using ReLogic.Content;

namespace MagicDyeSupplementaries
{
    public class SupplementariesShaderData : ArmorShaderData
    {
        private Vector3 _nColor = Vector3.Zero;
        private Vector3 _nSecondaryColor = Vector3.Zero;
        private float _nSaturation = 1f;
        private float _nOpacity = 1f;
        private int _type = 0;
        public SupplementariesShaderData(Ref<Effect> shader, string passName)
        : base(shader, passName)
        {
        }

        public override void Apply(Entity entity, DrawData? drawData)
        {
            var tempColor = ShiftingRainbowColorModifier.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);//shifting rainbow

            switch (_type)
            {
                case 1: //biome
                    tempColor = Biome.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 2: //crystal shine
                    tempColor = CrystalShine.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 3: //demon fire
                    tempColor = DemonFire.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 4: //depth
                    tempColor = Depth.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 5: //determined heart
                    tempColor = DeterminedHeart.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 6: //life
                    tempColor = Life.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 7: //mana
                    tempColor = Mana.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 8: //riches
                    tempColor = Riches.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 9: //shifting rainbow slot
                    break;
                case 10: //speed
                    tempColor = Speed.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 11: //star light
                    tempColor = StarLight.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 12: //time
                    tempColor = Time.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 13:
                    tempColor = PlayerHair.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 14:
                    tempColor = PlayerSkin.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 15:
                    tempColor = PlayerEyes.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 16:
                    tempColor = PlayerShirt.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 17:
                    tempColor = PlayerUndershirt.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 18:
                    tempColor = PlayerPants.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
                case 19:
                    tempColor = PlayerShoes.ModifyColor(entity, _nColor, _nSecondaryColor, _nSaturation, _nOpacity);
                    break;
            }
            if (tempColor.primaryColor.X == -100)
                return;

            UseColor(tempColor.primaryColor);

            UseSecondaryColor(tempColor.secondaryColor);

            UseSaturation(tempColor.saturation);

            UseOpacity(tempColor.opacity);

            base.Apply(entity, drawData);
        }

        public SupplementariesShaderData UseNewColor(float r, float g, float b) => UseNewColor(new Vector3(r, g, b));
        public SupplementariesShaderData UseNewColor(Color color) => UseNewColor(color.ToVector3());

        public SupplementariesShaderData UseNewColor(Vector3 color)
        {
            _nColor = color;
            return this;
        }

        public SupplementariesShaderData UseNewOpacity(float alpha)
        {
            _nOpacity = alpha;
            return this;
        }

        public SupplementariesShaderData UseNewSecondaryColor(float r, float g, float b) => UseNewSecondaryColor(new Vector3(r, g, b));
        public SupplementariesShaderData UseNewSecondaryColor(Color color) => UseNewSecondaryColor(color.ToVector3());

        public SupplementariesShaderData UseNewSecondaryColor(Vector3 color)
        {
            _nSecondaryColor = color;
            return this;
        }

        public SupplementariesShaderData UseNewSaturation(float saturation)
        {
            _nSaturation = saturation;
            return this;
        }

        public SupplementariesShaderData UseType(int type)
        {
            _type = type;
            return this;
        }
    }
}