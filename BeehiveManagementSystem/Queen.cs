using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeehiveManagementSystem
{
    internal class Queen : Bee
    {
        private IWorker[] workers = new IWorker[0];
        public const float EGGS_PER_SHIFT = 0.45f;
        public const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;

        private float eggs = 0; // 蜂卵数
        private float unassignedWorkders = 3;

        public string StatusReport { get; private set; }
        public override float CostPerShift { get { return 2.15f; } }

        public Queen() : base("Queen")
        {
            AssignBee("Nectar Collector");
            AssignBee("Honey Manufacturer");
            AssignBee("Egg Care");
        }

        private void AddWorker(IWorker worker)
        {
            if (unassignedWorkders >= 1)
            {
                unassignedWorkders--;
                Array.Resize(ref workers, workers.Length + 1);
                workers[workers.Length - 1] = worker;
            }
        }

        private void UpdateStatusReport()
        {
            StatusReport = $"Vault report: \n{HoneyVault.StatusReport}\n" +
                $"\nEgg count: {eggs: 0.0}\nUnassigned workders: {unassignedWorkders: 0.0}\n" +
                $"{WorkerStatus("Nectar Collecotr")}\n{WorkerStatus("Honey Manufacturer")}" +
                $"\n{WorkerStatus("Egg Care")}\nTOTAL WORKERS: {workers.Length}";
        }

        public void CareForEggs(float eggsToConvert)
        {
            if (eggs >= eggsToConvert)
            {
                eggs -= eggsToConvert;
                unassignedWorkders += eggsToConvert;
            }
        }

        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach(IWorker workder in workers)
            {
                if (workder.Job == job) count++;
            }
            string s = "s";
            if (count == 1) s = "";
            return $"{count} {job} bee{s}";
        }
        public void AssignBee(string job)
        {
            switch (job)
            {
                case "Egg Care":
                    AddWorker(new EggCare(this));
                    break;
                case "Nectar Collector":
                    AddWorker(new NectarCollector());
                    break;
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufacturer());
                    break;
            }
            UpdateStatusReport();
        }
        protected override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;
            foreach (Bee worker in workers)
            {
                worker.WorkTheNextShift();
            }
            HoneyVault.ConsumeHoney(unassignedWorkders * HONEY_PER_UNASSIGNED_WORKER);
            UpdateStatusReport();
        }

    }
}
