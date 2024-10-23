using System.Security.Claims;
using Application.DTOs.Request.Management;
using Application.Interfaces.Managements;
using Application.ViewModels.Management;
using Domain.Models.Management;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation.Management
{
    public class ProductService : IProductService
    {
        private readonly BGSDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(BGSDbContext context, IHttpContextAccessor http, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _http = http;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task CreateProduct(CreateProductRequest model)
        {
            try
            {
                var currentUserId = _http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(currentUserId == null)
                {
                    throw new Exception("User is not loged in");
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName!.Equals(model.ProductName));
                if(product != null)
                {
                    throw new Exception("The product is existed");
                }

                if(model.PicturePaths !=null & model.PicturePaths.Any())
                {
                    //string stringFileName = UploadFile(model);
                    var newProduct = new Product()
                    {
                        ProductName = model.ProductName,
                        Pictures = await UploadFile(model.PicturePaths),
                        Description = model.Description,
                        ProductPrice = model.ProductPrice,
                        UserId = currentUserId, 
                        DiscountProductprice = model.DiscountProductprice, 
                        CategoryId = model.CategoryId,
                        IsFeatured = model.IsFeatured == false
                    };
                    _context.Products.Add(newProduct);
                }                                

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public Task DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditProduct(int id, CreateProductRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OldOrNewProducts>> GetAllOldOrNewProducts()
        {
            return await _context.Products.Include(i => i.User).Select(p => new OldOrNewProducts()
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Pictures = p.Pictures,
                ProductPrice = p.ProductPrice,
                Description = p.Description,
                CreatedDate = p.CreatedDate, 
                DiscountProductprice = p.DiscountProductprice, 
                IsFeatured = p.IsFeatured,
            }).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.Include(i => i.User).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllRelatedProducts(int productId, int count)
        {
            var product = await GetProductById(productId);
            return product.Category!.Products.Where(p => p.Id != productId).Take(count).ToList();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(p => p.Category).ThenInclude(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
        }

        #region
        //private  string UploadFile(IFormFile file)
        //{
        //    string fileName = string.Empty; 
        //    if(file != null)
        //    {
        //        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
        //        fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
        //        string filePath = Path.Combine(uploadDir, fileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            file.CopyTo(fileStream);
        //        }
        //    }            
        //    return fileName!;
        //}

        private async Task<string> UploadFile(IEnumerable<IFormFile> files)
        {
                var filePaths = new List<string>();
                foreach(var file in files)
                {
                    if(file.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var extension = Path.GetExtension(file.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uniqueFileName); 
                        using(var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        filePaths.Add($"/uploads/{uniqueFileName}");
                    }                    
                }
            return string.Join(",", filePaths);
        }

        //private string UploadFileForEdit(EditProductRequest model)
        //{
        //    string wwwRootPath = _webHostEnvironment.WebRootPath;
        //    var extension = Path.GetFileName(model.PicturePaths!.FileName);
        //    string nameFile = model.Name!.ToLower();
        //    string fileName = nameFile + extension;

        //    var addPicture = new Product() { Pictures = fileName };

        //    string userPath = wwwRootPath + "/Images/";

        //    string path = Path.Combine(userPath, fileName);
        //    using (var fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        model.PicturePaths.CopyTo(fileStream);
        //    }

        //    return fileName;
        //}
        #endregion
    }
}