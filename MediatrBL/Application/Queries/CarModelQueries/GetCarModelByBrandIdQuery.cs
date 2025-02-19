using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Queries.CarModelQueries
{
    public record GetCarModelByBrandIdQuery(int id) : IRequest<List<CarModel>>;
}
