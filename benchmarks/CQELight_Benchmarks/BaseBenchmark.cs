using BenchmarkDotNet.Attributes;
using System;

namespace CQELight_Benchmarks
{
    public abstract class BaseBenchmark
    {

        [Params(1000)]
        public int N;

        public Guid AggregateId = Guid.NewGuid();

        protected Random _random = new Random();


    }
}
