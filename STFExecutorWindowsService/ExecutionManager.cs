using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STFExecutorWindowsService
{
    internal class ExecutionManager
    {
        List<PipelineInfo> PipelineQueue = new List<PipelineInfo>();

        public ExecutionManager(PipelineInfo pipelineInfo)
        {
            //Task.Run(() => { TriggerPipeline(); });
        }

        private async void TriggerPipeline(PipelineInfo pipelineInfo)
        {
            pipelineInfo.Status = "Running";
            Task task = new Task(StartPipelineExecution);
            await task;
        }

        private void StartPipelineExecution()
        {
            while (PipelineQueue.Count > 0)
            {
                Thread t1 = new Thread(StartExecution);
                t1.Start();
            }
        }

        private void StartExecution()
        {
            Thread.Sleep(TimeSpan.FromMinutes(5));
        }

        public string AddToQueue(PipelineInfo pipelineInfo)
        {

            if (PipelineQueue.Any(x => x.ConfigKey.Equals(pipelineInfo.ConfigKey)))
            {
                return "AlreadyInQueue";
            }
            PipelineQueue.Add(pipelineInfo);
            if (PipelineQueue.Where(x => x.Status == "Running").Count() == 0)
            {
                var task = Task.Run(() => { TriggerPipeline(PipelineQueue.FirstOrDefault()); });
            }
            return "AddedToQueue";
        }
        public List<string> GetQueueList()
        {
            return PipelineQueue.Select(x => x.PipelineInstance).ToList();
        }

        internal string AbortPipeline(string pipelineInstanceName)
        {
            var abortPipeineInfo = PipelineQueue.Where(x => x.PipelineInstance == pipelineInstanceName).FirstOrDefault();
            if (abortPipeineInfo == null)
                return "PipelineInstanceNotFound";
            PipelineQueue.Remove(abortPipeineInfo);
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