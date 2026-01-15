using System;
using System.Collections.Generic;

namespace demkaV130126.Models;

public partial class AgentType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ICollection<Agent> Agents { get; set; } = new List<Agent>();
}
