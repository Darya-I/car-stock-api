using CarStockDAL.Models;
using MediatR;

namespace MediatrBL.Application.Queries.ColorQueries
{
    public record GetAllColorsQuery() : IRequest<List<Color>>;
}
