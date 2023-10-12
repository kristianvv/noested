using Noested.Models;

namespace Noested.Data
{
    public interface IServiceOrderRepository
    {
        List<ServiceOrderModel> GetAllServiceOrders();
        ServiceOrderModel? GetOrderById(int id);
        void AddServiceOrder(ServiceOrderModel newOrder);
        void UpdateOrder(ServiceOrderModel updatedOrder);
    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ServiceOrderDatabase _database; // Mock database to be replaced

        public ServiceOrderRepository(ServiceOrderDatabase database)
        {
            _database = database;
        }

        public List<ServiceOrderModel> GetAllServiceOrders()
        {
            return (List<ServiceOrderModel>)_database.GetAllServiceOrders();
        }

        public ServiceOrderModel? GetOrderById(int id)
        {
            return _database.GetOrderById(id);
        }

        public void AddServiceOrder(ServiceOrderModel newOrder)
        {
            _database.AddServiceOrder(newOrder);
        }

        // Update Existing Service Orders
        public void UpdateOrder(ServiceOrderModel updatedOrder)
        {
            _database.UpdateOrder(updatedOrder);
        }
    }
}
