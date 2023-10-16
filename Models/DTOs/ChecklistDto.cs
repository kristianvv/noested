using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noested.Models.DTOs
{
    public class ChecklistDto
    {
        public List<Category> Categories { get; set; }
        public ChecklistDto()
        {
            Categories = new List<Category>();
        }
    }

    //
    public class Category
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }

        public Category()
        {
            Name = "";
            Items = new List<Item>();
        }
    }

    //
    public class Item
    {
        public string Name { get; set; }
        public string Status { get; set; } // OK, Bør Skiftes, Defekt

        public Item()
        {
            Name = "";
            Status = "";
        }
    }
}