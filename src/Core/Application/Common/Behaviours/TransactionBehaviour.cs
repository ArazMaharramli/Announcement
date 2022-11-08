using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public TransactionBehaviour(
            IDbContext dbContext,
            ILogger<TransactionBehaviour<TRequest, TResponse>> logger,
            IEventBusService eventBusService)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(IDbContext));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
            _eventBusService = eventBusService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = typeof(TRequest).Name;

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

                    _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await _dbContext.CommitTransactionAsync(transaction, cancellationToken);

                    transactionId = transaction.TransactionId;

                });

                await _eventBusService.PublishAsync();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }
    }
}
