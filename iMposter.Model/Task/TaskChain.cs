using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace iMposter.Model.Task
{
    public class TaskChain
    {
        public IList<TaskChainElement> TaskList { get; set; }
        public Object TargetElement { get; set; }
        protected IEnumerator<TaskChainElement> taskListEnumerator;

        public TaskChain(Object TargetElement)
        {
            this.TaskList = new List<TaskChainElement>();
            this.TargetElement = TargetElement;
        }

        public void Execute()
        {
            taskListEnumerator = TaskList.GetEnumerator();
            ExecuteNext();
        }

        protected void ExecuteNext()
        {
            if (taskListEnumerator.MoveNext())
            {
                TaskChainElement taskChainElement = taskListEnumerator.Current;
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = taskChainElement.PeriodOffset;
                dispatcherTimer.Tick += new EventHandler(ScheduleNextTaskExecution);
                dispatcherTimer.Tag = taskChainElement;
                dispatcherTimer.Start();
            }
            else
            {
                // tmp 
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        protected void ScheduleNextTaskExecution(object sender, EventArgs e)
        {
            DispatcherTimer dispatcherTimer = sender as DispatcherTimer;
            TaskChainElement taskChainElement = dispatcherTimer.Tag as TaskChainElement;
            dispatcherTimer.Stop();
            if (!taskChainElement.IsPerformed)
            {
                taskChainElement.IsPerformed = true;
                taskChainElement.Action(TargetElement);
                ExecuteNext();
            }
        }
    }
}
