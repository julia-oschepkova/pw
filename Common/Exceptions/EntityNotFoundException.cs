using System;

namespace Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid id, Type entityType) : base($"Entity of type {entityType.Name} with id {id} was not found")
        {
            Id = id;
            EntityType = entityType;
        }

        public EntityNotFoundException(string message, Type entityType) : base(message + $"Entity of type {entityType.Name}")
        {
            EntityType = entityType;
        }

        public Guid Id { get; private set; }

        public Type EntityType { get; private set; }
    }
}