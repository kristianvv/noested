using Noested.Controllers;
using Noested.Models;
using Noested.Models.DTOs;

namespace Noested.Services
{
	public class ChecklistService
	{
        private readonly ILogger<ServiceOrderController> _logger;

        public ChecklistService(ILogger<ServiceOrderController> logger)
		{
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="form"></param>
        public static async Task PopulateChecklistFromForm(ServiceOrderModel order, IFormCollection? form)
        {
            order.Checklists = new ChecklistDTO();
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

                    await Task.CompletedTask;
                }
            }
        }
    }
}