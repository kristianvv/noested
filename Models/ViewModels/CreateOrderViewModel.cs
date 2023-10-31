using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Noested.Models
{
    public class CreateOrderViewModel
    {
        public ServiceOrder? NewServiceOrder { get; set; }
        public Customer? NewCustomer { get; set; }

    }

}

