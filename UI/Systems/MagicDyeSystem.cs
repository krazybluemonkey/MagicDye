using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;

namespace MagicDye.UI.Systems
{
    [Autoload(Side = ModSide.Client)]
    class MagicDyeUISystem : ModSystem
    {
        internal UserInterface MagicDyeInterface;
        internal MagicDyeUI MagicDyeUI;
        private GameTime _lastUpdateUiGameTime;

        public override void Load()
        {
            MagicDyeUI = new MagicDyeUI();
            MagicDyeUI.Activate();
            MagicDyeInterface = new UserInterface();
        }

        internal void ShowMagicDyeUI()
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);
            MagicDyeInterface?.SetState(MagicDyeUI);
        }

        internal void HideMagicDyeUI()
        {
            MagicDyeInterface?.SetState(null);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            MagicDyeInterface?.Update(gameTime);
            _lastUpdateUiGameTime = gameTime;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MagicDye: MagicDyeInterface",
                    delegate {
                        if (MagicDyeInterface?.CurrentState != null)
                        {
                            MagicDyeInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                MagicDyeUI = null;
            }
        }
    }
}