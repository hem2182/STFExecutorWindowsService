using System;
using System.Collections.Generic;

namespace STFExecutorWindowsService
{
    public sealed class PipelineManager
    {
        private static Dictionary<string, ExecutionManager> executionManagerDictionary = new Dictionary<string, ExecutionManager>();

        public PipelineManager()
        {

        }

        public string AddToQueue(PipelineInfo pipelineInfo)
        {
            ExecutionManager exeManager = null;
            if (!executionManagerDictionary.ContainsKey(pipelineInfo.LockName))
            {
                exeManager = new ExecutionManager(pipelineInfo);
                executionManagerDictionary[pipelineInfo.LockName] = exeManager;
            }
            exeManager = executionManagerDictionary[pipelineInfo.LockName];
            return exeManager.AddToQueue(pipelineInfo);
        }

        internal string AbortPipeline(string toolName, string pipeline, string environment, string pipelineInstanceName)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment);
            ExecutionManager exeManager = executionManagerDictionary[pipelineInfo.LockName];
            return exeManager.AbortPipeline(pipelineInstanceName);
        }

        internal List<string> GetPipelineQueueList(string toolName, string pipeline, string environment)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment);
            ExecutionManager exeManager = executionManagerDictionary[pipelineInfo.LockName];
            return exeManager.GetQueueList();
        }
    }
}