using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace demkaV130126.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Producttypeid { get; set; }

    public string Articlenumber { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }
    public Bitmap ImageBitmap => new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "products/" + Image);

    public int? Productionpersoncount { get; set; }

    public int? Productionworkshopnumber { get; set; }

    public decimal Mincostforagent { get; set; }
    
    public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; } = new List<ProductCostHistory>();

    public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public virtual Producttype Producttype { get; set; } = null!;
}
