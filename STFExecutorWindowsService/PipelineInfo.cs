using System;
using System.Collections.Generic;

namespace STFExecutorWindowsService
{
    public class PipelineInfo
    {
        public string LockName { get; set; }
        internal ConfigData ConfigData { get; private set; }

        public string PipelineInstance { get; private set; }

        public string ConfigKey { get; set; }
        public string Status { get; internal set; }

        public PipelineInfo(string toolName, string pipeline, string environment, bool updatePlan = false, string pipelineInstanceName = null, List<string> testCasesIds = null)
        {
            ConfigData = GetConfigData(toolName, pipeline, environment);
            ConfigData.UpdatePlan = updatePlan;
            LockName = string.Join("_", environment, pipeline, "Cloud");
            ConfigKey = string.Join("_", environment, toolName, pipeline);
            Status = "Queued";
            if (!string.IsNullOrEmpty(pipelineInstanceName))
            {
                PipelineInstance = string.Join("_", toolName, pipeline, pipelineInstanceName);
            }
            else
            {
                PipelineInstance = string.Join("_", toolName, pipeline, DateTime.UtcNow.Date.ToString(), "1");
            }
        }

        private ConfigData GetConfigData(string toolName, string pipeline, string environment)
        {
            ConfigData testConfigData = new ConfigData();
            testConfigData.Environment = environment;
            testConfigData.Pipeline = pipeline;
            testConfigData.ToolName = toolName;
            testConfigData.Key = string.Join("_", environment, toolName, pipeline);
            testConfigData.ConfigFolder = @"D:\DTE\STF\";
            return testConfigData;
        }


    }
}