using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.BrandQueries;

namespace MediatrBL.Services
{
    public class BrandServiceWithMediatr : IBrandService
    {
        /// <summary>
        /// Экземпляр MediatR для отправки команд и запросов.
        /// </summary>
        private readonly IMediator _mediator;

        public BrandServiceWithMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
           return await _mediator.Send(new GetAllBrandsQuery());
        }

        public Task<Brand> GetBrandByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Brand> GetBrandByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }
    }
}
