using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class ReportAnswerAttachment
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]        
        public int ReportAnswerId { get; set; }               // FK para ReportAnswer

        [Required]
        // Arquivo salvo em disco (relative path from wwwroot, ex: uploads/reports/1/answers/23/guid.jpg)
        public string RelativePath { get; set; } = null!;

        [Required]
        // Thumbnail relative path (same base folder or thumbs/ subfolder)
        public string ThumbnailRelativePath { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;         // original file name

        [Required]
        public string ContentType { get; set; } = null!;      // ex: image/jpeg

        [Required]
        public long FileSize { get; set; }                    // bytes

        [Required]
        public int ImageWidth { get; set; }

        [Required]
        public int ImageHeight { get; set; }

        public string? UploadedBy { get; set; }               // user id (nullable)


        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;




        public virtual ReportAnswer ReportAnswer { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
