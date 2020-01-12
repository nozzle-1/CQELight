using CQELight.Abstractions.CQS.Interfaces;

namespace CQELight_Benchmarks.Models
{
    public class TestCommand : ICommand
    {
        public TestCommand(int i, bool simulateWork, int jobDuration)
        {
            I = i;
            SimulateWork = simulateWork;
            JobDuration = jobDuration;
        }

        public int I { get; }
        public bool SimulateWork { get; }
        public int JobDuration { get; }
    }
}
