using TaleWorlds.Library;

namespace RealisticRecruitment.Tools
{
    internal class PrintManager
    {
        internal static void PrintInGame(string s)
        {
            InformationManager.DisplayMessage(new InformationMessage(s));
        }
    }
}
