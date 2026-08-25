using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace RealisticRecruitment
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
                foreach (Type patchType in appliedPatches) harmony.CreateClassProcessor(patchType).Unpatch();

                failedPatches.Add(new PatchError
                {
                    patchTypes = patchTypes,
                    exception = exception
                });
            }
        }

        internal static void ShowPatchErrors()
        {
            ShowNextPatchError();
        }

        private static void ShowNextPatchError(int i = 0)
        {
            if (i >= failedPatches.Count) return;

            PatchError failedPatch = failedPatches[i];
            string patchTypesString = string.Empty;
            foreach (Type patchType in failedPatch.patchTypes) patchTypesString += patchType.Name + "\n";

            InformationManager.ShowInquiry(
                new InquiryData(
                    $"Realistic Recruitment",
                    $"Exception:\n{failedPatch.exception.GetType().FullName}\n\nDisabled Features:\n{patchTypesString}",
                    true,
                    false,
                    "Continue",
                    null,
                    () => { ShowNextPatchError(++i); },
                    null
                )
            );
        }
    }
}
