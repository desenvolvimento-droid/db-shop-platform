using MediatR;
using Shop.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Base;

public abstract class IdempotentEventHandler<TEvent, TReadModel>
    : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
    where TReadModel : ReadModelBase, new()
{
    private readonly IReadModelRepository<TReadModel> _repository;

    protected IdempotentEventHandler(IReadModelRepository<TReadModel> repository)
    {
        _repository = repository;
    }

    public async Task Handle(TEvent @event, CancellationToken cancellationToken)
    {
        var readModel = await _repository.GetAsync(GetId(@event));

        if (readModel == null)
        {
            readModel = new TReadModel();
            readModel.Id = GetId(@event);
            readModel.LastEventVersion = -1;
        }

        if (readModel.LastEventVersion >= GetVersion(@event))
            return;

        await ApplyAsync(readModel, @event, cancellationToken);

        readModel.LastEventVersion = GetVersion(@event);

        await _repository.UpsertAsync(readModel);
    }

    protected abstract Guid GetId(TEvent @event);
    protected abstract long GetVersion(TEvent @event);

    protected abstract Task ApplyAsync(TReadModel readModel, TEvent @event, CancellationToken ct);
}

