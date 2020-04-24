using CQELight.Abstractions.EventStore;
using System;

namespace CQELight_Benchmarks.Models
{
    partial class TestAggregate : EventSourcedAggregate<Guid, TestAggregateState>
    {

        #region Ctor

        public TestAggregate()
        {
            State = new TestAggregateState();
        }
        
        #endregion

    }
}
