using System;
using System.Collections.Generic;

namespace WebStore.Models;

public partial class ProductLine
{
    public int LineId { get; set; }

    public string LineName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
