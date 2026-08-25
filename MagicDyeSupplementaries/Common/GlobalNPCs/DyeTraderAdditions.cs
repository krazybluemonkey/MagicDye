using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MagicDyeSupplementaries.Content.Items.Dyes;

namespace MagicDyeSupplementaries.Common.GlobalNPCs
{
    class DyeTraderAdditions : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.DyeTrader)
            {
                shop.Add<FamiliarHairDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhases04);
                shop.Add<FamiliarSkinDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhaseWaningGibbous);
                shop.Add<FamiliarEyesDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhaseThirdQuarter);
                shop.Add<FamiliarShirtDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhaseWaningCrescent);
                shop.Add<FamiliarUndershirtDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhaseWaxingCrescent);
                shop.Add<FamiliarPantsDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhaseFirstQuarter);
                shop.Add<FamiliarShoesDye>(Condition.NpcIsPresent(NPCID.Clothier), Condition.MoonPhaseWaxingGibbous);
                shop.Add<SilverTrimDye>(Condition.MoonPhases26);
            }
        }
    }
}