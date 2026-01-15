using System;
using System.Collections.Generic;

namespace demkaV130126.Models;

public partial class MaterialSupplier
{
    public int Id { get; set; }

    public int Materialid { get; set; }

    public int Supplierid { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
