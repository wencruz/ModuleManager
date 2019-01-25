using System;
using System.IO;
using UnityEngine;

using static ModuleManager.FilePathRepository;

namespace ModuleManager
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CustomConfigsManager : MonoBehaviour
    {
        internal void Start()
        {
            if (HighLogic.CurrentGame.Parameters.Career.TechTreeUrl != TECHTREE_CONFIG.Path && TECHTREE_CONFIG.IsLoadable)
            {
                Log("Setting modded tech tree as the active one");
                HighLogic.CurrentGame.Parameters.Career.TechTreeUrl = TECHTREE_CONFIG.Path;
            }

            if (PhysicsGlobals.PhysicsDatabaseFilename != PHYSICS_CONFIG.Path && PHYSICS_CONFIG.IsLoadable)
            {
                Log("Setting modded physics as the active one");

                PhysicsGlobals.PhysicsDatabaseFilename = PHYSICS_CONFIG.Path;

                if (!PhysicsGlobals.Instance.LoadDatabase())
                    Log("Something went wrong while setting the active physics config.");
            }
        }

        public static void Log(String s)
        {
            print("[CustomConfigsManager] " + s);
        }

    }
}
