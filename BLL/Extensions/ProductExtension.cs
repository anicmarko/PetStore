using BLL.DTOs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class ProductExtension
    {

        public static ProductEntity ToEntity(this CreateUpdateProductDTO dto)
        {
            return new ProductEntity
            {
                Brand = dto.Brand,
                Title = dto.Title,
                Price = dto.Price
            };
        }
    }
}
