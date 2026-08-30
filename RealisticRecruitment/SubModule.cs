using TaleWorlds.MountAndBlade;

using RealisticRecruitment.Adjustments;
using RealisticRecruitment.Policies;
using RealisticRecruitment.Recruitment;
using RealisticRecruitment.Tools;

namespace RealisticRecruitment
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            //Tools
            ErrorFile.Init();
            ConfigFile.Init();

            //Adjustements
            PatchManager.Apply(typeof(TroopExperience));
            PatchManager.Apply(typeof(VolunteerSpawnrate));

            // Recruitment
            PatchManager.Apply(typeof(LordRecruitmentRule));
            PatchManager.Apply(typeof(LordRecruitmentDecision));
            PatchManager.Apply(typeof(PlayerRecruitmentRule));
            
            // Policies
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
// 1.3 AI Behavior
// Added LordRecruimtbneDecision
// Added TroopExperience
// Added Value in config für Troop Exeprience

// 1.4 Prisoner
// Befreite:
// if (sameCulture && 10%) || (isMerc && 50%) -> bieten direkt den join an
// Gefangene:
// if (25%) -> von vanilla gefangenen landen tatsächlich bei den gefangenen
// if (sameCulture && 5%) || (isMerc && 25%) -> bieten direkt die rekrutierung (join) an
// Allgemein:
// evt falls join angeboten ist direkt button anbieten kein join über zeit (muss auch in die AI implementierbar sein)
// wenn befreíte oder gefangene joinen starten sie als wounded im trupp
// Höhere Chance das Prisoner fliehen

// 1.5 Performance
// PatchMonitor
// ilegales rekrutieren durch hostile actions mit minus moral in der truppe bestrafen , Kürzliche Aktivitäten plus oder minus auch für das befreien von soldaten
// Eigene Mercenary Troop Trees bauen

// Mercenary Patch
// Macht mercenary patch überhaupt sinn oder rekrutieren die ihre truppen einfach quasi aus der luft?
// Nur lösung finden für den Player als merc, die ai generiert ja ihre truppen
// Mercenray recruitment policy mit -.25 milita, wenn aktiv sind mercenaries gratis, wenn deaktiviert koster jetzt MEHR denare (Kosten per Influence Point die die erwirtschaften)
// Mercenarie Clans sollen in erster Linie Mercenaries aus Städten rekrutieren oder aber wenn die Policy aktiv ist sie aus den Dörfern ihres aktuellen Königreichs holen. ()

// Writ of Levy
// erfordert AI update dann kann ich nachdenken ob es item oder quest werden soll
// Writ of Levy(Aushebungsmandat), eine quest in der/ oder ein item das erlaubt ... von einem settlement eines clans x rekrutiert werden soll
// einmaliges rekrutieren nur dann möglich wenn potentielle rekruten > 0, aber wie viele tatsächlich da sind ist abhängig von den notablen im settlement
// jeder lord kann dir die quest/ oder das item... geben wenn fiefs > 0
// 500 als basepreis * kombi aus avg(wealth der settlements) und realtion mit dem clan
