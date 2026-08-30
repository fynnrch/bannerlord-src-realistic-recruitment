using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace RealisticRecruitment.Recruitment
{
    [HarmonyPatch(typeof(DefaultSettlementAccessModel), "CanMainHeroRecruitTroops")]
    internal static class PlayerRecruitmentRule
    {
        [HarmonyPostfix]
        private static void Postfix(Settlement settlement, ref bool __result, ref bool disableOption, ref TextObject disabledText)
        {
            // validate vanilla behaviour
            if (!__result || disableOption) return;

            // custom rule: isHeroAllowedToRecruitInSettlement
            if (RecruitmentRestrictions.isHeroAllowedToRecruitInSettlement(Hero.MainHero, settlement)) return;

            // forbid recruiting
            __result = false;
            disableOption = true;
            disabledText = new TextObject("{=RR_NoRecruitmentRight}You have no right to recruit troops here.");
            return;
        }
    }
}
