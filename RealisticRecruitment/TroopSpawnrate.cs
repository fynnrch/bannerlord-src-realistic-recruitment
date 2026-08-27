using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace RealisticRecruitment
{
    [HarmonyPatch(typeof(DefaultVolunteerModel), nameof(DefaultVolunteerModel.GetDailyVolunteerProductionProbability))]
    public static class VolunteerSpawnrate
    {
        static void Postfix(Settlement settlement, ref float __result)
        {
            if (!ConfigFile.ConfigData.RestrictTroopSpawnrate) return;
            if (settlement == null) return;

            if (settlement.IsVillage)
            {
                float hearth = settlement.Village.Hearth;

                float multiplier = MathF.Clamp(
                    hearth / 1000f,
                    0.25f,
                    0.75f
                );

                __result *= multiplier;
            }
            else if (settlement.IsTown)
            {
                float prosperity = settlement.Town.Prosperity;

                float multiplier = MathF.Clamp(
                    prosperity / 10000f,
                    0.35f,
                    0.85f
                );

                __result *= multiplier;
            }
        }
    }
}
