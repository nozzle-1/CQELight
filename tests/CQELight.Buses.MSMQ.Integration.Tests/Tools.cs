namespace CQELight.Buses.MSMQ.Integration.Tests
{
    static class Tools
    {

        public static void CleanQueue()
        {
            var queue = Helpers.GetMessageQueue();
            queue.Purge();
        }
    }
}
