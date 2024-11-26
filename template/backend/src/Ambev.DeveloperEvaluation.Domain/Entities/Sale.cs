using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public string Branch { get; set; }
        public bool IsCancelled { get; set; }

        // Relationship with the sale items
        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

        public void RecalculateTotalAmount()
        {
            TotalAmount = Items.Sum(item => item.TotalAmount);
        }
    }
}
