using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WalletService.Domain.Interfaces;

namespace WalletService.Infrastructure.EventSourcing
{
    public class AggregateRepository<T> where T : class
    {
        private readonly IEventStore _eventStore;
        
        public AggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }
        
        public async Task<T> GetByIdAsync(Guid id)
        {
            var events = await _eventStore.GetEventsAsync(id);
            if (!events.Any())
                return null;
                
            var result = Activator.CreateInstance(typeof(T), true) as T;
            
            foreach (var @event in events)
            {
                ApplyEvent(result, @event);
            }
            
            return result;
        }
        
        public async Task SaveAsync(T aggregate, int expectedVersion)
        {
            var idProperty = aggregate.GetType().GetProperty("Id");
            if (idProperty == null)
                throw new InvalidOperationException("Aggregate must have an Id property");
                
            var id = (Guid)idProperty.GetValue(aggregate);
            
            var domainEventsProperty = aggregate.GetType()
                .GetProperty("DomainEvents", BindingFlags.Public | BindingFlags.Instance);
                
            if (domainEventsProperty == null)
                throw new InvalidOperationException("Aggregate must have a DomainEvents property");
                
            var events = domainEventsProperty.GetValue(aggregate) as System.Collections.IEnumerable;
            if (events == null)
                throw new InvalidOperationException("DomainEvents must be enumerable");
                
            var eventsList = events.Cast<object>().ToList();
            
            await _eventStore.SaveEventsAsync(id, eventsList, expectedVersion);
            
            var clearEventsMethod = aggregate.GetType()
                .GetMethod("ClearDomainEvents", BindingFlags.Public | BindingFlags.Instance);
                
            if (clearEventsMethod == null)
                throw new InvalidOperationException("Aggregate must have a ClearDomainEvents method");
                
            clearEventsMethod.Invoke(aggregate, null);
        }
        
        private void ApplyEvent(T aggregate, object @event)
        {
            var applyMethod = aggregate.GetType()
                .GetMethod("ApplyEvent", BindingFlags.Public | BindingFlags.Instance, null, new[] { @event.GetType() }, null);
                
            if (applyMethod == null)
                throw new InvalidOperationException($"Aggregate does not have an ApplyEvent method for {@event.GetType().Name}");
                
            applyMethod.Invoke(aggregate, new[] { @event });
        }
    }
}