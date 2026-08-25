using TaleWorlds.MountAndBlade;

namespace RealisticRecruitment
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            PatchManager.Apply(typeof(LordRecruitment));
            PatchManager.Apply(typeof(PlayerRecruitment));
            PatchManager.Apply(typeof(Policy_RoyalLevyRight_Create), typeof(Policy_RoyalLevyRight_Compute));
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            PatchManager.ShowPatchErrors();
        }
    }
}

// AI (gerade Söldner, Banditen und co) verbessern in suche nach rekrutierungs spots
// Spawnraten von Notables anpassen anhand des Village oder Town Wealths

// auf nexusmods schreiben theortisch auch abwärts kompatibel aber behaltet error meldungen im auge thema signaturen und so
// Lord & Player Recruitment
// policy
