using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace RealisticRecruitment.Recruitment
{
    [HarmonyPatch(typeof(RecruitmentCampaignBehavior), "RecruitVolunteersFromNotable")]
    internal static class LordRecruitmentRule
    {
        [HarmonyPrefix]
        private static bool Prefix(MobileParty mobileParty, Settlement settlement)
        {
            // filter non lord partys
            if (!mobileParty.IsLordParty) return true;

            // custom rule: isHeroAllowedToRecruitInSettlement
            if (RecruitmentRestrictions.isHeroAllowedToRecruitInSettlement(mobileParty.LeaderHero, settlement)) return true;

            // forbid recruiting
            return false;
        }
    }

    [HarmonyPatch(typeof(AiVisitSettlementBehavior), "GetApproximateVolunteersCanBeRecruitedDataFromSettlement")]
    internal static class LordRecruitmentDecision
    {
        [HarmonyPrefix]
        private static bool Prefix(Hero hero, Settlement settlement, ref ValueTuple<int, float> __result)
        {
            // custom rule: isHeroAllowedToRecruitInSettlement
            if (RecruitmentRestrictions.isHeroAllowedToRecruitInSettlement(hero, settlement)) return true;

            // forbid AI to recruit volunteers in this settlement
            __result = (0, 0f);
            return false;
        }
    }
}
