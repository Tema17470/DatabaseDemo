using System;
using System.Collections.Generic;

namespace WebStore.Models;

public partial class Product
{
    public int ProdId { get; set; }

    public int? LineId { get; set; }

    public string ProdDesc { get; set; } = null!;

    public int ProdPrice { get; set; }

    public string ProdCategory { get; set; } = null!;

    public virtual ProductLine? Line { get; set; }

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();
}
