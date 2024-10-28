using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace ClothingBrand.Application.Services
{
    public class ProductService:IProductService
    {
        private readonly IUnitOfWork _unitRepository;
        private string imagesPath;
        private readonly IConfiguration _configuration;
        private readonly string ApplicationUrl;

        public ProductService(IUnitOfWork unitRepository, IConfiguration configuration)
        {
            _unitRepository = unitRepository;
            imagesPath = "/images/";
            _configuration = configuration;
            ApplicationUrl = _configuration["ApplicationUrl"];
        }
        public IEnumerable<GETProductDTO> GetAll(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
        {
            
            var iList= _unitRepository.productRepository.GetAll(filter, "Category,Discount", tracked)
                .Select(e=>new GETProductDTO
                {
                    CategoryName=e.Category.Name,
                    Name=e.Name,
                    Description=e.Description,
                    Discount= (decimal)(e.Discount!=null&&DateTime.Now<=e.Discount.EndDate&&DateTime.Now>=e.Discount.StartDate?e.Discount?.Percentage*e.Price:0),
                    Id=e.Id,
                    Price=e.Price,
                    ImageUrl= ApplicationUrl+imagesPath + e.ImageUrl,
                    ISBN=e.ISBN,
                    StockQuantity=e.StockQuantity, 

                });
          return iList;
        }
        public  IEnumerable<GETProductDTO> GetAllWithpagination(int page,int pageSize ,Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
        {

            var iList = _unitRepository.productRepository.GetAllWithPagaAsync(page , pageSize, filter, "Category,Discount", tracked)
                .Select(e => new GETProductDTO
                {
                    CategoryName = e.Category.Name,
                    Name = e.Name,
                    Description = e.Description,
                    Discount = (decimal)(e.Discount != null && DateTime.Now <= e.Discount.EndDate && DateTime.Now >= e.Discount.StartDate ? e.Discount.Percentage * e.Price : 0),
                    Id = e.Id,
                    Price = e.Price,
                    ImageUrl = ApplicationUrl + imagesPath + e.ImageUrl,
                    ISBN = e.ISBN,
                    StockQuantity = e.StockQuantity,

                });
            return iList;
        }

        public GETProductDTO GEtProduct(int id)
        {
            var product = _unitRepository.productRepository.Get((x) => x.Id == id, includeProperties: "Category,Discount");

            return new GETProductDTO
            {
                CategoryName = product.Category.Name,
                Name = product.Name,
                Description = product.Description,
                Discount = (decimal)(product.Discount != null && DateTime.Now <=product.Discount.EndDate && DateTime.Now >= product.Discount.StartDate ? product.Discount?.Percentage * product.Price : 0),
                Id = product.Id,
                Price = product.Price,
                ImageUrl = ApplicationUrl + imagesPath + product.ImageUrl,
                ISBN = product.ISBN,
                StockQuantity = product.StockQuantity,

            };
        }


        public async Task AddProduct(ProductDTO productDTO)
        {
            var product = new Product()
            {
                CategoryId = productDTO.CategoryId,
                Name = productDTO.Name,
                Description = productDTO.Description,
                ImageUrl = await GeTImageUrlAsync(productDTO.Image),
                ISBN = productDTO.ISBN,
                StockQuantity = productDTO.StockQuantity,
                Price = productDTO.Price,
                DiscountId = productDTO.DiscountId,

            };
            _unitRepository.productRepository.Add(product);
            _unitRepository.Save();
        }
        public void Remove(int id)
        {
            _unitRepository.productRepository.Remove(_unitRepository.productRepository.Get((x)=>x.Id==id));
            _unitRepository.Save();


        }
        public async Task update(int id,ProductDTO productDTO)
        {




            var product = new Product()
            {

                CategoryId = productDTO.CategoryId,
                Name = productDTO.Name,
                Description = productDTO.Description,
                ImageUrl = productDTO.Image == null ? null : await GeTImageUrlAsync(productDTO.Image),
                ISBN = productDTO.ISBN,
                StockQuantity = productDTO.StockQuantity,
                Price = productDTO.Price,
                DiscountId = productDTO.DiscountId,
                Id = id

            };
            
            _unitRepository.productRepository.Update(product);
            _unitRepository.Save();
            

        }

        private async Task<string> GeTImageUrlAsync(IFormFile image)
        {
            string UniqueName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string filepath = "wwwroot"+imagesPath + UniqueName;
            using (FileStream file = new FileStream(filepath, FileMode.Create))
            {
                await image.CopyToAsync(file);
                file.Close();
            }
            return UniqueName;

           

        }

    }
}
