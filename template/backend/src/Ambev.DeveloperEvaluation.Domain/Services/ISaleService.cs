﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleService
    {
        Task<decimal> CalculateDiscountAsync(Sale sale);
        Task<bool> ValidateSaleAsync(Sale sale);
    }
}