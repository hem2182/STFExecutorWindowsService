using log4net.Config;
using LoggerService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: XmlConfigurator(Watch = true)]

namespace TestConsole
{
    internal class Program
    {
        //public static PipelineManager pipelineManager = null;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static void Main(string[] args)
        {
            TriggerPipeline("FOC", "FOCRegression", "Tst1", false);

            TriggerPipeline("FOC", "FOCMisc", "Tst1", false);

            TriggerPipeline("FOC", "FOCExtra", "Tst90", false);

            TriggerPipeline("FOC", "FOCExtra123", "Tst90", false);

            var queueResult = GetPipelineQueue("FOC", "FOCRegression", "Tst1");

            Thread.Sleep(TimeSpan.FromSeconds(13));
            var abortStatus = AbortPipeline("FOC", "FOCRegression", "Tst1", "FOC_FOCRegression_28Nov2019_1");
            var abortStatus1 = AbortPipeline("FOC", "FOCMisc", "Tst1", "FOC_FOCMisc_28Nov2019_1");
        }

        private static string AbortPipeline(string toolName, string pipeline, string environment, string pipelineInstanceName)
        {
            var abortPipelineStatus = PipelineManager.AbortPipeline(toolName, pipeline, environment, pipelineInstanceName);
            var newQueueList = GetPipelineQueue(toolName, pipeline, environment);
            return abortPipelineStatus;
        }

        private static List<string> GetPipelineQueue(string toolName, string pipeline, string environment)
        {
            var queueList = PipelineManager.GetPipelineQueueList(toolName, pipeline, environment);
            return queueList;
        }

        private static string RerunPipeline(string toolName, string pipeline, string environment, string pipelineInstanceName, List<string> testCaseIds = null)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment, pipelineInstanceName: pipelineInstanceName, testCasesIds: testCaseIds);
            CreateLogsDirectory(toolName, pipeline, environment, pipelineInstanceName);
            var rerunPipelineStatus = PipelineManager.AddToQueue(pipelineInfo);
            return rerunPipelineStatus;
        }

        private static void CreateLogsDirectory(string toolName, string pipeline, string environment, string pipelineInstanceName)
        {
            var logsBasePath = @"C:\Users\Hemant Sharma\Desktop\ExecutorServiceLogs";
            var logPath = Path.Combine(logsBasePath, environment, toolName, pipeline, pipelineInstanceName);
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            logger.InfoFormat("Created directory for {0}_{1}_{2}_{3}", toolName, pipeline, environment, pipelineInstanceName);
        }

        private static string TriggerPipeline(string toolName, string pipeline, string environment, bool updatePlan)
        {
            PipelineInfo pipelineInfo = new PipelineInfo(toolName, pipeline, environment, updatePlan);

            CreateLogsDirectory(toolName, pipeline, environment, pipelineInfo.PipelineInstance);

            var triggerPipelineStatus = PipelineManager.AddToQueue(pipelineInfo);
            return triggerPipelineStatus;
        }
    }
}
