using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMposter.Model.Task
{
    public delegate void TaskChainElementActionDelegate(Object targetElement);

    public class TaskChainElement
    {
        public bool IsPerformed { get; set; }
        public TimeSpan PeriodOffset { get; set; }
        public TaskChainElementActionDelegate Action { get; set; }

        public TaskChainElement(TaskChainElementActionDelegate Action, int PeriodOffsetMiliseconds)
        {
            IsPerformed = false;
            this.Action = Action;
            this.PeriodOffset = TimeSpan.FromMilliseconds(PeriodOffsetMiliseconds);
        }
    }
}
