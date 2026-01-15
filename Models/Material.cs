using System;
using System.Collections.Generic;

namespace demkaV130126.Models;

public partial class Material
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Countinpack { get; set; }

    public string Unit { get; set; } = null!;

    public decimal? Countinstock { get; set; }

    public decimal Mincount { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }

    public string? Image { get; set; }

    public int Materialtypeid { get; set; }

    public virtual ICollection<MaterialCountHistory> MaterialCountHistories { get; set; } = new List<MaterialCountHistory>();

    public virtual ICollection<MaterialSupplier> MaterialSuppliers { get; set; } = new List<MaterialSupplier>();

    public virtual Materialtype Materialtype { get; set; } = null!;

    public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();
}
