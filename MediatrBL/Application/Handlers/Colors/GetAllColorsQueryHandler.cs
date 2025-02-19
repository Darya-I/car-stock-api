using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.ColorQueries;

namespace MediatrBL.Application.Handlers.Colors
{
    public class GetAllColorsQueryHandler : IRequestHandler<GetAllColorsQuery, List<Color>>
    {
        private IColorRepository<Color> _colorRepository;

        public GetAllColorsQueryHandler(IColorRepository<Color> colorRepository) 
        {
            _colorRepository = colorRepository;
        }
        public async Task<List<Color>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
        {
            var colors = await _colorRepository.GetAllColorsAsync();

            if (colors == null) 
            {
                throw new EntityNotFoundException();
            }
            return colors;
        }
    }
}
