using System;
using System.Collections.Generic;

namespace TestConsole
{
    public class PipelineInfo
    {
        public string LockName { get; set; }
        internal ConfigData ConfigData { get; private set; }

        public string PipelineInstance { get; private set; }

        public string ConfigKey { get; set; }

        public PipelineStatus Status { get; set; }

        public PipelineInfo(string toolName, string pipeline, string environment, bool updatePlan = false, string pipelineInstanceName = null, List<string> testCasesIds = null)
        {
            ConfigData = GetConfigData(toolName, pipeline, environment);
            ConfigData.UpdatePlan = updatePlan;
            LockName = string.Join("_", environment, toolName, "Cloud");
            ConfigKey = string.Join("_", environment, toolName, pipeline);
            Status = PipelineStatus.Queued;
            if (!string.IsNullOrEmpty(pipelineInstanceName))
            {
                PipelineInstance = string.Join("_", toolName, pipeline, pipelineInstanceName);
            }
            else
            {
                PipelineInstance = string.Join("_", toolName, pipeline, DateTime.UtcNow.Date.ToString("ddMMMyyyy"), "1");
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

    public enum PipelineStatus
    {
        Queued,
        Running,
        AbortRequested,
        Aborted,
        Completed
    }
}