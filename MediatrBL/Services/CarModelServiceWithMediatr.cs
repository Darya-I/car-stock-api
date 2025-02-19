using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.CarModelQueries;

namespace MediatrBL.Services
{
    public class CarModelServiceWithMediatr : ICarModelService
    {
        /// <summary>
        /// Экземпляр MediatR для отправки команд и запросов.
        /// </summary>
        private readonly IMediator _mediator;

        public CarModelServiceWithMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<List<CarModel>> GetCarModelByBrandIdAsync(int id)
        {
            return await _mediator.Send(new GetCarModelByBrandIdQuery(id));
        }

        public Task<CarModel> GetCarModelByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CarModel> GetCarModelByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }
    }
}
