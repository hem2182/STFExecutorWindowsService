using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace STFExecutorWindowsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISTFExecutorService
    {
        string TriggerPipeline(string toolName, string Pipeline, string environment, bool updatePlan);

        string RerunPipeline(string toolName, string Pipeline, string environment, string PipelineInstanceName, List<string>testCaseIds);

        string AbortPipeline(string toolName, string Pipeline, string environment, string PipelineInstanceName);

        List<string> GetPipelineQueue(string toolName, string Pipeline, string environment);
    }
}
