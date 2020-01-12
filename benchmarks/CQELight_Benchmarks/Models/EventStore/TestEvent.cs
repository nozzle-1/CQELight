using CQELight.Abstractions.Events;
using System;

namespace CQELight_Benchmarks.Models
{
    public class TestEvent : BaseDomainEvent
    {

        #region Properties

        public int AggregateIntValue { get; set; }
        public string AggregateStringValue { get; set; }

        #endregion

        #region Ctor

        public TestEvent(Guid id, Guid aggId)
        {
            Id = id;
            AggregateId = aggId;
            AggregateType = typeof(TestAggregate);
        }

        #endregion

    }
}