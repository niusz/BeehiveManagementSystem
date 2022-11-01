using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeehiveManagementSystem
{
    abstract class Bee : IWorker
    {
        public string Job { get; private set; }
        public abstract float CostPerShift { get; }
        public Bee(string job)
        {
            Job = job;
        }
        protected abstract void DoJob();
        public void  WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift)) DoJob(); 
        }
    }
}
