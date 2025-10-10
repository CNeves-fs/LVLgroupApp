using AutoMapper;
using ClosedXML.Excel;
using Core.Constants;
using Core.Entities.Select2;
using Core.Features.Artigos.Commands.Create;
using Core.Features.Artigos.Commands.Delete;
using Core.Features.Artigos.Commands.Update;
using Core.Features.Artigos.Queries.GetAllCached;
using Core.Features.Artigos.Queries.GetById;
using Core.Features.Artigos.Queries.GetByRef;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Empresas.Queries.GetByNome;
using Core.Features.Genders.Queries.GetAllCached;
using Core.Features.Genders.Queries.GetById;
using Core.Features.Genders.Queries.GetByNome;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Artigo.Controllers.Gender;
using LVLgroupApp.Areas.Artigo.Models.Artigo;
using LVLgroupApp.Areas.Artigo.Models.Gender;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Artigo.Controllers.Artigo
{
    [Area("Artigo")]
    [Authorize]
    public class ArtigoController : BaseController<ArtigoController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ArtigoController> _localizer;

        private readonly IHubContext<ProgressHub> _hubContext;

        private IWebHostEnvironment _environment;


        //---------------------------------------------------------------------------------------------------


        public ArtigoController(IWebHostEnvironment environment, IStringLocalizer<ArtigoController> localizer, IHubContext<ProgressHub> hubContext)
        {
            _hubContext = hubContext;
            _localizer = localizer;
            _environment = environment;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Artigos.View)]
        public IActionResult Index()
        {
            var model = new ArtigoViewModel();
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Artigo Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Artigos.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Artigo Contoller - LoadAll - return lista vazia de artigoViewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.View)]
        public async Task<IActionResult> GetArtigos()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var genderfilter = Request.Form["genderFilter"].FirstOrDefault();
                int intFilterGender = genderfilter != null ? Convert.ToInt32(genderfilter) : 0;

                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // lista de artigos
                var responseAllArtigos = await _mediator.Send(new GetAllArtigosCachedQuery());
                if (!responseAllArtigos.Succeeded) return null;
                var allArtigosData = _mapper.Map<List<Core.Entities.Artigos.Artigo>>(responseAllArtigos.Data).AsQueryable();

                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allArtigosData = allArtigosData.Where(a => a.EmpresaId == intEmpresaFilter);
                };

                // filtrar por gender se necessário
                if (intFilterGender > 0)
                {
                    allArtigosData = allArtigosData.Where(a => a.GenderId == intFilterGender);
                };

                var responseAllGenders = await _mediator.Send(new GetAllGendersCachedQuery());
                if (!responseAllGenders.Succeeded) return null;
                var allGendersData = _mapper.Map<List<Core.Entities.Artigos.Gender>>(responseAllGenders.Data).AsQueryable();

                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return null;
                var allEmpresasData = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                var artigosData = allArtigosData
                    .Join(allGendersData,
                        a => a.GenderId,
                        g => g.Id,
                        (a, g) => new { artigo = a, gender = g })
                    .Join(allEmpresasData,
                        ag => ag.artigo.EmpresaId,
                        e => e.Id,
                        (ag, e) => new {ag.artigo, ag.gender, e.Nome})
                    .Select(avm => new ArtigoViewModel() {
                        Id = avm.artigo.Id,
                        Referencia = avm.artigo.Referencia,
                        EmpresaId = avm.artigo.EmpresaId,
                        Empresa = avm.Nome,
                        GenderId = avm.gender.Id,
                        Gender = avm.gender.Nome,
                        Pele = avm.artigo.Pele,
                        Cor = avm.artigo.Cor
                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    artigosData = artigosData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    artigosData = artigosData.Where(x => x.Referencia.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                            x.Empresa.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                            x.Gender.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                }
                recordsTotal = artigosData.Count();
                var data = artigosData.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Artigo Contoller - GetArtigos - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Artigos.Create)]
        public async Task<IActionResult> OnGetLoadFromExcel()
        {
            var artigoViewModel = new ArtigoViewModel();
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_LoadFromExcel", artigoViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.Create)]
        public async Task<IActionResult> OnPostLoadFromExcel(IList<IFormFile> filesExcel)
        {
            var counter = 0;
            if (filesExcel == null) return new ObjectResult(new { status = "error" });

            foreach (IFormFile file in filesExcel)
            {
                try
                {
                    //await hub1.SendStartProgressAsync(file.FileName);
                    await _hubContext.Clients.All.SendAsync("ReceiveStartMessage", "File: " + file.FileName + " em processamento...");

                    var fileextension = System.IO.Path.GetExtension(file.FileName);
                    var filename = Guid.NewGuid().ToString() + fileextension;
                    var contentPath = System.IO.Path.Combine(_environment.WebRootPath, "ExcelFiles");
                    var filepath = System.IO.Path.Combine(contentPath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        file.CopyTo(fs);
                    }

                    int rowno = 1;
                    XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filepath);
                    var sheet = workbook.Worksheets.First();
                    var rows = sheet.Rows().ToList();
                    var sheetSize = sheet.Rows().Count() - 1;


                    foreach (var row in rows)
                    {
                        if (rowno != 1)
                        {
                            await UpdateProgressBarAsync(rowno - 1, sheetSize, _hubContext);

                            var artigo = await GetArtigoFromExcelRow(row);
                            var response_artigo = await _mediator.Send(new GetArtigoByReferenciaQuery() { Referencia = row.Cell(3).Value.ToString() });
                            if (response_artigo.Succeeded)
                            {
                                if (response_artigo.Data != null)
                                {
                                    // artigo já existe
                                    artigo.Id = response_artigo.Data.Id;
                                    var updateArtigoCommand = _mapper.Map<UpdateArtigoCommand>(artigo);
                                    var result = await _mediator.Send(updateArtigoCommand);
                                    Thread.Sleep(500);
                                }
                                else
                                {
                                    // artigo ainda não existe
                                    var createArtigoCommand = _mapper.Map<CreateArtigoCommand>(artigo);
                                    var result = await _mediator.Send(createArtigoCommand);
                                    Thread.Sleep(500);
                                }
                                counter++;

                            }
                        }
                        rowno++;
                    }
                }
                catch
                {
                    return Json(new { status = "error", done = counter});
                }
            }

            await StopProgressBarAsync(filesExcel.Count, _hubContext);
            return new ObjectResult(new { status = "success", files = filesExcel.Count, artigos = counter });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.Create)]
        public JsonResult OnPostCancel()
        {
            DeleteExcelFolderContent(_environment);
            return new JsonResult(new { status = "success" });
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Artigos.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo Artigo
            {
                var artigoViewModel = new ArtigoViewModel();
                artigoViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);
                artigoViewModel.Genders = await GenderController.GetSelectListAllGendersAsync(0, _mapper, _mediator);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", artigoViewModel) });
            }
            else // Editar Artigo
            {
                var response = await _mediator.Send(new GetArtigoByIdQuery() { Id = id });
                var artigoViewModel = _mapper.Map<ArtigoViewModel>(response.Data);
                artigoViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(artigoViewModel.EmpresaId, _mapper, _mediator);
                artigoViewModel.Genders = await GenderController.GetSelectListAllGendersAsync(artigoViewModel.GenderId, _mapper, _mediator);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", artigoViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, ArtigoViewModel artigo)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Artigo
                    var createArtigoCommand = _mapper.Map<CreateArtigoCommand>(artigo);
                    var result = await _mediator.Send(createArtigoCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Artigo com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update Artigo
                    var updateArtigoCommand = _mapper.Map<UpdateArtigoCommand>(artigo);
                    var result = await _mediator.Send(updateArtigoCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Artigo com ID"]} {result.Data} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllArtigosCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<ArtigoViewModel>>(response.Data);
                    foreach (var item in viewModel)
                    {
                        if (item.EmpresaId > 0)
                        {
                            var response_empId = await _mediator.Send(new GetEmpresaByIdQuery() { Id = item.EmpresaId });
                            if (response_empId.Succeeded)
                            {
                                item.Empresa = response_empId.Data.Nome;
                            }
                        }
                        if (item.GenderId > 0)
                        {
                            var response_genderId = await _mediator.Send(new GetGenderByIdQuery() { Id = item.GenderId });
                            if (response_genderId.Succeeded)
                            {
                                item.Gender = response_genderId.Data.Nome;
                            }
                        }
                    }

                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    return null;
                }
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", artigo);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteArtigoCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Artigo com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllArtigosCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<ArtigoViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    return null;
                }
            }
            else
            {
                _notify.Error(deleteCommand.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Artigos.Create)]
        public async Task<IActionResult> OnGetCreateArtigoAsync()
        {
            var artigoViewModel = new ArtigoViewModel();
            artigoViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);
            artigoViewModel.Genders = await GenderController.GetSelectListAllGendersAsync(0, _mapper, _mediator);
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_ViewCreateArtigo", artigoViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.Create)]
        public async Task<JsonResult> OnPostCreateArtigoAsync(ArtigoViewModel artigo)
        {
            if (ModelState.IsValid)
            {
                //create new Artigo
                var createArtigoCommand = _mapper.Map<CreateArtigoCommand>(artigo);
                var result = await _mediator.Send(createArtigoCommand);
                if (result.Succeeded)
                {
                    var id = result.Data;
                    _notify.Success($"{_localizer["Artigo com ID"]} {result.Data} {_localizer[" criado."]}");
                    return new JsonResult(new { isValid = true, html = string.Empty });
                }
                else
                {
                    _notify.Error(result.Message);
                    return new JsonResult(new { isValid = false, html = string.Empty });
                }
                
            }
            else
            {
                return new JsonResult(new { isValid = false, html = string.Empty });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Artigos.View)]
        public async Task<JsonResult> LoadArtigos()
        {

            var artigosResponse = await _mediator.Send(new GetAllArtigosCachedQuery());

            if (artigosResponse.Succeeded)
            {
                var artigosViewModel = _mapper.Map<List<ArtigoViewModel>>(artigosResponse.Data);
                var artigos = new SelectList(artigosViewModel, nameof(ArtigoViewModel.Id), nameof(ArtigoViewModel.Referencia), null, null);

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { artigosList = artigos });
                return Json(jsonString);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetListAllArtigos(string term, int page)
        {
            int resultCount = 25;
            int offset = (page - 1) * resultCount;
            string searchStr = string.IsNullOrEmpty(term) ? "" : term;

            var artigosResponse = await _mediator.Send(new GetAllArtigosCachedQuery());
            var artigosViewModel = _mapper.Map<List<ArtigoViewModel>>(artigosResponse.Data);
            var searchData = artigosViewModel.Where(a => a.Referencia.Contains(searchStr, StringComparison.OrdinalIgnoreCase));
            int count = searchData.Count();

            searchData = searchData.Skip(offset).Take(resultCount);
            var artigosData = searchData.Select(a => new Select2Item{ id = a.Id, text = a.Referencia }).ToList();

            int endCount = offset + resultCount;
            bool morePages = count > endCount;

            var data = new
            {
                results = artigosData,
                pagination = new
                {
                    more = morePages
                }
            };

            return Ok(data);
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        public async Task<JsonResult> GetArtigoAsync(int artigoId)
        {
            var response = await _mediator.Send(new GetArtigoByIdQuery() { Id = artigoId });
            if (response.Succeeded)
            {
                var artigoViewModel = new ArtigoViewModel();
                artigoViewModel = _mapper.Map<ArtigoViewModel>(response.Data);
                if (artigoViewModel.EmpresaId > 0)
                {
                    var response_empId = await _mediator.Send(new GetEmpresaByIdQuery() { Id = artigoViewModel.EmpresaId });
                    if (response_empId.Succeeded)
                    {
                        artigoViewModel.Empresa = response_empId.Data.Nome;
                    }
                }
                if (artigoViewModel.GenderId > 0)
                {
                    var response_genderId = await _mediator.Send(new GetGenderByIdQuery() { Id = artigoViewModel.GenderId });
                    if (response_genderId.Succeeded)
                    {
                        artigoViewModel.Gender = response_genderId.Data.Nome;
                    }
                }
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { artigo = artigoViewModel });
                return Json(jsonString);
            }
            return Json(new { status = "error" });
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> LoadTamanhos(int genderId)
        {
            if (genderId > 0)
            {
                //var artigoResponse = await _mediator.Send(new GetArtigoByIdQuery() { Id = artigoId });
                var genderResponse = await _mediator.Send(new GetGenderByIdQuery() { Id = genderId });

                if (genderResponse.Succeeded)
                {
                    var genderViewModel = _mapper.Map<GenderViewModel>(genderResponse.Data);

                    var tamanhosN = genderViewModel.TamanhosNum.Split(";");
                    var tamanhosA = genderViewModel.TamanhosAlf.Split(";");
                    string[] tamanhos = tamanhosN.Concat(tamanhosA).ToArray();

                    //var tamanhosList = new SelectList(tamanhos);
                    var tamanhosList = new List<Select2Item>();
                    for(int i = 1; i <= tamanhos.Length; i++)
                    {
                        tamanhosList.Add(new Select2Item() { id = i, text= tamanhos[i-1] });
                    }

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { tamanhosList = tamanhosList });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<ArtigoViewModel> GetArtigoViewModelAsync(int artigoId, IMediator mediator, IMapper mapper)
        {
            if (artigoId == 0) return null;
            var response = await mediator.Send(new GetArtigoByIdQuery() { Id = artigoId });
            if (response.Succeeded)
            {
                var artigoViewModel = mapper.Map<ArtigoViewModel>(response.Data);
                var empresaViewModel = await EmpresaController.GetEmpresaAsync(artigoViewModel.EmpresaId, mapper, mediator);
                artigoViewModel.Empresa = empresaViewModel.Nome;
                return artigoViewModel;
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListTamanhos(int artigoId, int selectedId, IMediator mediator, IMapper mapper)
        {
            if (artigoId > 0)
            {
                var artigoResponse = await mediator.Send(new GetArtigoByIdQuery() { Id = artigoId });

                if (artigoResponse.Succeeded)
                {
                    var artigoViewModel = mapper.Map<ArtigoViewModel>(artigoResponse.Data);
                    var genderResponse = await mediator.Send(new GetGenderByIdQuery() { Id = artigoViewModel.GenderId });
                    var genderViewModel = mapper.Map<GenderViewModel>(genderResponse.Data);

                    var tamanhosN = genderViewModel.TamanhosNum.Split(";");
                    var tamanhosA = genderViewModel.TamanhosAlf.Split(";");
                    string[] tamanhos = tamanhosN.Concat(tamanhosA).ToArray();

                    var tamanhosList = new List<Select2Item>();
                    for (int i = 1; i <= tamanhos.Length; i++)
                    {
                        tamanhosList.Add(new Select2Item() { id = i, text = tamanhos[i - 1] });
                    }

                    return new SelectList(tamanhosList, nameof(Select2Item.id), nameof(Select2Item.text), selectedId, null);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static bool DeleteExcelFolderContent(IWebHostEnvironment env)
        {
            try
            {
                var contentPath = System.IO.Path.Combine(env.WebRootPath, "ArtigosExcel");
                DirectoryInfo di = new DirectoryInfo(contentPath);
                var files = di.GetFiles();
                foreach (var file in files)
                {
                    file.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        private async Task<ArtigoViewModel> GetArtigoFromExcelRow(IXLRow row)
        {

            var response_empresa = await _mediator.Send(new GetEmpresaByNomeQuery() { Nome = row.Cell(1).Value.ToString() });
            if (!response_empresa.Succeeded) return null;

            var response_gender = await _mediator.Send(new GetGenderByNomeQuery() { Nome = row.Cell(2).Value.ToString() });
            if (!response_gender.Succeeded) return null;

            var artigo = new ArtigoViewModel
            {
                Referencia = row.Cell(3).Value.ToString(),
                EmpresaId = response_empresa.Data.Id,
                GenderId = response_gender.Data.Id,
                Pele = row.Cell(4).Value.ToString(),
                Cor = row.Cell(5).Value.ToString(),
            };

            return artigo;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllArtigosAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var artigosResponse = await mediator.Send(new GetAllArtigosCachedQuery());
            var artigosViewModel = mapper.Map<List<ArtigoViewModel>>(artigosResponse.Data);
            return new SelectList(artigosViewModel, nameof(ArtigoViewModel.Id), nameof(ArtigoViewModel.Referencia), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllArtigosFromEmpresaAsync(int empresaId, int selectedId, IMapper mapper, IMediator mediator)
        {
            var artigosResponse = await mediator.Send(new GetAllArtigosByEmpresaIdCachedQuery() { empresaId = empresaId });
            var artigosViewModel = mapper.Map<List<ArtigoViewModel>>(artigosResponse.Data);
            return new SelectList(artigosViewModel, nameof(ArtigoViewModel.Id), nameof(ArtigoViewModel.Referencia), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task StartProgressBarAsync(string fileName, IHubContext<ProgressHub> hubContext)
        {
            var startMessage = _localizer["Ficheiro:"] + fileName + _localizer["em processamento ..."];
            await ProgressBar.StartProgressBarAsync(startMessage, hubContext);
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task UpdateProgressBarAsync(int idx, int total, IHubContext<ProgressHub> hubContext)
        {
            var percentage = total == 0 ? 0 : idx * 100 / total;
            var progressMessage = _localizer["Artigo #"] + idx + _localizer["de"] + total + _localizer["carregado com sucesso."];
            await ProgressBar.SetProgressBarAsync(progressMessage, percentage, hubContext);
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task StopProgressBarAsync(int totalFiles, IHubContext<ProgressHub> hubContext)
        {
            var endMessage = totalFiles + _localizer["ficheiro(s) Excel carregado(s) com sucesso."];
            await ProgressBar.StopProgressBarAsync(endMessage, hubContext);
        }


        //---------------------------------------------------------------------------------------------------

    }
}