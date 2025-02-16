using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskSeqService
    {

        private readonly ConcurrentQueue<Func<Task>> _taskQueue = new();
        private readonly Subject<Func<Task>> _taskSubject = new();
        private bool _isProcessing = false;

        public TaskSeqService()
        {
            _taskSubject
                .Select(taskFunc => Observable.FromAsync(taskFunc))
                .Concat()
                .Subscribe(_ =>
                {
                    _isProcessing = false;
                    ProcessNextTask();
                });
        }

        public void EnqueueTask(Func<Task> taskFunc)
        {
            _taskQueue.Enqueue(taskFunc);
            ProcessNextTask();
        }

        private void ProcessNextTask()
        {
            if (_isProcessing || _taskQueue.IsEmpty)
                return;

            if (_taskQueue.TryDequeue(out var nextTask))
            {
                _isProcessing = true;
                _taskSubject.OnNext(async () => await nextTask());
            }
        }
    }
}
