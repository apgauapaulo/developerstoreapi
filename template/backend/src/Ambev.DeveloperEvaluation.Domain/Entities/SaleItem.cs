using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; } // Foreign Key
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; set; }

    // Relationship with sale
    public Sale Sale { get; set; }

    public void CalculateTotalAmount()
    {
        TotalAmount = (UnitPrice * Quantity) - Discount;
    }
}