using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Noested.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noested.Models.DTOs
{

    public class CompletedOrderDto
    {
        public ServiceOrderModel? CompletedOrder { get; set; }
        public IFormCollection? Form { get; set; }
    }

}