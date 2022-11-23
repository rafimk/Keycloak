using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Keycloak.Helper.Infrastructure.Services
{
    public class EisEventProcessorService : IMessageProcessor
    {
       
        private readonly IServiceScopeFactory _scopeFactory;
         private ILogger<EisEventProcessorService> _logger;



        public EisEventProcessorService(IServiceScopeFactory scopeFactory, ILogger<EisEventProcessorService> _logger)
        {
            _scopeFactory = scopeFactory;
            _logger = _logger;
        }


        public async Task Process(Payload payload, string eventType)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var payloadContent = payload.Content;

                var sourceSystemName = payload?.SourceSystemName;
                object payloadContractCommand = null;

                if (sourceSystemName is null)
                {
                    _logger.LogError("SourceSystemName is null");
                    return;
                }

                switch (sourceSystemName)
                {
                    case "EIS":
                        switch (eventType)
                        {
                            case "item.created":
                                payloadContractCommand = EISCore.Application.Util.JsonSerializerUtil.DeserializeObject<ItemContracr>(payloadContent.ToString())
                                await mediator.Send(new CreateItemCommand(payloadContent));
                                break;
                            case "item.updated":
                                await mediator.Send(new UpdateItemCommand(payloadContent));
                                break;
                            case "item.deleted":
                                await mediator.Send(new DeleteItemCommand(payloadContent));
                                break;
                            default:
                                _logger.LogInformation("Event type not found");
                                break;
                        }
                        break;
                    default:
                        _logger.LogError("SourceSystemName is not supported");
                        return;
                }

                if (payloadContractCommand is null)
                {
                    _logger.LogError("PayloadContractCommand is null");
                    return;
                }

                await mediator.Send(payloadContractCommand);
                
               
            }
        }
    }
}