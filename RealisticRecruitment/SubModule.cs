using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace RealisticRecruitment
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Harmony harmony = new Harmony("RealisticRecruitment");

            harmony.PatchAll();
        }
    }
}
