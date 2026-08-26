using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace RealisticRecruitment
{
    [HarmonyPatch(typeof(RecruitmentCampaignBehavior), "RecruitVolunteersFromNotable")]
    internal static class LordRecruitment
    {
        [HarmonyPrefix]
        private static bool Prefix(MobileParty mobileParty, Settlement settlement)
        {
            // filter non lord partys
            if (!mobileParty.IsLordParty) return true;

            // custom rule: isNpcAllowedToRecruitInSettlement
            if (isNpcAllowedToRecruitInSettlement(mobileParty, settlement)) return true;

            // forbid recruiting
            return false;
        }

        private static bool isNpcAllowedToRecruitInSettlement(MobileParty mobileParty, Settlement settlement)
        {
            if (isNpcFromOwnerClan(mobileParty, settlement)) return true;
            if (isNpcAllowedToRecruitAsForeigner(mobileParty, settlement)) return true;
            return false;
        }

        private static bool isNpcFromOwnerClan(MobileParty mobileParty, Settlement settlement)
        {
            if (mobileParty.LeaderHero.Clan == settlement.OwnerClan) return true;
            return false;
        }

        private static bool isNpcAllowedToRecruitAsForeigner(MobileParty mobileParty, Settlement settlement)
        {
            Clan recruiterClan = mobileParty.LeaderHero.Clan;
            Clan ownerClan = settlement.OwnerClan;
            PolicyObject royalLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_RoyalLevyRight_Create.RoyalLevyRightId);

            if (FactionManager.IsAtWarAgainstFaction(recruiterClan, ownerClan)) return false;

            int relation = recruiterClan.Leader.GetRelation(ownerClan.Leader);
            bool sameKingdom = recruiterClan.Kingdom != null && ownerClan.Kingdom != null && recruiterClan.Kingdom == ownerClan.Kingdom;
            bool isKingWithRoyalLevyRight = sameKingdom && recruiterClan.Leader == recruiterClan.Kingdom.Leader && recruiterClan.Kingdom.HasPolicy(royalLevyRight);

            if (relation >= ConfigFile.ConfigData.InternalRecruitmentRelationThreshold && sameKingdom) return true;
            if (relation >= ConfigFile.ConfigData.ExternalRecruitmentRelationThreshold) return true;
            if (isKingWithRoyalLevyRight) return true;
            return false;
        }
    }
}
