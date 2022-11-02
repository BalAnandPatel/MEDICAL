using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalkToTreatService.Entities
{    
    public class Product
    {
        public string StockId { get; set; }
        public string SKU { get; set; }
        public string ParentSKU { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public ItemTypes ItemType { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string ColorHex { get; set; }
        public string ColorText { get; set; }
        public string Size { get; set; }
        public string Currency { get; set; }
        public bool Published { get; set; }
        public decimal CostPrice { get; set; }
        public decimal ListPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal LengthMM { get; set; }
        public decimal WidthMM { get; set; }
        public decimal HeightMM { get; set; }
        public long CurrentStock { get; set; }
        public decimal Weight { get; set; }
        public string Gender { get; set; }
        public string CategoryId { get; set; }
        public string  ParentCategory { get; set; }
        public string TeamName { get; set; }

        public string BrandId { get; set; }
        public string SubBrandId { get; set; }
        public string SubBrandName { get; set; }
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
        public string UOM { get; set; }
        public string BarCode { get; set; }
        public bool IsTaxable { get; set; }
        public int ItemGroupID { get; set; }
        public string ItemGroup { get; set; }
        public bool Visible { get; set; }
    }

    public enum ItemTypes
    {
        SimpleProduct = 1,
        //Service = 2,
        Variant = 6,
        //Virtual = 5,
        Bundle = 7,
        FixedSubscription = 8,
        VariableSubscription = 9,
        Addon = 10
    }
}