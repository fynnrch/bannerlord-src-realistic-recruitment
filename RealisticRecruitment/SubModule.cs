using TaleWorlds.MountAndBlade;

namespace RealisticRecruitment
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            ErrorFile.Init();
            ConfigFile.Init();

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

// PatchMonitor
// ilegales rekrutieren durch hostile actions mit minus moral in der truppe bestrafen
// AI (gerade Söldner, Banditen und co) verbessern in suche nach rekrutierungs spots
// Mercenaries patchen das die auch irgendwo truppen herbekommen
// Spawnraten von Notables anpassen anhand des Village oder Town Wealths
// neue policy wo alle lords unteriander ausheben können mit -2 miliz -.5 fpr royal policy

// auf nexusmods schreiben theortisch auch abwärts kompatibel aber behaltet error meldungen im auge thema signaturen und so
// alle modules vorstlellen und erklären
// wichtig besonder CustomData erklären
