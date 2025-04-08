using System;
using System.Collections.Generic;

namespace WebStore.Models;

public partial class OrderedProduct
{
    public int ListId { get; set; }

    public int? OrdId { get; set; }

    public int? ProdId { get; set; }

    public virtual Order? Ord { get; set; }

    public virtual Product? Prod { get; set; }
}
