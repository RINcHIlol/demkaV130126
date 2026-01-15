using System;
using System.Collections.Generic;

namespace demkaV130126.Models;

public partial class ProductMaterial
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int MaterialId { get; set; }

    public decimal? Count { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
