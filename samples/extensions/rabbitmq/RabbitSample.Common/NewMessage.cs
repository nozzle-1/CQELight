using CQELight.Abstractions.Events;

namespace RabbitSample.Common
{
    public class NewMessage : BaseDomainEvent
    {
        #region Properties

        public string Payload { get; set; }

        #endregion
    }
}
