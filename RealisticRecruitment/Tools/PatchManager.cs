using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace RealisticRecruitment.Tools
{
    internal struct PatchError
    {
        internal Type[] patchTypes;
        internal Exception exception;
    }

    internal static class PatchManager
    {
        private static readonly Harmony harmony = new Harmony("RealisticRecruitment");
        private static readonly List<PatchError> failedPatches = new List<PatchError>();

        internal static void Apply(params Type[] patchTypes)
        {
            List<Type> appliedPatches = new List<Type>();

            try
            {
                foreach (Type patchType in patchTypes)
                {
                    harmony.CreateClassProcessor(patchType).Patch();
                    appliedPatches.Add(patchType);
                }
            }
            catch (Exception exception)
            {
                foreach (Type patchType in appliedPatches)
                {
                    try { harmony.CreateClassProcessor(patchType).Unpatch(); }
                    catch (Exception innerException) {
                        ErrorFile.Write(
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n" +
                            $"Unpatch failed: {patchType.Name}\n" +
                            $"{innerException}\n" +
                            "----------------------------------------"
                        );
                    }
                }

                failedPatches.Add(new PatchError
                {
                    patchTypes = patchTypes,
                    exception = exception
                });

                ErrorFile.Write(
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n" +
                    $"Patch failed: {string.Join(", ", patchTypes.Select(x => x.Name))}\n" +
                    $"{exception}\n" +
                    "----------------------------------------"
                );
            }
        }

        internal static void ShowPatchErrors()
        {
            if (failedPatches.Count == 0) return;

            InformationManager.ShowInquiry(
                new InquiryData(
                    $"[RealisticRecruitment]\nDisabled Features",
                    $"{string.Join("\n", failedPatches.Select(x => $"{string.Join(", ", x.patchTypes.Select(t => t.Name))} - {x.exception.Message}"))}\n\nSee error.log for more details.",
                    true,
                    false,
                    "Continue",
                    null,
                    null,
                    null
                )
            );
        }
    }
}
