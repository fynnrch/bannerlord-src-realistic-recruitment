using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

using RealisticRecruitment.Policies;
using RealisticRecruitment.Tools;

namespace RealisticRecruitment.Recruitment
{
    internal class RecruitmentRestrictions
    {
        internal static bool isHeroAllowedToRecruitInSettlement(Hero hero, Settlement settlement)
        {
            if (isHeroFromOwnerClan(hero, settlement)) return true;
            if (isHeroAllowedToRecruitAsForeigner(hero, settlement)) return true;
            return false;
        }

        private static bool isHeroFromOwnerClan(Hero hero, Settlement settlement)
        {
            if (hero.Clan == settlement.OwnerClan) return true;
            return false;
        }

        private static bool isHeroAllowedToRecruitAsForeigner(Hero hero, Settlement settlement)
        {
            Clan recruiterClan = hero.Clan;
            Clan ownerClan = settlement.OwnerClan;
            PolicyObject nobleLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_NobleLevyRight_Create.NobleLevyRightId);
            PolicyObject royalLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_RoyalLevyRight_Create.RoyalLevyRightId);

            if (FactionManager.IsAtWarAgainstFaction(recruiterClan, ownerClan)) return false;

            int relation = recruiterClan.Leader.GetRelation(ownerClan.Leader);
            bool sameKingdom = recruiterClan.Kingdom != null && ownerClan.Kingdom != null && recruiterClan.Kingdom == ownerClan.Kingdom;
            bool isLordWithNobleLevyRight = sameKingdom && recruiterClan.Kingdom.HasPolicy(nobleLevyRight);
            bool isKingWithRoyalLevyRight = sameKingdom && recruiterClan.Leader == recruiterClan.Kingdom.Leader && recruiterClan.Kingdom.HasPolicy(royalLevyRight);

            if (relation >= ConfigFile.ConfigData.InternalRecruitmentRelationThreshold && sameKingdom) return true;
            if (relation >= ConfigFile.ConfigData.ExternalRecruitmentRelationThreshold) return true;
            if (isLordWithNobleLevyRight) return true;
            if (isKingWithRoyalLevyRight) return true;
            return false;
        }
    }
}
