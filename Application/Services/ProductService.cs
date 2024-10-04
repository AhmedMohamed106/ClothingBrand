using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;


namespace ClothingBrand.Application.Services
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IEnumerable<GETProductDTO> GEtAll()
        {
            var iList= _productRepository.GetAll(includeProperties: "Category,Discount")
                .Select(e=>new GETProductDTO
                {
                    CategoryName=e.Category.Name,
                    Name=e.Name,
                    Description=e.Description,
                    Discount= (decimal)(e.Discount!=null&&DateTime.Now<=e.Discount.EndDate&&DateTime.Now>=e.Discount.StartDate?e.Discount?.Percentage*e.Price:0),
                    Id=e.Id,
                    Price=e.Price,
                    ImageUrl=e.ImageUrl,
                    ISBN=e.ISBN,
                    StockQuantity=e.StockQuantity, 

                });
          return iList;
        }

        public GETProductDTO GEtProduct(int id)
        {
            var product = _productRepository.Get((x) => x.Id == id, includeProperties: "Category,Discount");

            return new GETProductDTO
            {
                CategoryName = product.Category.Name,
                Name = product.Name,
                Description = product.Description,
                Discount = (decimal)(product.Discount != null && DateTime.Now <=product.Discount.EndDate && DateTime.Now >= product.Discount.StartDate ? product.Discount?.Percentage * product.Price : 0),
                Id = product.Id,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                ISBN = product.ISBN,
                StockQuantity = product.StockQuantity,

            };
        }


        public void AddProduct(ProductDTO productDTO)
        {
            var product = new Product()
            {
                CategoryId = productDTO.CategoryId,
                Name = productDTO.Name,
                Description = productDTO.Description,
                ImageUrl = productDTO.ImageUrl,
                ISBN = productDTO.ISBN,
                StockQuantity = productDTO.StockQuantity,
                Price = productDTO.Price,
                DiscountId = productDTO.DiscountId,

            };
            _productRepository.Add(product);
        }
        public void Remove(int id)
        {
            _productRepository.Remove(_productRepository.Get((x)=>x.Id==id));

        }
        public void update(int id,ProductDTO productDTO)
        {
            var product = new Product()
            {
                CategoryId = productDTO.CategoryId,
                Name = productDTO.Name,
                Description = productDTO.Description,
                ImageUrl = productDTO.ImageUrl,
                ISBN = productDTO.ISBN,
                StockQuantity = productDTO.StockQuantity,
                Price = productDTO.Price,
                DiscountId = productDTO.DiscountId,
                Id = id

            };
            _productRepository.Update(product);
        }

    }
}
