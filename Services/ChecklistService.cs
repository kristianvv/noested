using Noested.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Noested.Services
{
    public class ChecklistService
    {


        /// <summary>
        ///     Brukes for å behandle ferdigutfylt sjekkliste fra mekaniker.
        /// </summary>
        /// <param name="order">Serviceordren som ble hentet fra DB</param>
        /// <param name="form">Den ferdigutfylte sjekklisten generert av JS-filen</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task PopulateChecklistFromForm(ServiceOrderModel order, IFormCollection? form)
        {

            if (order == null || form == null)
            {
                throw new ArgumentNullException("Order/Form CANNOT be null");
            }

            order.Checklists = new ChecklistDTO(); // Initialisere ny sjekkliste

            foreach (var key in form.Keys) // Finne alle item_ nøkler i formen for å fylle ut sjekkelementene i lista
            {
                if (key.StartsWith("item_"))
                {
                    await PopulateItemFromFormKey(order, form, key);
                }
            }
        }

        // Hjelpemetode for å fylle sjekklisteelementene basert på item_ nøkler fra formen
        private static async Task PopulateItemFromFormKey(ServiceOrderModel order, IFormCollection form, string key)
        {
            // Hente ut elementnøkkel, kategorinøkkel og status fra formen
            string itemName = ExtractItemName(key); // Hjelpemetode under (1)
            string category = ExtractCategory(form, itemName); // Hjelpemetode under (2)
            string status = ExtractStatus(form, key); // Hjelpemetode under (3)

            // Finne eller opprette kategori for å legge element til
            var categoryObj = FindOrCreateCategory(order.Checklists, category);

            // Lage nytt element og legg til i kategori
            var item = new Item { Name = itemName, Status = status };
            categoryObj.Items.Add(item);

            await Task.CompletedTask;
        }

        // Hjelpemetode (1) – Hente navn fra formnøkkel.
        private static string ExtractItemName(string key)
        {
            return key.Substring(5).Replace('_', ' '); // "item_"
        }

        // Hjelpemetode (2) – Utvinne kategorinavnet assosiert med et item_ fra formen
        private static string ExtractCategory(IFormCollection form, string itemName)
        {
            string categoryKey = $"category_{itemName.Replace(' ', '_')}";
            return form.ContainsKey(categoryKey) ? form[categoryKey].ToString() : "Unknown";
        }

        // Hjelpemetode (3) – Utvinne status (radio) for elementet 
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
            }
            return categoryObj;
        }
    }
}
