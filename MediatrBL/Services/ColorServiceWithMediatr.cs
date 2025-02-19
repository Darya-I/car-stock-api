using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStockBLL.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.ColorQueries;

namespace MediatrBL.Services
{
    public class ColorServiceWithMediatr : IColorService
    {
        /// <summary>
        /// Экземпляр MediatR для отправки команд и запросов.
        /// </summary>
        private readonly IMediator _mediator;

        public ColorServiceWithMediatr(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Color>> GetAllColorsAsync()
        {
            return await _mediator.Send(new GetAllColorsQuery());
        }

        public Task<Color> GetColorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Color> GetColorByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }
    }
}
