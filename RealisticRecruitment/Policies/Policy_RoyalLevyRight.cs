using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace RealisticRecruitment.Policies
{
    [HarmonyPatch(typeof(Campaign), "InitializeDefaultCampaignObjects")]
    internal static class Policy_RoyalLevyRight_Create
    {
        internal const string RoyalLevyRightId = "rr_policy_royal_levy_right";

        [HarmonyPostfix]
        private static void Postfix()
        {
            Game.Current.ObjectManager.RegisterPresumedObject<PolicyObject>(new PolicyObject(RoyalLevyRightId))
                .Initialize(
                new TextObject("{=RR_Policy_RoyalLevyRight_Name}Royal Levy Right"),
                new TextObject("{=RR_Policy_RoyalLevyRight_Description}The ruler is granted the right to levy troops throughout the realm, regardless of personal relations with the lords who hold the land."),
                new TextObject("{=RR_Policy_RoyalLevyRight_Log}granting the ruler the right to levy troops throughout the realm"),
                new TextObject("{=RR_Policy_RoyalLevyRight_Effects}The ruler may recruit volunteers from any settlement within the kingdom, regardless of relations with the ruling clan.{newline}Settlement militia production is reduced by 0.5 per day."),
                0.75f,   // Authoritarian
                -0.10f,  // Oligarchic
                -0.50f   // Egalitarian
            );
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementMilitiaModel), nameof(DefaultSettlementMilitiaModel.CalculateMilitiaChange))]
    internal static class Policy_RoyalLevyRight_Compute
    {
        [HarmonyPostfix]
        private static void Postfix(Settlement settlement, ref ExplainedNumber __result)
        {
            Kingdom kingdom = settlement.OwnerClan.Kingdom;
            if (kingdom == null) return;

            PolicyObject royalLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_RoyalLevyRight_Create.RoyalLevyRightId);
            if (kingdom.ActivePolicies.Contains(royalLevyRight))
            {
                __result.Add(-0.5f, royalLevyRight.Name);
            }
        }
    }
}
