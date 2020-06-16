using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModuleManager.Extensions;
using ModuleManager.Logging;
using ModuleManager.Threading;

using static ModuleManager.FilePathRepository;

namespace ModuleManager
{
    public class MMPatchRunner
    {
        private readonly IBasicLogger kspLogger;

        public string Status { get; private set; } = "";
        public string Errors { get; private set; } = "";

        public MMPatchRunner(IBasicLogger kspLogger)
        {
            this.kspLogger = kspLogger ?? throw new ArgumentNullException(nameof(kspLogger));
        }

        public IEnumerator Run()
        {
            PostPatchLoader.Instance.databaseConfigs = null;

            // Wait for game database to be initialized for the 2nd time and wait for any plugins to initialize
            yield return null;
            yield return null;

            IEnumerable<ModListGenerator.ModAddedByAssembly> modsAddedByAssemblies = ModListGenerator.GetAdditionalModsFromStaticMethods(ModLogger.Instance);

            IEnumerable<IProtoUrlConfig> databaseConfigs = null;
            MMPatchLoader patchLoader = new MMPatchLoader(modsAddedByAssemblies, ModLogger.Instance);

            ITaskStatus patchingThreadStatus = BackgroundTask.Start(delegate
            {
                databaseConfigs = patchLoader.Run();
            });

            while(true)
            {
                yield return null;

                Status = patchLoader.status;
                Errors = patchLoader.errors;

                if (!patchingThreadStatus.IsRunning) break;
            }

            if (patchingThreadStatus.IsExitedWithError)
            {
                kspLogger.Exception("The patching thread threw an exception", patchingThreadStatus.Exception);
                FatalErrorHandler.HandleFatalError("The patching thread threw an exception");
            }

            if (databaseConfigs == null)
            {
                kspLogger.Error("The patcher returned a null collection of configs");
                FatalErrorHandler.HandleFatalError("The patcher returned a null collection of configs");
                yield break;
            }

            PostPatchLoader.Instance.databaseConfigs = databaseConfigs;
        }
    }
}
