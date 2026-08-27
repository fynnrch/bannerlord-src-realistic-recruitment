using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace RealisticRecruitment
{
    [HarmonyPatch(typeof(Campaign), "InitializeDefaultCampaignObjects")]
    internal static class Policy_NobleLevyRight_Create
    {
        internal const string NobleLevyRightId = "rr_policy_noble_levy_right";

        [HarmonyPostfix]
        private static void Postfix()
        {
            Game.Current.ObjectManager.RegisterPresumedObject<PolicyObject>(new PolicyObject(NobleLevyRightId))
                .Initialize(
                new TextObject("{=RR_Policy_NobleLevyRight_Name}Noble Levy Right"),
                new TextObject("{=RR_Policy_NobleLevyRight_Description}The nobles of the realm are granted the right to levy troops throughout the kingdom, regardless of personal relations with the lords who hold the land."),
                new TextObject("{=RR_Policy_NobleLevyRight_Log}granting the nobles the right to levy troops throughout the realm"),
                new TextObject("{=RR_Policy_NobleLevyRight_Effects}All clans of the kingdom may recruit volunteers from settlements held by other clans within the kingdom, regardless of personal relations with the ruling clan.{newline}Settlement militia production is reduced by 2 per day."),
                0.10f,   // Authoritarian
                0.75f,  // Oligarchic
                -0.40f   // Egalitarian
            );
        }
    }

    [HarmonyPatch(typeof(DefaultSettlementMilitiaModel), nameof(DefaultSettlementMilitiaModel.CalculateMilitiaChange))]
    internal static class Policy_NobleLevyRight_Compute
    {
        [HarmonyPostfix]
        private static void Postfix(Settlement settlement, ref ExplainedNumber __result)
        {
            Kingdom kingdom = settlement.OwnerClan.Kingdom;
            if (kingdom == null) return;

            PolicyObject nobleLevyRight = Game.Current.ObjectManager.GetObject<PolicyObject>(Policy_NobleLevyRight_Create.NobleLevyRightId);
            if (kingdom.ActivePolicies.Contains(nobleLevyRight))
            {
                __result.Add(-2f, nobleLevyRight.Name);
            }
        }
    }
}
