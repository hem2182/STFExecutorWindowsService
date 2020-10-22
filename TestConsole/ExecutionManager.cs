using log4net;
using LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class ExecutionManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ILog pipelineLogger = null;
        public List<PipelineInfo> PipelineQueue = null;

        public ExecutionManager(ILog logger, PipelineInfo pipelineInfo)
        {
            InitializeQueue();
            //logger.InfoFormat("Execution Manager started for lockName:{0}", pipelineInfo.LockName);
            //logger.Info("Triggering Pipeline Instance " + pipelineInfo.PipelineInstance);
            //pipelineInfo.Status = PipelineStatus.Running;
            //logger.Info("Setting Pipeline " + pipelineInfo.PipelineInstance + " status to Running for lock " + pipelineInfo.LockName);
            //Thread triggerThread = new Thread(() => { TriggerPipeline(pipelineInfo); });
            //logger.Info("Trigger Thread Id: " + triggerThread.ManagedThreadId + " initiated pipeline " + pipelineInfo.PipelineInstance);
            //triggerThread.Start();

        }

        private void InitializeQueue()
        {
            if (PipelineQueue == null)
                PipelineQueue = new List<PipelineInfo>();
        }

        public string AddToQueue(PipelineInfo pipelineInfo)
        {
            logger.Info("Adding Pipeline Instance " + pipelineInfo.PipelineInstance + " into QueueList for lock name " + pipelineInfo.LockName);
            if (PipelineQueue.Any(x => x.ConfigKey.Equals(pipelineInfo.ConfigKey)))
            {
                return "AlreadyInQueue";
            }
            PipelineQueue.Add(pipelineInfo);
            logger.Info("Added Pipeline Instance " + pipelineInfo.PipelineInstance + " into QueueList for lock name " + pipelineInfo.LockName);
            if (PipelineQueue.Where(x => x.Status == PipelineStatus.Running).Count() == 0)
            {
                logger.Info("Triggering Pipeline Instance " + PipelineQueue.FirstOrDefault().PipelineInstance);
                pipelineInfo.Status = PipelineStatus.Running;
                logger.Info("Setting Pipeline " + pipelineInfo.PipelineInstance + " status to Running for lock " + pipelineInfo.LockName);
                Thread triggerThread = new Thread(() =>
                {
                    TriggerPipeline(PipelineQueue.FirstOrDefault());
                });
                logger.Info("Trigger Thread Id: " + triggerThread.ManagedThreadId + " initiated pipeline " + PipelineQueue.FirstOrDefault().PipelineInstance);
                triggerThread.Start();
                //triggerThread.Join();
                //var task = Task.Run(() => { TriggerPipeline(PipelineQueue.FirstOrDefault()); });

                return "Pipeline Triggered";
            }
            return "AddedToQueue";
        }

        private void TriggerPipeline(PipelineInfo pipelineInfo)
        {
            while (PipelineQueue.Count > 0)
            {
                var loggerName = string.Join("_", PipelineQueue.FirstOrDefault().ConfigKey, PipelineQueue.FirstOrDefault().LockName);
                pipelineLogger = LoggerBase.GetLogger(loggerName, PipelineQueue.FirstOrDefault().ConfigData.Environment, PipelineQueue.FirstOrDefault().ConfigData.ToolName,
                    PipelineQueue.FirstOrDefault().ConfigData.Pipeline, PipelineQueue.FirstOrDefault().PipelineInstance);

                pipelineLogger.InfoFormat("Logger instance created for {0}_{1}_{2}_{3} with name: {4}", PipelineQueue.FirstOrDefault().ConfigData.Environment,
                    PipelineQueue.FirstOrDefault().ConfigData.ToolName, PipelineQueue.FirstOrDefault().PipelineInstance, PipelineQueue.FirstOrDefault().ConfigData.Pipeline, loggerName);

                pipelineLogger.Info("Started Execution for " + PipelineQueue.FirstOrDefault().LockName + "=>" + PipelineQueue.FirstOrDefault().PipelineInstance);
                PipelineQueue.FirstOrDefault().Status = PipelineStatus.Running;
                StartExecution();
            }
        }

        private void StartExecution()
        {
            int counter = 10;
            while (counter != 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                pipelineLogger.InfoFormat("Pipeline {0}_{1} is still running.", PipelineQueue.FirstOrDefault().ConfigKey, PipelineQueue.FirstOrDefault().PipelineInstance);
                if (PipelineQueue.FirstOrDefault().Status == PipelineStatus.AbortRequested)
                {
                    pipelineLogger.Info("Pipeline " + PipelineQueue.FirstOrDefault().LockName + "=>" + PipelineQueue.FirstOrDefault().PipelineInstance + " has requested to Abort.");
                    pipelineLogger.Info("Aborted Pipeline " + PipelineQueue.FirstOrDefault().LockName + "=>" + PipelineQueue.FirstOrDefault().PipelineInstance);
                    break;
                }
                counter--;
            }
            pipelineLogger.Info("Execution Complete for Pipeline Instance " + PipelineQueue.FirstOrDefault().LockName + "=>" + PipelineQueue.FirstOrDefault().PipelineInstance);
            PipelineQueue.RemoveAt(0);
        }
        public List<string> GetQueueList()
        {
            return PipelineQueue.Select(x => x.PipelineInstance + "->" + x.Status).ToList();
        }

        internal string AbortPipeline(string pipelineInstanceName)
        {
            var abortPipeineInfo = PipelineQueue.Where(x => x.PipelineInstance == pipelineInstanceName).FirstOrDefault();
            if (abortPipeineInfo == null)
                return "PipelineInstanceNotFound";
            if (abortPipeineInfo.Status == PipelineStatus.Running)
                abortPipeineInfo.Status = PipelineStatus.AbortRequested;
            else
            {
                PipelineQueue.Remove(abortPipeineInfo);
                logger.Info("Aborted Pipeline " + abortPipeineInfo.LockName + "=>" + abortPipeineInfo.PipelineInstance);
                logger.Info("Pipeline " + abortPipeineInfo.LockName + "=>" + abortPipeineInfo.PipelineInstance + " removed from Queue as it has not yet started.");
            }

            if (PipelineQueue.Any(x => x.PipelineInstance == pipelineInstanceName))
            {
                return "FailedToAbortPipeline";
            }
            else
            {
                return "AbortSuccessfull";
            }
        }
    }
}