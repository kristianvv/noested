using System.Net.NetworkInformation;
using Noested.Models;

namespace Noested.Services
{
    public class ChecklistService
    {
        public static async Task PopulateChecklistFromForm(ServiceOrderModel order, IFormCollection? form)
        {
            if (order == null || form == null)
            {
                throw new ArgumentNullException("Order/Form CANNOT be null");
            }
                
            order.Checklists = new ChecklistDTO(); // Initialize new checklist

            foreach (var key in form.Keys) // Find all item_ keys in the form to populate the checklist items
            {
                if (key.StartsWith("item_"))
                {
                    // _logger.LogInformation($"Processing form key: {key}");
                    await PopulateItemFromFormKey(order, form, key);
                }
            }
        }

        private static async Task PopulateItemFromFormKey(ServiceOrderModel order, IFormCollection form, string key)
        {
            System.Diagnostics.Debug.WriteLine($"FORM KEY: {key}"); // Lage Item Basert På Denne Nøkkelen

            var parts = key.Split(new[] { "_category_" }, StringSplitOptions.None); // dele stringen
            System.Diagnostics.Debug.WriteLine($"PARTS: '{parts[0]}' og '{parts[1]}'");

            string itemName = ExtractItemName(parts[0]); // første del er item navnet
            System.Diagnostics.Debug.WriteLine($"ITEM NAME {itemName}");

            string categoryName = parts.Length > 1 ? parts[1].Replace('_', ' ') : "Unknown";
            System.Diagnostics.Debug.WriteLine($"CATEGORY NAME {categoryName}"); // andre del er kategorinavnet

            string status = ExtractStatus(form, key); // value ligger i form, hentes ut med nøkkelen
            System.Diagnostics.Debug.WriteLine($"STATUS: {status}"); //


            var categoryObj = FindOrCreateCategory(order.Checklists, categoryName);

            // Create a new item and add it to the category
            var item = new Item { Name = itemName, Status = status };
            categoryObj.Items.Add(item);

            await Task.CompletedTask;
        }

        private static string ExtractItemName(string key)
        {
            return key.Substring(5).Replace('_', ' '); // Remove "item_" prefix and replace underscores with spaces
        }

        private static string ExtractStatus(IFormCollection form, string key)
        {
            return form[key].ToString() ?? "Unknown";
        }

        private static Category FindOrCreateCategory(ChecklistDTO checklists, string categoryName)
        {
            var categoryObj = checklists.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (categoryObj == null)
            {
                categoryObj = new Category { Name = categoryName };
                checklists.Categories.Add(categoryObj);
                System.Diagnostics.Debug.WriteLine($"CATEGORY SAVED IN CHECKLISTS.CATEGORY.NAME {categoryObj.Name}");

            }
            return categoryObj;
        }
    }
}
