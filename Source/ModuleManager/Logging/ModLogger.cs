using System;
using UnityEngine;

using K = KSPe.Util.Log;

namespace ModuleManager.Logging
{
    public class ModLogger : IBasicLogger
    {
        internal static readonly K.Logger LOG = K.Logger.CreateForType<ModuleManager>();        

        private delegate void LogMethod(string message, object[] parms);
        private readonly LogMethod[] methods;
        public ModLogger()
        {
            this.methods = new LogMethod[5];
            int i = 0;
            this.methods[i++] = new LogMethod(LOG.error);
            this.methods[i++] = new LogMethod(LOG.error);
            this.methods[i++] = new LogMethod(LOG.warn);
            this.methods[i++] = new LogMethod(LOG.info);
            this.methods[i++] = new LogMethod(LOG.error);
            LOG.level = K.Level.TRACE;
        }

        // Gambiarra porque eu não previ essa possibilidade no KSPe.Util.Log!
        private static readonly object[] NONE = new object[0];
		public void Log(LogType logType, string message)
		{
			this.methods[(int)logType](message, NONE);
		}

		public void Exception(string message, Exception exception)
		{
			LOG.error(exception, message);
		}
	}
}
