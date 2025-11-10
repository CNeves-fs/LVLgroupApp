using Core.Entities.Ocorrencias;
using Core.Interfaces.Shared;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DocumentService : IDocumentService
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IWebHostEnvironment _environment;

        private readonly LVLgroupDbContext _dbContext;


        //---------------------------------------------------------------------------------------------------


        public DocumentService(IWebHostEnvironment environment, LVLgroupDbContext dbContext)
        {
            _environment = environment;
            _dbContext = dbContext;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UploadDocumentAsync(int ocorrenciaId, IFormFile file)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "ocorrencias", ocorrenciaId.ToString());
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var document = new OcorrenciaDocument
            {
                OcorrenciaId = ocorrenciaId,
                FileName = file.FileName,
                FilePath = filePath,
                UploadDate = DateTime.Now
            };

            _dbContext.OcorrenciasDocuments.Add(document);
            await _dbContext.SaveChangesAsync();
        }


        //---------------------------------------------------------------------------------------------------

    }
}