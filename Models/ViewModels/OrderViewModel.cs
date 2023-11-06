using System.Reflection;

namespace Noested.Models
{
    public class OrderViewModel
    {
        public ServiceOrder? FillOrder { get; set; }
        public Customer? NewCustomer { get; set; }
        public Checklist? NewChecklist { get; set; }
    }
}
