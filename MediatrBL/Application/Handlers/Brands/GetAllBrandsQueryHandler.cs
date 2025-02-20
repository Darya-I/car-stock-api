using CarStockBLL.CustomException;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using MediatR;
using MediatrBL.Application.Queries.BrandQueries;

namespace MediatrBL.Application.Handlers.Brands
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, List<Brand>>
    {
        private IBrandRepository<Brand> _brandRepository;
        public GetAllBrandsQueryHandler(IBrandRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<List<Brand>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllBrandsAsync();
            if (brands == null) 
            {
                throw new EntityNotFoundException();
            }
            return brands;
        }
    }
}
