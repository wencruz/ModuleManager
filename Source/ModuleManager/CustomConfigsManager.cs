using System;
using System.IO;
using UnityEngine;

namespace ModuleManager
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CustomConfigsManager : MonoBehaviour
    {
        internal void Start()
        {
			if (HighLogic.CurrentGame.Parameters.Career.TechTreeUrl != MMPatchLoader.TECHTREE_CONFIG.Path && MMPatchLoader.TECHTREE_CONFIG.IsLoadable)
            {
                Log("Setting modded tech tree as the active one");
                HighLogic.CurrentGame.Parameters.Career.TechTreeUrl = MMPatchLoader.TECHTREE_CONFIG.Path;
            }

			if (PhysicsGlobals.PhysicsDatabaseFilename != MMPatchLoader.PHYSICS_CONFIG.Path && MMPatchLoader.PHYSICS_CONFIG.IsLoadable)
            {
                Log("Setting modded physics as the active one");
                PhysicsGlobals.PhysicsDatabaseFilename = MMPatchLoader.PHYSICS_CONFIG.Path;
                if (!PhysicsGlobals.Instance.LoadDatabase())
                    Log("Something went wrong while setting the active physics config.");
            }
        }

		private static readonly KSPe.Util.Log.Logger log = KSPe.Util.Log.Logger.CreateForType<CustomConfigsManager>();
        private static void Log(String s)
        {
            log.info(s);
        }

    }
}
