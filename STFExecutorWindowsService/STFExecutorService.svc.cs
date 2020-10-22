using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace STFExecutorWindowsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class STFExecutorService : ISTFExecutorService
    {
        public string AbortPipeline(string toolName, string pipeline, string environment, string pipelineInstanceName)
        {
            PipelineManager pipelineManager = new PipelineManager();
            var abortPipelineStatus = pipelineManager.AbortPipeline(toolName, pipeline, environment, pipelineInstanceName);
            var newQueueList = GetPipelineQueue(toolName, pipeline, environment);
            return abortPipelineStatus;
        }

        public List<string> GetPipelineQueue(string toolName, string pipeline, string environment)
        {
            PipelineManager pipelineManager = new PipelineManager();
            var queueList = pipelineManager.GetPipelineQueueList(toolName, pipeline, environment);
            return queueList;
        }

        public string RerunPipeline(string toolName, string pipeline, string environment, string pipelineInstanceName, List<string> testCaseIds  = null)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment, pipelineInstanceName: pipelineInstanceName, testCasesIds: testCaseIds);
            PipelineManager pipelineManager = new PipelineManager();
            var rerunPipelineStatus = pipelineManager.AddToQueue(pipelineInfo);
            return rerunPipelineStatus;
        }

        public string TriggerPipeline(string toolName, string pipeline, string environment, bool updatePlan)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment, updatePlan);
            PipelineManager pipelineManager = new PipelineManager();
            var triggerPipelineStatus = pipelineManager.AddToQueue(pipelineInfo);
            return triggerPipelineStatus;
        }
    }
}
