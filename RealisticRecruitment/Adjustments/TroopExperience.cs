using HarmonyLib;
using TaleWorlds.CampaignSystem.Roster;

using RealisticRecruitment.Tools;

namespace RealisticRecruitment.Adjustments
{
    [HarmonyPatch(typeof(TroopRoster), nameof(TroopRoster.AddXpToTroopAtIndex))]
    internal static class TroopExperience
    {
        [HarmonyPrefix]
        private static void Prefix(ref int xpAmount)
        {
            xpAmount = (int)(xpAmount * ConfigFile.ConfigData.TroopExperienceRate / 100f);
        }
    }
}
