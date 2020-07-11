﻿using System;
using ModuleManager.Extensions;
using ModuleManager.Logging;

namespace ModuleManager.Progress
{
    public class PatchProgress : IPatchProgress
    {
        public ProgressCounter Counter { get; private set; }

        private readonly IBasicLogger logger;

        public float ProgressFraction
        {
            get
            {
                if (Counter.totalPatches > 0)
                    return Counter.appliedPatches / (float)Counter.totalPatches;
                return 0;
            }
        }

        public EventVoid OnPatchApplied { get; } = new EventVoid("OnPatchApplied");
        public EventData<IPass> OnPassStarted { get; } = new EventData<IPass>("OnPassStarted");

        public PatchProgress(IBasicLogger logger)
        {
            this.logger = logger;
            Counter = new ProgressCounter();
        }

        public PatchProgress(IPatchProgress progress, IBasicLogger logger)
        {
            this.logger = logger;
            Counter = progress.Counter;
        }

        public void PatchAdded()
        {
            Counter.totalPatches.Increment();
        }

        public void PassStarted(IPass pass)
        {
            if (pass == null) throw new ArgumentNullException(nameof(pass));
            logger.Info(pass.Name + " pass");
            OnPassStarted.Fire(pass);
        }

        public void ApplyingUpdate(IUrlConfigIdentifier original, UrlDir.UrlConfig patch)
        {
            logger.Info($"Applying update {patch.SafeUrl()} to {original.FullUrl}");
            Counter.patchedNodes.Increment();
        }

        public void ApplyingCopy(IUrlConfigIdentifier original, UrlDir.UrlConfig patch)
        {
            logger.Info($"Applying copy {patch.SafeUrl()} to {original.FullUrl}");
            Counter.patchedNodes.Increment();
        }

        public void ApplyingDelete(IUrlConfigIdentifier original, UrlDir.UrlConfig patch)
        {
            logger.Info($"Applying delete {patch.SafeUrl()} to {original.FullUrl}");
            Counter.patchedNodes.Increment();
        }

        public void PatchApplied()
        {
            Counter.appliedPatches.Increment();
            OnPatchApplied.Fire();
        }

        public void NeedsUnsatisfiedRoot(UrlDir.UrlConfig url)
        {
            logger.Info($"Deleting root node in file {url.parent.url} node: {url.type} as it can't satisfy its NEEDS");
            Counter.needsUnsatisfied.Increment();
        }

        public void NeedsUnsatisfiedNode(UrlDir.UrlConfig url, string path)
        {
            logger.Info($"Deleting node in file {url.parent.url} subnode: {path} as it can't satisfy its NEEDS");
        }

        public void NeedsUnsatisfiedValue(UrlDir.UrlConfig url, string path)
        {
            logger.Info($"Deleting value in file {url.parent.url} value: {path} as it can't satisfy its NEEDS");
        }

        public void NeedsUnsatisfiedBefore(UrlDir.UrlConfig url)
        {
            logger.Info($"Deleting root node in file {url.parent.url} node: {url.type} as it can't satisfy its BEFORE");
            Counter.needsUnsatisfied.Increment();
        }

        public void NeedsUnsatisfiedFor(UrlDir.UrlConfig url)
        {
            logger.Warning($"Deleting root node in file {url.parent.url} node: {url.type} as it can't satisfy its FOR (this shouldn't happen)");
            Counter.needsUnsatisfied.Increment();
        }

        public void NeedsUnsatisfiedAfter(UrlDir.UrlConfig url)
        {
            logger.Info($"Deleting root node in file {url.parent.url} node: {url.type} as it can't satisfy its AFTER");
            Counter.needsUnsatisfied.Increment();
        }

        public void Warning(UrlDir.UrlConfig url, string message)
        {
            Counter.warnings.Increment();
            logger.Warning(message);
            RecordWarningFile(url);
        }

        public void Error(UrlDir.UrlConfig url, string message)
        {
            Counter.errors.Increment();
            logger.Error(message);
            RecordErrorFile(url);
        }

        public void Error(string message)
        {
            Counter.errors.Increment();
            logger.Error(message);
        }

        public void Exception(string message, Exception exception)
        {
            Counter.exceptions.Increment();
            logger.Exception(message, exception);
        }

        public void Exception(UrlDir.UrlConfig url, string message, Exception exception)
        {
            Exception(message, exception);
            RecordErrorFile(url);
        }

        private void RecordWarningFile(UrlDir.UrlConfig url)
        {
            string key = url.parent.GetUrlWithExtension();
            if (key[0] == '/')
                key = key.Substring(1);

            if (Counter.warningFiles.ContainsKey(key))
                Counter.warningFiles[key] += 1;
            else
                Counter.warningFiles[key] = 1;
        }

        private void RecordErrorFile(UrlDir.UrlConfig url)
        {
            string key = url.parent.GetUrlWithExtension();
            if (key[0] == '/')
                key = key.Substring(1);

            if (Counter.errorFiles.ContainsKey(key))
                Counter.errorFiles[key] += 1;
            else
                Counter.errorFiles[key] = 1;
        }
    }
}
