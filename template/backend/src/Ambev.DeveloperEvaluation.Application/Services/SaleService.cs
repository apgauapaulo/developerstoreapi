using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleService : ISaleService
    {
        // Discount rule: 10% discount for purchases with 10 or more items, for example
        public async Task<decimal> CalculateDiscountAsync(Sale sale)
        {
            if (sale.Items == null || sale.Items.Count == 0)
                return 0;

            var totalItems = sale.Items.Sum(i => i.Quantity);
            decimal discount = 0;

            if (totalItems >= 10)
            {
                discount = 0.1m; // 10% discount for purchases with 10 or more items
            }

            return discount;
        }

        // Validation rule: Prevent sales with more than 20 items
        public async Task<bool> ValidateSaleAsync(Sale sale)
        {
            if (sale.Items == null || sale.Items.Count == 0)
                return false;

            var totalItems = sale.Items.Sum(i => i.Quantity);

            // Prevent sales with more than 20 items
            if (totalItems > 20)
            {
                return false;
            }

            // Prevent discounts if the total number of items is less than 4
            if (sale.Items.Count < 4)
            {
                return false;
            }

            return true;
        }
    }
}
