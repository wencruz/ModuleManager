using System.IO;
using PluginConfig = KSPe.IO.Data.ConfigNode;
using KspConfig = KSPe.IO.KspConfigNode;

namespace ModuleManager
{
    internal static class FilePathRepository
    {
        internal static readonly PluginConfig SHA_CONFIG = PluginConfig.ForType<ModuleManager>(null, "ConfigSHA.cfg");
        internal static readonly PluginConfig CACHE_CONFIG = PluginConfig.ForType<ModuleManager>(null, "ConfigCache.cfg");
        internal static readonly PluginConfig PHYSICS_CONFIG = PluginConfig.ForType<ModuleManager>(null, "Physics.cfg");
        internal static readonly PluginConfig TECHTREE_CONFIG = PluginConfig.ForType<ModuleManager>("TechTree");
        internal static readonly KspConfig PHYSICS_DEFAULT = new KspConfig("Physics");
        internal static readonly KspConfig PART_DATABASE = new KspConfig("PartDatabase");

        internal static readonly string MMCfgOutputPath = KSPe.IO.File<ModuleManager>.Data.Solve("_MMCfgOutput");
    }
}
