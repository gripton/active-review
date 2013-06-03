using System;
using Quartz;
using Quartz.Impl;

namespace ARR.Background.Events
{
    public interface IEventPublisherScheduler : IDisposable
    {
        void Start();
        void ShutDown();
    }

    public class EventPublisherScheduler : IEventPublisherScheduler
    {
        private IScheduler _scheduler;
        private readonly IEventPublisherResolver _eventPublisherResolver;

        public EventPublisherScheduler(IEventPublisherResolver eventPublisherResolver)
        {
            _eventPublisherResolver = eventPublisherResolver;
        }

        public void Start()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            _scheduler = schedFact.GetScheduler();
            _scheduler.JobFactory = _eventPublisherResolver;
            _scheduler.Start();

            var detail = JobBuilder
                .Create<EventPublisherJobFactory.EventPublisher>()
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .ForJob(detail)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
                .StartNow()
                .Build();

            _scheduler.ScheduleJob(detail, trigger);
        }

        public void ShutDown()
        {
            _scheduler.Shutdown();
        }
        
        public void Dispose()
        {
            _scheduler.Shutdown();
        }
    }
}