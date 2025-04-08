using System;
using System.Collections.Generic;

namespace WebStore.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UId { get; set; }

    public DateOnly OrdDate { get; set; }

    public string OrdStatus { get; set; } = null!;

    public int? OrdTotal { get; set; }

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    public virtual User UIdNavigation { get; set; }
}
