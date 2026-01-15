using System;
using System.Collections.Generic;

namespace demkaV130126.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public DateOnly Startdate { get; set; }

    public int? Qualityrating { get; set; }

    public string? Suppliertype { get; set; }

    public virtual ICollection<MaterialSupplier> MaterialSuppliers { get; set; } = new List<MaterialSupplier>();
}
