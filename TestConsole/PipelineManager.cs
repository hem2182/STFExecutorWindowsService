using System;
using System.Collections.Generic;
using log4net;


namespace TestConsole
{
    public static class PipelineManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<string, ExecutionManager> executionManagerDictionary = new Dictionary<string, ExecutionManager>();

        public static string AddToQueue(PipelineInfo pipelineInfo)
        {
            logger.Info("Pipeline Lock Name:" + pipelineInfo.LockName);
            ExecutionManager exeManager = null;
            if (!executionManagerDictionary.ContainsKey(pipelineInfo.LockName))
            {
                logger.Info("Initializing ExecutionManager for Pipeline " + pipelineInfo.LockName);
                exeManager = new ExecutionManager(logger, pipelineInfo);
                executionManagerDictionary[pipelineInfo.LockName] = exeManager;
            }
            logger.Info("Fetching ExecutionManager for Pipeline " + pipelineInfo.LockName);
            exeManager = executionManagerDictionary[pipelineInfo.LockName];
            return exeManager.AddToQueue(pipelineInfo);
        }

        internal static string AbortPipeline(string toolName, string pipeline, string environment, string pipelineInstanceName)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment);
            ExecutionManager exeManager = executionManagerDictionary[pipelineInfo.LockName];
            return exeManager.AbortPipeline(pipelineInstanceName);
        }

        internal static List<string> GetPipelineQueueList(string toolName, string pipeline, string environment)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment);
            ExecutionManager exeManager = executionManagerDictionary[pipelineInfo.LockName];
            return exeManager.GetQueueList();
        }
    }
}