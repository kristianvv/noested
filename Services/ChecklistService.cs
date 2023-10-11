using Noested.Controllers;
using Noested.Models;
using Noested.Models.DTOs;
using Noested.Data;

namespace Noested.Services
{
    public class ChecklistService
    {
        private readonly ILogger<ServiceOrderController> _logger;

        public ChecklistService(ILogger<ServiceOrderController> logger)
        {
            _logger = logger;
        }


        public void PopulateChecklistFromForm(ServiceOrderModel order, IFormCollection form)
        {
            _logger.LogInformation($"Received form data: {form}");
            if (order == null || form == null)
            {
                throw new ArgumentException("Order or form cannot be null.");
            }

            order.Checklists = new ChecklistDto();
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("item_"))
                {
                    string itemName = key.Substring(5).Replace('_', ' ');
                    string categoryKey = $"category_{itemName.Replace(' ', '_')}";
                    string category = form.ContainsKey(categoryKey) ? form[categoryKey].ToString() : "Unknown";
                    string status = form[key].ToString() ?? "Unknown";

                    var categoryObj = order.Checklists.Categories.FirstOrDefault(c => c.Name == category);
                    if (categoryObj == null)
                    {
                        categoryObj = new Category { Name = category };
                        order.Checklists.Categories.Add(categoryObj);
                    }

                    var item = new Item { Name = itemName, Status = status };
                    categoryObj.Items.Add(item);
                }
            }
        }
    }
}