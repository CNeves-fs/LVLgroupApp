using AutoMapper;
using Core.Features.Fotos.Commands.Create;
using Core.Features.Fotos.Commands.Delete;
using Core.Features.Fotos.Commands.Update;
using Core.Features.Fotos.Queries.GetAllCached;
using Core.Features.Fotos.Queries.GetById;
using DocumentFormat.OpenXml.Wordprocessing;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Claim.Controllers.Fototag;
using LVLgroupApp.Areas.Claim.Models.Foto;
using MailKit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Claim.Controllers.Foto
{
    [Area("Claim")]
    [Authorize]
    public class FotoController : BaseController<FotoController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<FotoController> _localizer;

        private IWebHostEnvironment _environment;


        //---------------------------------------------------------------------------------------------------


        public FotoController(IWebHostEnvironment Environment, IStringLocalizer<FotoController> localizer)
        {
            _environment = Environment;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Chamada por ajax call a partir da View _CreateOrEdit.
        /// É passado o Id da claim a ser editada e o guid gerado
        /// na View Index ou _ViewAll (mecanismos de editar claim).
        /// => Prepara uma estrutura de dados para passar com a partial
        /// view _ViewFotoUploader. É inicializada com o Id da claim
        /// em causa e a lista de todas as foto tags para marcar uma foto.
        /// Retorna a partial view _ViewFotoUploader
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="claimFolder"></param>
        /// <returns></returns>

        public async Task<IActionResult> OnGetFotoUploaderAsync(int claimId = 0, string claimFolder = "")
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - OnGetFotoUploaderAsync - Entrou com ClaimId=" + claimId + " claimFolder=" + claimFolder);
            var fotoviewModel = new FotoUploaderViewModel
            {
                ClaimId = claimId,
                ClaimFolder = claimFolder,
                Fototags = await FototagController.GetSelectListAllFototagsAsync(0, _mapper, _mediator),
                FototagId = 0
                //Descrição = string.Empty
            };

            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - OnGetFotoUploaderAsync - Vai sair e retornar _ViewFotoUploader");
            return PartialView("_ViewFotoUploader", fotoviewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Chamada por ajax call a partir da partialView _ViewFotoUploader.
        /// É passado o Id da claim a ser editada e o guid gerado
        /// na View Index ou _ViewAll (mecanismos de editar claim).
        /// => Prepara uma estrutura de dados para passar com a partial
        /// view _ViewFotoGallery. É inicializada com o Id da claim
        /// em causa, a lista de todas as foto tags para marcar uma foto e todas
        /// as fotos da galeria. Retorna a partial view _ViewFotoGallery.
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="claimFolder"></param>
        /// <returns></returns>

        public async Task<IActionResult> OnGetViewGalleryAsync(int claimId = 0, string claimFolder = "")
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - OnGetViewGalleryAsync - Entrou com  claimId=" + claimId + " tempFolder=" + claimFolder);

            var fotosviewModel = new List<FotoViewModel>();
            if(claimId == 0)
            {
                // criar claim em folder temporário
                var createdFotosResponse = await _mediator.Send(new GetAllFotosByTempFolderCachedQuery() { tempFolder = claimFolder });
                fotosviewModel = _mapper.Map<List<FotoViewModel>>(createdFotosResponse.Data);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - OnGetViewGalleryAsync - get all fotos in db in claimfolder=" + claimFolder + " total=" + fotosviewModel.Count);
            }
            else
            {
                // editar claim no folder ClaimId
                var fotosResponse = await _mediator.Send(new GetAllFotosByClaimIdCachedQuery() { claimId = claimId });
                fotosviewModel = _mapper.Map<List<FotoViewModel>>(fotosResponse.Data);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - OnGetViewGalleryAsync - get all fotos from claim=" + claimId + " total=" + fotosviewModel.Count);
            }

            var fotoGallery = new FotoGalleryViewModel
            {
                ClaimId = claimId,
                ClaimFolder = claimFolder,
                Fotos = fotosviewModel,
            };

            try
            {
                var wwwPath = _environment.WebRootPath;
                var contentPath = Path.Combine(_environment.WebRootPath, "Claims");
                var dirPath = Path.Combine(contentPath, claimFolder);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - OnGetViewGalleryAsync - Vai ler file do folder=" + dirPath);

                if (Directory.Exists(dirPath))
                {
                    foreach (var foto in fotoGallery.Fotos)
                    {
                        foto.ClaimFolder = claimFolder;
                        foto.Path = "~/Claims/" + claimFolder + "/" + foto.FileName;
                        foto.ImageSource = "/Claims/" + claimFolder + "/" + foto.FileName;
                        foto.Tag = await FototagController.GetFototagAsync((int)foto.FototagId, _mapper, _mediator);

                        var filePath = Path.Combine(dirPath, foto.FileName);
                        using (Image image = Image.Load(filePath))
                        {
                            foto.ImageWidth = image.Width.ToString();
                            foto.ImageHeight = image.Height.ToString();
                        }
                    }
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - OnGetViewGalleryAsync - Folder não existe=" + dirPath);
                }
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - OnGetViewGalleryAsync - vai sair e retornar _ViewFotoGallery fotos=" + fotoGallery.Fotos.Count);
                return PartialView("_ViewFotoGallery", fotoGallery);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - OnGetViewGalleryAsync - IO exception vai sair e retornar view. Error: " + ex.Message);
                return PartialView("_ViewFotoGallery", fotoGallery);
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Atende o file upload do client.
        /// cria o folder temporário se necessário e copia para lá a foto.
        /// regista a foto na bd.
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="claimFolder"></param>
        /// <param name="descrição"></param>
        /// <param name="fototagId"></param>
        /// <param name="FilePhoto"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> UploadToFolderAsync(int claimId, string claimFolder, string descrição, int fototagId, IFormFile FilePhoto)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - UploadToFolderAsync folder = " + claimFolder + " claim =" + claimId);

            if (ModelState.IsValid && FilePhoto != null)
            {
                try
                {
                    var wwwPath = _environment.WebRootPath;
                    var contentPath = Path.Combine(_environment.WebRootPath, "Claims");

                    _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - Vai criar file no folder=" + claimFolder + " file=" + FilePhoto.FileName);
                    
                    // colocar file no claimFolder
                    var dirPath = Path.Combine(contentPath, claimFolder);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                        _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - Folder não existia e foi criado=" + dirPath);
                    }
                    else
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - Folder já existia=" + dirPath);
                    }

                    var fileExt = ".jpeg";
                    var fileName = Guid.NewGuid().ToString() + fileExt;
                    var filePath = dirPath + "/" + fileName;

                    // limit image to 1500px width
                    var widthLimit = 1500;
                    var image = GetImageAutoRotate(FilePhoto);

                    // salvar imagem final como jpeg
                    var encoder = new JpegEncoder
                    {
                        Quality = 80
                    };


                    var output = new MemoryStream();
                    image.Mutate(x => x.Resize(widthLimit, 0));
                    image.Save(output, encoder);
                    output.Position = 0;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        output.CopyTo(stream);
                    }
                    _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - File criado com sucesso=" + filePath);

                    // registar o file na db
                    var fotoviewModel = new FotoViewModel
                    {
                        ClaimId = claimId > 0 ? claimId : null,
                        ClaimFolder = claimId > 0 ? null : claimFolder,
                        FototagId = fototagId,
                        Descrição = descrição,
                        FileName = fileName,
                        Path = filePath,
                        ImageSource = "/Claims/" + claimFolder + "/" + fileName
                    };
                    _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - Vai registar o file na db em Claim=" + fotoviewModel.ClaimId + " claimFolder=" + fotoviewModel.ClaimFolder + " foto=" + fotoviewModel.FileName);
                    var createFotoCommand = _mapper.Map<CreateFotoCommand>(fotoviewModel);
                    var result = await _mediator.Send(createFotoCommand);
                    _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - Foto criada com sucesso na db=" + fotoviewModel.FileName + " fotoId=" + result.Data);

                    _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UploadToTempFolderAsync - Vai sair e retornar success");
                    return new ObjectResult(new { status = "success" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - UploadToTempFolderAsync - IO exception vai sair e retornar Error: " + ex.Message);
                    return new ObjectResult(new { status = "error" });
                }
            }
            else
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - UploadToTempFolderAsync - ModelState IsNot Valid ou FilePhoto == null");
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza uma foto existente na _ViewFotoGallery.
        ///a foto continua a existir no folder tempFolderGuid.
        ///a fototag ou a descrição podem ser atualizados.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="claimId"></param>
        /// <param name="claimFolder"></param>
        /// <param name="descrição"></param>
        /// <param name="fototagId"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> UpdateFotoInFolderAsync(int Id, int claimId, string claimFolder, string descrição, int fototagId)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UpdateFotoInTempFolderAsync - Entrou para foto=" + Id + " claim=" + claimId + " claimFolder=" + claimFolder);
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UpdateFotoInTempFolderAsync - Vai ler foto da db com fotoId=" + Id);
            
            var fotoResponse = await _mediator.Send(new GetFotoByIdQuery() { Id = Id });
            var fotoviewModel = _mapper.Map<FotoViewModel>(fotoResponse.Data);
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UpdateFotoInTempFolderAsync - foto lida da db=" + fotoviewModel.FileName);

            fotoviewModel.Descrição = descrição;
            fotoviewModel.FototagId = fototagId;

            var updateFotoCommand = _mapper.Map<UpdateFotoCommand>(fotoviewModel);
            var resultUpdt = await _mediator.Send(updateFotoCommand);
            if (resultUpdt.Succeeded)
            {
                _notify.Success($"{_localizer["Foto com ID"]} {resultUpdt.Data} {_localizer[" atualizada."]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Foto Contoller - UpdateFotoInTempFolderAsync - foto updated com sucesso na db=" + updateFotoCommand.Id);
                return new ObjectResult(new { status = "success" });
            }

            _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - UpdateFotoInTempFolderAsync - Erro ao atualizar foto com fotoId=" + updateFotoCommand.Id + " Vai sair e retornar error");
            return new ObjectResult(new { status = "error" });
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o botão remover da foto em _ViewFotoGallery
        /// remove o registo da foto na db
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="claimFolder"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> DeleteFotoInFolderAsync(int Id, string claimFolder)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Entrou para delete de fotoId=" + Id + " claimFolder=" + claimFolder);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Vai ler foto da db fotoId=" + Id);
                var fotoResponse = await _mediator.Send(new GetFotoByIdQuery() { Id = Id });
                if (fotoResponse.Succeeded)
                {
                    var fotoviewModel = _mapper.Map<FotoViewModel>(fotoResponse.Data);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - foto lida da db=" + fotoviewModel.FileName);

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Vai fazer delete da foto no FS foto=" + fotoviewModel.FileName);
                    var contentPath = Path.Combine(_environment.WebRootPath, "Claims");
                    var dirPath = Path.Combine(contentPath, claimFolder);
                    if (Directory.Exists(dirPath))
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Folder existe= " + dirPath);
                        DirectoryInfo diSource = new DirectoryInfo(dirPath);
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Existem " + diSource.GetFiles().Length + " em " + dirPath);
                        foreach (FileInfo fi in diSource.GetFiles())
                        {
                            if (fi.Name == fotoviewModel.FileName)
                            {
                                // delete file
                                fi.Delete();

                                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - file deleted com sucesso= " + fi.Name);
                                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Vai remover da db foto=" + Id);

                                var deleteCommand = await _mediator.Send(new DeleteFotoCommand { Id = Id });
                                if (deleteCommand.Succeeded)
                                {
                                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - foto deleted com sucesso=" + Id);
                                    _notify.Success($"{_localizer["Foto with ID"]} {Id} {_localizer["deleted."]}");
                                    return new ObjectResult(new { status = "success" });
                                }
                                else
                                {
                                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Erro ao fazer delete da foto : vai retornar error");
                                    return new ObjectResult(new { status = "error" });
                                }
                            }
                        }
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Erro: foto not found : vai retornar error");
                        return new ObjectResult(new { status = "error" });
                    }
                    else
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Diretoria não existe. Vai sair e retornar error");
                        return new ObjectResult(new { status = "error" });
                    }
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Erro ao ler foto da db fotoId=" + Id);
                    return new ObjectResult(new { status = "error" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// move a foto fileName de um folder source para outro folder dest
        /// (esta função move todas as fotos de source para dest)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sourceFolderName"></param>
        /// <param name="destFolderName"></param>
        /// <returns></returns>

        public static bool MoveFotoFromTempToClaimFolder(
            string fileName,
            string sourceFolderName,
            string destFolderName,
            IWebHostEnvironment environment,
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            try
            {
                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - Entrou para mover os files de source=" + sourceFolderName + " para dest=" + destFolderName);
                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var sourcePath = Path.Combine(contentPath, sourceFolderName);
                var destPath = Path.Combine(contentPath, destFolderName);

                if (Directory.Exists(sourcePath) && Directory.Exists(destPath))
                {
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - Ambos os Folders existem");
                    DirectoryInfo diSource = new DirectoryInfo(sourcePath);
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - Existem " + diSource.GetFiles().Length + " em " + sourcePath);
                    foreach (FileInfo fi in diSource.GetFiles())
                    {
                        fi.MoveTo(Path.Combine(destPath, fi.Name), true);
                        logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - File " + fi.Name + " foi movido para " + destPath);
                    }
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - Todos os files foram movidos com sucesso: vai retornar true");
                    return true;
                }
                else
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - Diretoria não existe. Vai sair e retornar false");
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - MoveFotoFromTempToClaimFolder - IO exception vai sair e retornar false: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica se uma foto existe no folder da claim
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>

        public static bool FotoInFolderExiste(
            string fileName, 
            string folderName,
            IWebHostEnvironment environment,
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            try
            {
                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - FotoInFolderExiste - Entrou para verificar fileName=" + fileName + " folderName=" + folderName);
                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var dirPath = Path.Combine(contentPath, folderName);
                if (Directory.Exists(dirPath))
                {
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - FotoInFolderExiste - Folder existe= " + dirPath);
                    DirectoryInfo diSource = new DirectoryInfo(dirPath);
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Existem " + diSource.GetFiles().Length + " em " + dirPath);
                    foreach (FileInfo fi in diSource.GetFiles())
                    {
                        if (fi.Name == fileName)
                        {
                            // file existe
                            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - FotoInFolderExiste - file verificado com sucesso= " + fi.Name);
                            return true;
                        }
                    }
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - FotoInFolderExiste - Erro: foto not found : vai retornar false");
                    return false;
                }
                else
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - FotoInFolderExiste - Diretoria não existe. Vai sair e retornar error");
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - FotoInFolderExiste - IO exception vai sair e retornar false: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um folder na estrutura de pastas da aplicação
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="environment"></param>
        /// <returns></returns>

        public static bool CreateFolder(
            string folderName,
            IWebHostEnvironment environment, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            try
            {
                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - CreateFolder - Entrou para criar o Folder=" + folderName);

                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var dirPath = Path.Combine(contentPath, folderName);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - CreateFolder - vai retornar true com folder criado=" + dirPath);
                    //Thread.Sleep(1000);
                    return true;
                }

                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - CreateFolder - vai retornar False porque folder já existia");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - CreateFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz delete de um folder na estrutura de pastas da aplicação
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static bool DeleteFolder(
            string folderName,
            IWebHostEnvironment environment, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            try
            {
                logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFolder - Entrou para delete folder=" + folderName);

                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var dirPath = Path.Combine(contentPath, folderName);
                if (Directory.Exists(dirPath))
                {
                    DirectoryInfo di = new DirectoryInfo(dirPath);
                    logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFolder - Folder existe e vai remover folder= " + dirPath);
                    di.Delete(true);
                    //Thread.Sleep(1000);

                    logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFolder - folder deleted com sucesso. Vai sair e retornar True");
                    return true;
                }

                logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFolder - folder não existe - não foi feito delete. Vai sair e retornar False");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica se um folder existe na estrutura de pastas da aplicação
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static bool FolderExiste(
            string folderName, 
            IWebHostEnvironment environment, 
            ILogger logger, 
            string sessionId, 
            string sessionName)
        {
            try
            {
                logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - FolderExiste - Entrou para verificar se existe folder=" + folderName);

                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var dirPath = Path.Combine(contentPath, folderName);
                if (Directory.Exists(dirPath))
                {
                    logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - FolderExiste - folder existe. Vai sair e retornar True");
                    return true;
                }

                logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - FolderExiste - folder não existe. Vai sair e retornar False");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - FolderExiste - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz rename de um folder na estrutura de pastas da aplicação
        /// rename oldName para newName
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static bool RenameFolder(
            string oldName, 
            string newName, 
            IWebHostEnvironment environment, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - RenameFolder - Entrou para rename de oldFolder=" + oldName + " para=" + newName);

            try
            {
                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var fromPath = Path.Combine(contentPath, oldName);
                var toPath = Path.Combine(contentPath, newName);

                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - RenameFolder - from=" + fromPath + " topath=" + toPath);

                if (Directory.Exists(fromPath))
                {
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - RenameFolder - fromPath existe e vai retornar True");

                    Thread.Sleep(1000);
                    Directory.Move(fromPath, toPath);

                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - RenameFolder - Folder renomeado com sucesso para " + toPath + "Vai sair e retornar True");
                    return true;
                }
                else
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - RenameFolder - Folder " + fromPath + " não existe. Vai sair e retornar False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - RenameFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para registar na db as fotos uploaded.
        /// a claim foi criada, o folder das fotos agora é o CodeId.
        /// as fotos ficam a apontar para a claim agora criada.
        /// as fotos ficam marcadas como definitivas com TempFolderGuid = null
        /// </summary>
        /// <param name="tempFolderGuid"></param>
        /// <param name="claimId"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static async Task<bool> SetFotosInClaimAsync(
            string tempFolderGuid, 
            string CodeId, 
            int claimId, 
            IWebHostEnvironment environment, 
            IMediator mediator, 
            IMapper mapper, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - Entrou para registar na db as fotos que existiam em " + tempFolderGuid + " e existem agora em " + CodeId + " Fotos de ClaimId= " + claimId);

            var wwwPath = environment.WebRootPath;
            var contentPath = Path.Combine(environment.WebRootPath, "Claims");
            var dirPath = Path.Combine(contentPath, CodeId);

            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - Claim dirPath=" + dirPath);

            try
            {
                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - Vai ler na db todas as fotos que existem no tempFolder = " + tempFolderGuid);
                var fotosResponse = await mediator.Send(new GetAllFotosByTempFolderCachedQuery() { tempFolder = tempFolderGuid });
                var fotosList = mapper.Map<List<FotoViewModel>>(fotosResponse.Data);

                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - total de fotos lidas da db para atualizar=" + fotosList.Count);

                // registar as fotos na db
                foreach (FotoViewModel foto in fotosList)
                {
                    foto.ClaimId = claimId > 0 ? claimId : null;
                    foto.ClaimFolder = null;
                    foto.Path = dirPath + "/" + foto.FileName;
                    foto.ImageSource = "/Claims/" + CodeId + "/" + foto.FileName;
                    var updateFotoCommand = mapper.Map<UpdateFotoCommand>(foto);
                    var result = await mediator.Send(updateFotoCommand);
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - foto atualizada=" + foto.Id);
                }

                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - Todas as fotos foram atualizadas. Vai sair e retornar True");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - SetFotosInClaimAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para atualizar na bd a path das fotos de uma claim.
        /// foi feito rename do folder da claim e é necessário registar
        /// na bd a nova localização das fotos.
        /// </summary>
        /// <param name="oldFolder"></param>
        /// <param name="newFolder"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static async Task<bool> UpdatePathFotosInClaimAsync(
            string oldFolder,
            string newFolder,
            int claimId,
            IWebHostEnvironment environment,
            IMediator mediator,
            IMapper mapper,
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - Entrou para registar na db as fotos que existiam em " + oldFolder + " e existem agora em " + newFolder + " Fotos de ClaimId= " + claimId);

            var wwwPath = environment.WebRootPath;
            var contentPath = Path.Combine(environment.WebRootPath, "Claims");
            var dirPath = Path.Combine(contentPath, newFolder);

            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - Claim dirPath=" + dirPath);

            try
            {
                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - Vai ler na db todas as fotos da ClaimId = " + claimId);
                var fotosResponse = await mediator.Send(new GetAllFotosByClaimIdCachedQuery() { claimId = claimId });
                var fotosList = mapper.Map<List<FotoViewModel>>(fotosResponse.Data);

                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - total de fotos lidas da db para atualizar=" + fotosList.Count);

                // registar as fotos na db
                foreach (FotoViewModel foto in fotosList)
                {
                    foto.Path = dirPath + "/" + foto.FileName;
                    var updateFotoCommand = mapper.Map<UpdateFotoCommand>(foto);
                    var result = await mediator.Send(updateFotoCommand);
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - foto atualizada=" + foto.Id);
                }

                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - Todas as fotos foram atualizadas. Vai sair e retornar True");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - UpdatePathFotosInClaimAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// conta o numero de fotos associadas a uma claim
        /// </summary>
        /// <param name="tempFolderGuid"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static async Task<int> CountFotosInFolderAsync(
            string claimFolder,
            int Id,
            IMediator mediator, 
            IMapper mapper, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            try
            {
                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - CountFotosInFolderAsync - claimfolder=" + claimFolder);
                var count = 0;

                if(Id == 0)
                {
                    // create claim in temp folder
                    var createdFotosResponse = await mediator.Send(new GetAllFotosByTempFolderCachedQuery() { tempFolder = claimFolder });
                    var createdFotosList = mapper.Map<List<FotoViewModel>>(createdFotosResponse.Data);
                    count = createdFotosList.Count;
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - CountFotosInFolderAsync - Vai sair e retornar o total de fotos em tempfolder=" + count);
                }
                else
                {
                    // edit claim in codeid folder
                    var fotosResponse = await mediator.Send(new GetAllFotosByClaimIdCachedQuery() { claimId = Id });
                    var fotosList = mapper.Map<List<FotoViewModel>>(fotosResponse.Data);
                    count = fotosList.Count;
                    logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - CountFotosInFolderAsync - Vai sair e retornar o total de fotos em tempfolder=" + count);
                }
                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - CountFotosInFolderAsync - DB exception vai sair e retornar Error: " + ex.Message);
                return 0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz delete de todos os registos de fotos pertencentes a uma claim
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <returns></returns>

        public static async Task<bool> DeleteAllFotosFromDBAsync(
            int claimId, 
            IMediator mediator, 
            IMapper mapper, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync -  Entrou para remover da db todas as fotos de ClaimId=" + claimId);

            try
            {
                if (claimId > 0)
                {
                    var fotosResponse = await mediator.Send(new GetAllFotosByClaimIdCachedQuery() { claimId = claimId });
                    var fotosList = mapper.Map<List<FotoViewModel>>(fotosResponse.Data);

                    if (fotosResponse.Succeeded)
                    {
                        logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync - Fotos lidas com sucesso da db. Total de fotos para remover=" + fotosList.Count);
                        foreach (FotoViewModel foto in fotosList)
                        {
                            var deleteFotoCommand = await mediator.Send(new DeleteFotoCommand() { Id = foto.Id });
                            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync - foto deleted=" + foto.Id);
                        }

                        logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync - fotos foram todas removidas da db. Vai sair e retornar True");
                        return true;
                    }
                    else
                    {
                        logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync - Erro ao ler lista de fotos na db. Vai sair e retornar False");
                        return false;
                    }
                }
                else
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync - Erro: claimId == 0. Vai sair e retornar False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllFotosFromDBAsync - DB exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz delete de todos os folders temporários
        /// com mais de 2 dias
        /// </summary>
        /// <returns>bool</returns>

        public static bool DeleteAllTempFolders(
            IWebHostEnvironment environment,
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllTempFolders -  Entrou para remover os temp folders");

            try
            {
                var wwwPath = environment.WebRootPath;
                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var allFolders = Directory.GetDirectories(contentPath);

                foreach ( var folder in allFolders )
                {
                    var folderName = Path.GetFileName(folder);
                    var strArray = folderName.Split('-');
                    if (strArray.Length == 5)
                    {
                        // folder temporário
                        DirectoryInfo d = new DirectoryInfo(folder);
                        if (d.CreationTime < DateTime.Now.AddDays(-2))
                        {
                            // delete old folder
                            Directory.Delete(folder, true);
                            logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllTempFolders - Folder deleted: " + folderName);
                        }
                    };
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllTempFolderAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// procura fotos em todos os folders temporários.
        /// Se encontrar uma foto com o mesmo nome, retorna o folder name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>string</returns>

        public static string FindFotoInTempFolders(
            string fileName,
            IWebHostEnvironment environment,
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - FindFotoInTempFolders -  Entrou para procurar foto = " + fileName);

            try
            {
                var wwwPath = environment.WebRootPath;
                var contentPath = Path.Combine(environment.WebRootPath, "Claims");
                var allFolders = Directory.GetDirectories(contentPath);

                foreach (var folder in allFolders)
                {
                    var folderName = Path.GetFileName(folder);
                    var strArray = folderName.Split('-');
                    if (strArray.Length == 5)
                    {
                        // folder temporário
                        DirectoryInfo diSource = new DirectoryInfo(folder);
                        foreach (FileInfo fi in diSource.GetFiles())
                        {
                            if (fi.Name == fileName)
                            {
                                // file existe
                                logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - FindFotoInTempFolders - file encontrado com sucesso= " + fi.Name + " no folder=" + folderName);
                                return folderName;
                            }
                        }
                    };
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteAllTempFolderAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return string.Empty;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz delete de todos os registos de fotos
        /// que tenham o ClaimFolder == claimFolder
        /// </summary>
        /// <param name="claimFolder"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>

        public static async Task<bool> DeleteFotosInFolderFromDBAsync(
            string claimFolder, 
            IMediator mediator, 
            IMapper mapper, 
            ILogger logger, 
            string sessionId, 
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync -  Entrou para remover da db todas as fotos existentes no claimFolder=" + claimFolder);

            try
            {
                if (!string.IsNullOrEmpty(claimFolder))
                {
                    var fotosResponse = await mediator.Send(new GetAllFotosByTempFolderCachedQuery() { tempFolder = claimFolder });
                    var fotosList = mapper.Map<List<FotoViewModel>>(fotosResponse.Data);

                    if (fotosResponse.Succeeded)
                    {
                        logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync - Fotos lidas da db com sucesso. Total de fotos para remover=" + fotosList.Count);

                        foreach (FotoViewModel foto in fotosList)
                        {
                            var deleteFotoCommand = await mediator.Send(new DeleteFotoCommand() { Id = foto.Id });
                            logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync - foto deleted=" + foto.Id);
                        }

                        logger.LogInformation(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync - Todas as fotos foram removidas. Vai sair e retornar True");
                        return true;
                    }
                    else
                    {
                        logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync - Erro ao ler lista de fotos na db. Vai sair e retornar False");
                        return false;
                    }
                }
                else
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync - Erro: claimFolder == null or empty. Vai sair e retornar False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInFolderFromDBAsync - DB exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz delete na db e no filesystem, de todas as fotos existentes em TempFolders
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteFotosInAllTempFolderFromDBAsync(
            IMediator mediator, 
            IMapper mapper, 
            IWebHostEnvironment environment, 
            ILogger logger,
            string sessionId,
            string sessionName)
        {
            logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync -  Entrou para remover da db e Fsystem todas as fotos existentes em todos os tempFolders");

            try
            {
                var fotosResponse = await mediator.Send(new GetAllFotosAllTempFolderCachedQuery());
                var fotosList = mapper.Map<List<FotoViewModel>>(fotosResponse.Data);

                if (fotosResponse.Succeeded)
                {
                    logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - Fotos lidas da db com sucesso. Total de fotos para remover=" + fotosList.Count);

                    foreach (FotoViewModel foto in fotosList)
                    {
                        if (!string.IsNullOrEmpty(foto.ClaimFolder))
                        {
                            var tempFolder = foto.ClaimFolder;
                            DeleteFolder(foto.ClaimFolder, environment, logger, sessionId, sessionName);
                            logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - folder deleted=" + foto.ClaimFolder);
                            var deleteFotoCommand = await mediator.Send(new DeleteFotoCommand() { Id = foto.Id });
                            logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - foto deleted=" + foto.Id);
                        }
                        else
                        {
                            logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - Erro inesperado. Foto na db registada como definitiva (CodeId). Vai sair e retornar False");
                            return false;
                        }
                    }

                    logger.LogInformation(sessionId + " | " + sessionName +  " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - fotos foram deleted. vai retornar True");
                    return true;
                }
                else
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - Erro ao ler lista de fotos na db. Vai sair e retornar False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Foto Contoller - DeleteFotosInAllTempFolderFromDBAsync - DB exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        public JsonResult GetLVLlogo(string logoName)
        {
            var dirPath = Path.Combine(_environment.WebRootPath, "images");
            var fullPath = Path.Combine(dirPath, logoName);
            if (!System.IO.File.Exists(fullPath)) return Json(new { status = "error" });
            var logoArray = System.IO.File.ReadAllBytes(fullPath);
            string logo_inBase64 = Convert.ToBase64String(logoArray);
            return Json(new { status = "success", logoBase64 = logo_inBase64 });
        }


        //---------------------------------------------------------------------------------------------------


        private Image GetImageAutoRotate(IFormFile Photo)
        {
            var streamUploaded = Photo.OpenReadStream();
            var image = Image.Load(streamUploaded);

            // verificar se foi aplicada orientação à foto
            var exifProfile = image.Metadata?.ExifProfile;
            var exifOrientation = string.Empty;

            if (exifProfile != null)
            {
                foreach (var item in exifProfile.Values)
                {
                    if (item.Tag == ExifTag.Orientation)
                    {
                        exifOrientation = item.GetValue().ToString();

                    }
                }
            }

            // no orientation was applied
            if (string.IsNullOrEmpty(exifOrientation)) return image;

            // verificar orientação aplicada
            RotateMode rotateMode;
            FlipMode flipMode;
            SetRotateFlipMode(exifOrientation, out rotateMode, out flipMode);

            // rotate as requested by orientation
            image.Mutate(x => x.RotateFlip(rotateMode, flipMode));

            //remover Icc profile e metadata
            image.Metadata.IccProfile = null;
            image.Metadata.XmpProfile = null;
            image.Metadata.ExifProfile = null;

            return image;
        }


        //---------------------------------------------------------------------------------------------------


        private void SetRotateFlipMode(string exifOrientation, out RotateMode rotateMode, out FlipMode flipMode)
        {

            switch (exifOrientation)
            {
                case "2":
                    rotateMode = RotateMode.None;
                    flipMode = FlipMode.Horizontal;
                    break;
                case "3":
                    rotateMode = RotateMode.Rotate180;
                    flipMode = FlipMode.None;
                    break;
                case "4":
                    rotateMode = RotateMode.Rotate180;
                    flipMode = FlipMode.Horizontal;
                    break;
                case "5":
                    rotateMode = RotateMode.Rotate90;
                    flipMode = FlipMode.Horizontal;
                    break;
                case "6":
                    rotateMode = RotateMode.Rotate90;
                    flipMode = FlipMode.None;
                    break;
                case "7":
                    rotateMode = RotateMode.Rotate90;
                    flipMode = FlipMode.Vertical;
                    break;
                case "8":
                    rotateMode = RotateMode.Rotate270;
                    flipMode = FlipMode.None;
                    break;
                default:
                    rotateMode = RotateMode.None;
                    flipMode = FlipMode.None;
                    break;
            }
        }


        //---------------------------------------------------------------------------------------------------   

    }
}