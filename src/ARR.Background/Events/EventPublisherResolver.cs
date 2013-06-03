using Quartz.Spi;

namespace ARR.Background.Events
{
    public interface IEventPublisherResolver : IJobFactory
    {
        EventPublisherJobFactory EventPublisherJobFactory { get; }
    }

    public class EventPublisherResolver : IEventPublisherResolver
    {
        public EventPublisherResolver(EventPublisherJobFactory publisherFactory)
        {
            EventPublisherJobFactory = publisherFactory;
        }

        public EventPublisherJobFactory EventPublisherJobFactory { get; private set; }

        public Quartz.IJob NewJob(TriggerFiredBundle bundle, Quartz.IScheduler scheduler)
        {
            return EventPublisherJobFactory.Instance;
        }

        public void ReturnJob(Quartz.IJob job)
        {
            throw new System.NotImplementedException();
        }
    }
}