using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace RealisticRecruitment
{
    [HarmonyPatch(typeof(DefaultSettlementAccessModel), "CanMainHeroRecruitTroops")]
    internal static class PlayerRecruitment
    {
        [HarmonyPostfix]
        private static void Postfix(Settlement settlement, ref bool __result, ref bool disableOption, ref TextObject disabledText)
        {
            // validate vanilla behaviour
            if (!__result || disableOption) return;

            // custom rule: isPlayerAllowedToRecruitInSettlement
            if (isPlayerAllowedToRecruitInSettlement(Hero.MainHero, settlement)) return;

            // forbid recruiting
            __result = false;
            disableOption = true;
            disabledText = new TextObject("{=RR_NoRecruitmentRight}You have no right to recruit troops here.");
            return;
        }

        private static bool isPlayerAllowedToRecruitInSettlement(Hero hero, Settlement settlement)
        {
            if (isPlayerFromOwnerClan(hero, settlement)) return true;
            if (isPlayerAllowedToRecruitAsForeigner(hero, settlement)) return true;
            return false;
        }

        private static bool isPlayerFromOwnerClan(Hero hero, Settlement settlement)
        {
            if (hero.Clan == settlement.OwnerClan) return true;
            return false;
        }

        private static bool isPlayerAllowedToRecruitAsForeigner(Hero hero, Settlement settlement)
        {
            Clan heroClan = hero.Clan;
            Clan ownerClan = settlement.OwnerClan;
            PolicyObject nobleLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_NobleLevyRight_Create.NobleLevyRightId);
            PolicyObject royalLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_RoyalLevyRight_Create.RoyalLevyRightId);

            int relation = heroClan.Leader.GetRelation(ownerClan.Leader);
            bool sameKingdom = heroClan.Kingdom != null && ownerClan.Kingdom != null && heroClan.Kingdom == ownerClan.Kingdom;
            bool isLordWithNobleLevyRight = sameKingdom && heroClan.Kingdom.HasPolicy(nobleLevyRight);
            bool isKingWithRoyalLevyRight = sameKingdom && heroClan.Leader == heroClan.Kingdom.Leader && heroClan.Kingdom.HasPolicy(royalLevyRight);

            if (relation >= ConfigFile.ConfigData.InternalRecruitmentRelationThreshold && sameKingdom) return true;
            if (relation >= ConfigFile.ConfigData.ExternalRecruitmentRelationThreshold) return true;
            if (isLordWithNobleLevyRight) return true;
            if (isKingWithRoyalLevyRight) return true;
            return false;
        }
    }
}
