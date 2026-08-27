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
            PatchManager.Apply(typeof(VolunteerSpawnrate));
            PatchManager.Apply(typeof(Policy_NobleLevyRight_Create), typeof(Policy_NobleLevyRight_Compute));
            PatchManager.Apply(typeof(Policy_RoyalLevyRight_Create), typeof(Policy_RoyalLevyRight_Compute));
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            PatchManager.ShowPatchErrors();
        }
    }
}

// TODO
// 1.3
// PatchMonitor
// Erstmal checken wie die überhaupt an ihre truppen kommen wegen mercenary problem, was sind denn alles AI Truppen überhaupt??
// AI (gerade Söldner, Banditen und co) verbessern in suche nach rekrutierungs spots,
// 1.4
// ilegales rekrutieren durch hostile actions mit minus moral in der truppe bestrafen , Kürzliche Aktivitäten plus oder minus auch für das befreien von soldaten
// Eigene Mercenary Troop Trees bauen

//Mercenary Patch
// Macht mercenary patch überhaupt sinn oder rekrutieren die ihre truppen einfach quasi aus der luft?
// Mercenray recruitment policy mit -.25 milita, wenn aktiv sind mercenaries gratis, wenn deaktiviert koster jetzt MEHR denare (Kosten per Influence Point die die erwirtschaften)
// Mercenarie Clans sollen in erster Linie Mercenaries aus Städten rekrutieren oder aber wenn die Policy aktiv ist sie aus den Dörfern ihres aktuellen Königreichs holen. ()

// NEXUSMODS WICHTIG mit 1.2.0 machen
// auf nexusmods schreiben theortisch auch abwärts kompatibel aber behaltet error meldungen im auge thema signaturen und so
// alle modules vorstlellen und erklären
// wichtig besonder CustomData erklären

//Changelog 1.2
// Add FileManager & CustomData System
// Add Noble Levy Right Policy and Tweaked Royal Levy Right (-2/-.5)
// Add Troop Spawnrate mit Schalter in config.json
