using System;
using System.Collections.Generic;

namespace demkaV130126.Models;

public partial class MaterialCountHistory
{
    public int Id { get; set; }

    public int MaterialId { get; set; }

    public DateTime ChangeDate { get; set; }

    public decimal CountValue { get; set; }

    public virtual Material Material { get; set; } = null!;
}
