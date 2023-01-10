using MediatR;
using System;

namespace Survey.Domain.Events
{
    public class EntityUserActivatedDomainEvent : INotification
    {
        public int EntityId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Type EntityType { get; set; }

        public EntityUserActivatedDomainEvent(Type entityType, string fullName, string email, int entityId)
        {
            EntityType = entityType;
            FullName = fullName;
            Email = email;
            EntityId = entityId;
        }
    }
}