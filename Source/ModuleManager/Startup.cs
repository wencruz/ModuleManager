using System;
using UnityEngine;

using ModuleManager.Logging;

namespace ModuleManager
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class Startup : MonoBehaviour
	{
        private void Start()
        {
            ModLogger.LOG.force("Version {0}", Version.Text);
        }
	}

}
