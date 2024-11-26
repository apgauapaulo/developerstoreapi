using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface IEventLoggingService
    {
        void LogVendaCriada(Guid saleId);
        void LogVendaModificada(Guid saleId);
        void LogVendaCancelada(Guid saleId);
        void LogItemCancelado(Guid saleId, Guid itemId);
    }

    public class EventLoggingService : IEventLoggingService
    {
        private readonly ILogger _logger;

        public EventLoggingService()
        {
            _logger = Log.ForContext<EventLoggingService>(); // Contextualizes the log for this class
        }

        public void LogVendaCriada(Guid saleId)
        {
            _logger.Information("Sale created with ID: {SaleId}", saleId);
        }

        public void LogVendaModificada(Guid saleId)
        {
            _logger.Information("Sale modified with ID: {SaleId}", saleId);
        }

        public void LogVendaCancelada(Guid saleId)
        {
            _logger.Information("Sale cancelled with ID: {SaleId}", saleId);
        }

        public void LogItemCancelado(Guid saleId, Guid itemId)
        {
            _logger.Information("Item with ID: {ItemId} cancelled in sale with ID: {SaleId}", itemId, saleId);
        }
    }
}
