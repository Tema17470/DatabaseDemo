using System;
using System.Collections.Generic;

namespace WebStore.Models;

public partial class User
{
    public int UId { get; set; }

    public string UFirstname { get; set; } = null!;

    public string ULastname { get; set; } = null!;

    public string UEmail { get; set; } = null!;

    public string UAddress { get; set; } = null!;

    public string URegion { get; set; } = null!;

    public int UPostalcode { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
