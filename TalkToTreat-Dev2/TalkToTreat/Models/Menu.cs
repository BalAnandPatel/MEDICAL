using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreat.Models
{
    public class Menu
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsActive { get; set; }
        public List<Menu> Items { get; set; }
    }
}