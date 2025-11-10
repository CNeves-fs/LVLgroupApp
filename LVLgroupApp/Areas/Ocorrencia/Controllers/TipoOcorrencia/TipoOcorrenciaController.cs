using AutoMapper;
using Core.Constants;
using Core.Enums;
using Core.Features.NotificacoesOcorrencias.Commands.Create;
using Core.Features.NotificacoesOcorrencias.Commands.Delete;
using Core.Features.NotificacoesOcorrencias.Queries.GetAllCached;
using Core.Features.TiposOcorrencias.Commands.Create;
using Core.Features.TiposOcorrencias.Commands.Delete;
using Core.Features.TiposOcorrencias.Queries.GetAllCached;
using Core.Features.TiposOcorrencias.Queries.GetById;
using Core.Features.TiposOcorrenciasLocalized.Commands.Create;
using Core.Features.TiposOcorrenciasLocalized.Commands.Update;
using Core.Features.TiposOcorrenciasLocalized.Queries.GetAllCached;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Ocorrencia.Controllers.Ocorrencia;
using LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Ocorrencia.Controllers.TipoOcorrencia
{
    [Area("Ocorrencia")]
    [Authorize]
    public class TipoOcorrenciaController : BaseController<TipoOcorrenciaController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<TipoOcorrenciaController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaController(IStringLocalizer<TipoOcorrenciaController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.TiposOcorrencias.View)]
        public IActionResult Index()
        {
            var model = new TipoOcorrenciaViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.TiposOcorrencias.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllTiposOcorenciasCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<TipoOcorrenciaViewModel>>(response.Data);

                // adicionar Translations ao ViewModel
                foreach (var item in viewModel)
                {
                    item.Translations = new List<TipoOcorrenciaLocalizedViewModel>();
                    var tiposOcorrenciasLocalizedResponse = await _mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = item.Id });
                    item.Translations = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposOcorrenciasLocalizedResponse.Data);
                    item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                    item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                    item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
                }

                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.TiposOcorrencias.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo TipoOcorrencia
            {
                var tipoOcorrenciaViewModel = new TipoOcorrenciaViewModel();
                tipoOcorrenciaViewModel.EditMode = false;
                tipoOcorrenciaViewModel.Categorias = OcorrenciaController.GetSelectListAllCategorias(0, _culture);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", tipoOcorrenciaViewModel) });
            }
            else // Editar TipoOcorrencia
            {
                var response = await _mediator.Send(new GetTipoOcorrenciaByIdQuery() { Id = id });
                var tipoOcorrenciaViewModel = _mapper.Map<TipoOcorrenciaViewModel>(response.Data);
                tipoOcorrenciaViewModel.EditMode = true;
                tipoOcorrenciaViewModel.Categorias = OcorrenciaController.GetSelectListAllCategorias(tipoOcorrenciaViewModel.CategoriaId, _culture);

                // adicionar Translations ao ViewModel
                tipoOcorrenciaViewModel.Translations = new List<TipoOcorrenciaLocalizedViewModel>();
                var tiposOcorrenciasLocalizedResponse = await _mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipoOcorrenciaViewModel.Id });
                tipoOcorrenciaViewModel.Translations = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposOcorrenciasLocalizedResponse.Data);
                tipoOcorrenciaViewModel.EsName = tipoOcorrenciaViewModel.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                tipoOcorrenciaViewModel.EnName = tipoOcorrenciaViewModel.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                tipoOcorrenciaViewModel.DefaultName = tipoOcorrenciaViewModel.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();

                // adicionar Notifications ao ViewModel
                var responseNotif = await _mediator.Send(new GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = id });
                if (responseNotif.Succeeded)
                {
                    var notifOcorrenciaList = _mapper.Map<List<NotificacaoOcorrenciaViewModel>>(responseNotif.Data);
                    foreach (var notif in notifOcorrenciaList)
                    {
                        if (notif.TipoDestino == NotificationDestinationType.ToSingleUser)
                        {
                            tipoOcorrenciaViewModel.ToUserIds += notif.ApplicationUserId + ";";
                            tipoOcorrenciaViewModel.ToUserEmails += notif.ApplicationUserEmail + ";";
                        }
                        else
                        {
                            tipoOcorrenciaViewModel.ToUserGroups += notif.TipoDestino + ";";
                        }
                    }
                }

                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", tipoOcorrenciaViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.TiposOcorrencias.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, TipoOcorrenciaViewModel tipoOcorrencia)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new TipoOcorrencia
                    var createTipoOcorrenciaCommand = _mapper.Map<CreateTipoOcorrenciaCommand>(tipoOcorrencia);
                    var result = await _mediator.Send(createTipoOcorrenciaCommand);
                    if (result.Succeeded)
                    {
                        // criar translations
                        var tipoOcorrenciaId = result.Data;
                        // pt
                        var createPtTOLCommand = new CreateTipoOcorrenciaLocalizedCommand();
                        createPtTOLCommand.Name = tipoOcorrencia.DefaultName;
                        createPtTOLCommand.Language = "pt";
                        createPtTOLCommand.TipoOcorrenciaId = tipoOcorrenciaId;
                        var resultPT = await _mediator.Send(createPtTOLCommand);
                        // es
                        var createEsTOLCommand = new CreateTipoOcorrenciaLocalizedCommand();
                        createEsTOLCommand.Name = tipoOcorrencia.EsName;
                        createEsTOLCommand.Language = "es";
                        createEsTOLCommand.TipoOcorrenciaId = tipoOcorrenciaId;
                        var resultEs = await _mediator.Send(createEsTOLCommand);
                        // en
                        var createEnTOLCommand = new CreateTipoOcorrenciaLocalizedCommand();
                        createEnTOLCommand.Name = tipoOcorrencia.EnName;
                        createEnTOLCommand.Language = "en";
                        createEnTOLCommand.TipoOcorrenciaId = tipoOcorrenciaId;
                        var resultEn = await _mediator.Send(createEnTOLCommand);

                        // criar notificações a partir do ToUserGroups
                        var destNotifsGroups = await CreateNotificationOcorr(tipoOcorrenciaId, tipoOcorrencia.ToUserGroups, string.Empty, true);

                        // criar notificações a partir do ToUserIds
                        var destNotifsUsers = await CreateNotificationOcorr(tipoOcorrenciaId, tipoOcorrencia.ToUserIds, tipoOcorrencia.ToUserEmails, false);

                        id = result.Data;
                        _notify.Success($"{_localizer["Tipo de Ocorrência com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update TipoOcorrencia
                    await UpdateTipoOcorrenciaLocalized(id, tipoOcorrencia);

                    //update NotificaçõesOcorrencia
                    await UpdateNotificationOcorr(id, tipoOcorrencia.ToUserGroups, tipoOcorrencia.ToUserIds, tipoOcorrencia.ToUserEmails);

                    _notify.Information($"{_localizer["Tipo de Ocorrência com ID"]} {id} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllTiposOcorenciasCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<TipoOcorrenciaViewModel>>(response.Data);
                    // adicionar Translations ao ViewModel
                    foreach (var item in viewModel)
                    {
                        item.Translations = new List<TipoOcorrenciaLocalizedViewModel>();
                        var tiposOcorrenciasLocalizedResponse = await _mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = item.Id });
                        item.Translations = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposOcorrenciasLocalizedResponse.Data);
                        item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                        item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                        item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", tipoOcorrencia);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.TiposOcorrencias.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteTipoOcorrenciaCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Tipo de ocorrência com ID"]} {id} {_localizer[" removido."]}");

                // return _ViewAll
                var viewModel = new List<TipoOcorrenciaViewModel>();
                var response = await _mediator.Send(new GetAllTiposOcorenciasCachedQuery());
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<TipoOcorrenciaViewModel>>(response.Data);
                    // adicionar Translations ao ViewModel
                    foreach (var item in viewModel)
                    {
                        item.Translations = new List<TipoOcorrenciaLocalizedViewModel>();
                        var tiposOcorrenciasLocalizedResponse = await _mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = item.Id });
                        item.Translations = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposOcorrenciasLocalizedResponse.Data);
                        item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                        item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                        item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
                    }

                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
            }
            else
            {
                _notify.Error(deleteCommand.Message);
                return new JsonResult(new { isValid = true, html = "" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma lista de Tipos de Ocorrencias em Json
        /// </summary>
        /// <returns>List<TipoOcorrenciaViewModel></returns>

        public async Task<JsonResult> LoadTipoOcorrencias()
        {
            var response = await _mediator.Send(new GetAllTiposOcorenciasCachedQuery());
            if (response.Succeeded)
            {
                var allTiposOcorrencias = _mapper.Map<List<TipoOcorrenciaViewModel>>(response.Data);
                // adicionar Translations ao allTiposOcorrencias
                foreach (var item in allTiposOcorrencias)
                {
                    item.Translations = new List<TipoOcorrenciaLocalizedViewModel>();
                    var tiposOcorrenciasLocalizedResponse = await _mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = item.Id });
                    item.Translations = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposOcorrenciasLocalizedResponse);
                    item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                    item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                    item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
                }

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { tipoocorrenciasList = allTiposOcorrencias });
                return Json(jsonString);

            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria as notificações para os destinatários associados ao 
        /// tipo da ocorrência
        /// </summary>
        /// <returns>success=bool</returns>

        internal async Task<bool> CreateNotificationOcorr(int ocorrenciaId, string strGroup, string strEmail, bool group)
        {
            // criar arrays de strings
            var arrayUserDest = Array.Empty<string>();
            var arrayEmailDest = Array.Empty<string>();
            var userDest = string.Empty;
            var strEmailDest = string.Empty;

            if (ocorrenciaId > 0 && !string.IsNullOrEmpty(strGroup))
            {
                // remover último ";" de strGroup
                userDest = strGroup.Remove(strGroup.Length - 1);
                arrayUserDest = userDest.Split(";");

                // remover último ";" de strEmail
                if (!group)
                {
                    strEmailDest = strEmail.Remove(strEmail.Length - 1);
                    arrayEmailDest = strEmailDest.Split(";");
                }

                // criar NotificaçãoOcorrencia
                for (var i = 0; i < arrayUserDest.Length; i++)
                {
                    var createNotifOcorrCommand = new CreateNotificacaoOcorrenciaCommand() { TipoOcorrenciaId = ocorrenciaId };
                    if (group)
                    {
                        var destType = new NotificationDestinationType();
                        var success = Enum.TryParse(arrayUserDest[i], out destType);
                        if (success) createNotifOcorrCommand.TipoDestino = destType;
                        createNotifOcorrCommand.ApplicationUserId = null;
                        createNotifOcorrCommand.ApplicationUserEmail = null;
                    }
                    else
                    {
                        if (arrayUserDest.Length != arrayEmailDest.Length) return false;
                        createNotifOcorrCommand.TipoDestino = NotificationDestinationType.ToSingleUser;
                        createNotifOcorrCommand.ApplicationUserId = arrayUserDest[i];
                        createNotifOcorrCommand.ApplicationUserEmail = arrayEmailDest[i];
                    }

                    var result = await _mediator.Send(createNotifOcorrCommand);

                }
                return true;
            }
            else
            {
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza as NotificaçõesOcorrencia de um TipoOcorrencia
        /// verifica as que existem na db e as que são passadas do cliente
        /// remove as que já não são necessárias e cria as que faltam
        /// </summary>
        /// <returns>success=bool</returns>

        internal async Task<bool> UpdateNotificationOcorr(int tipoOcorrenciaId, string strGroup, string strUserId, string strEmail)
        {
            // criar arrays de strings
            var arrayGroupDest = Array.Empty<string>();
            var arrayUserIdDest = Array.Empty<string>();
            var arrayEmailDest = Array.Empty<string>();

            if (!string.IsNullOrEmpty(strGroup))
            {
                // remover último ";" de ToUserGroups
                var userGroups = strGroup.Remove(strGroup.Length - 1);
                arrayGroupDest = userGroups.Split(";");
            }
            if (!string.IsNullOrEmpty(strUserId))
            {
                // remover último ";" de ToUserIds
                var userIds = strUserId.Remove(strUserId.Length - 1);
                arrayUserIdDest = userIds.Split(";");
            }
            if (!string.IsNullOrEmpty(strEmail))
            {
                // remover último ";" de ToUserEmails
                var userEmails = strEmail.Remove(strEmail.Length - 1);
                arrayEmailDest = userEmails.Split(";");
            }

            //ler da db todas as NotificacaoOcorrencia relativas a este TipoOcorrencia
            var notifOcorrListToDelete = new List<NotificacaoOcorrenciaViewModel>();
            var notifOcorrListToMantain = new List<NotificacaoOcorrenciaViewModel>();
            var notifOcorrListQuery = new GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipoOcorrenciaId };
            var notifOcorrListResult = await _mediator.Send(notifOcorrListQuery);
            if (notifOcorrListResult.Succeeded)
            {
                //lista com notifOcorr que podem ter que ser removidas da db
                notifOcorrListToDelete = _mapper.Map<List<NotificacaoOcorrenciaViewModel>>(notifOcorrListResult.Data);
            }

            //verificar se as novas notifOcorr já existem
            for (var i = 0; i < arrayGroupDest.Length; i++)
            {
                var existNotifOcorr1 = notifOcorrListToDelete.Where(n => n.TipoDestino.ToString() == arrayGroupDest[i]).FirstOrDefault();
                var deleted = notifOcorrListToDelete.Remove(existNotifOcorr1);
                if (deleted) notifOcorrListToMantain.Add(existNotifOcorr1);
            }
            for (var i = 0; i < arrayUserIdDest.Length; i++)
            {
                var existNotifOcorr2 = notifOcorrListToDelete.Where(n => n.ApplicationUserId == arrayUserIdDest[i]).FirstOrDefault();
                var deleted = notifOcorrListToDelete.Remove(existNotifOcorr2);
                if (deleted) notifOcorrListToMantain.Add(existNotifOcorr2);
            }

            //verificar se é necessário criar mais notifOcorr
            for (var i = 0; i < arrayGroupDest.Length; i++)
            {
                var existNotifOcorr3 = notifOcorrListToMantain.Where(n => n.TipoDestino.ToString() == arrayGroupDest[i]).FirstOrDefault();
                if (existNotifOcorr3 == null)
                {
                    var createNotifOcorrCommand = new CreateNotificacaoOcorrenciaCommand() { TipoOcorrenciaId = tipoOcorrenciaId };
                    var destType = new NotificationDestinationType();
                    var success = Enum.TryParse(arrayGroupDest[i], out destType);
                    if (success) createNotifOcorrCommand.TipoDestino = destType;
                    createNotifOcorrCommand.ApplicationUserId = null;
                    createNotifOcorrCommand.ApplicationUserEmail = null;
                    var resultCNOC = await _mediator.Send(createNotifOcorrCommand);
                }
            }
            for (var i = 0; i < arrayUserIdDest.Length; i++)
            {
                var existNotifOcorr4 = notifOcorrListToMantain.Where(n => n.ApplicationUserId == arrayUserIdDest[i]).FirstOrDefault();
                if (existNotifOcorr4 == null)
                {
                    var createNotifOcorrCommand = new CreateNotificacaoOcorrenciaCommand() { TipoOcorrenciaId = tipoOcorrenciaId };
                    createNotifOcorrCommand.TipoDestino = NotificationDestinationType.ToSingleUser;
                    createNotifOcorrCommand.ApplicationUserId = arrayUserIdDest[i];
                    createNotifOcorrCommand.ApplicationUserEmail = arrayEmailDest[i];
                    var resultCNOC = await _mediator.Send(createNotifOcorrCommand);
                }
            }

            //verificar se é necessário remover alguma notifOcorr
            foreach (var notifOcorr in notifOcorrListToDelete)
            {
                var notifOcorrToDelete = new DeleteNotificacaoOcorrenciaCommand() { Id = notifOcorr.Id };
                var resultDelete = await _mediator.Send(notifOcorrToDelete);
            }

            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza as strings de tradução de um TipoOcorrencia
        /// </summary>
        /// <returns>bool</returns>

        internal async Task<bool> UpdateTipoOcorrenciaLocalized(int tipoOcorrenciaId, TipoOcorrenciaViewModel tipoOcorr)
        {
            // ler as traduções existentes
            var ocorrLocalizedListQuery = new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipoOcorrenciaId };
            var ocorrLocalizedListResult = await _mediator.Send(ocorrLocalizedListQuery);
            if (ocorrLocalizedListResult.Succeeded)
            {
                var ocorrLocalizedList = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(ocorrLocalizedListResult.Data);

                //atualizar cada uma das traduções
                foreach (var trad in ocorrLocalizedList)
                {
                    switch (trad.Language)
                    {
                        case "pt":
                            trad.Name = tipoOcorr.DefaultName;
                            break;
                        case "es":
                            trad.Name = tipoOcorr.EsName;
                            break;
                        case "en":
                            trad.Name = tipoOcorr.EnName;
                            break;
                    }

                    //atualizar a tradução
                    var updateTOLCommand = _mapper.Map<UpdateTipoOcorrenciaLocalizedCommand>(trad);
                    var result = await _mediator.Send(updateTOLCommand);
                }
                return true;
            }
            return false;
        }


        //---------------------------------------------------------------------------------------------------

        /// <summary>
        /// converte a string com a descrição de um grupo num
        /// array de destinations.
        /// cria um NotificaçãoOcorrencia por cada destination
        /// </summary>
        /// <returns>string[]</returns>
        
        public static async Task<SelectList> GetSelectListFromCategoriaAsync(int categoriaId, int selectedId, IMapper mapper, IMediator mediator, IRequestCultureFeature culture)
        {
            // Culture contains the information of the requested culture
            var lang = culture.RequestCulture.Culture;
            var tiposViewModel = new List<TipoOcorrenciaLocalizedViewModel>();

            if (categoriaId == 0) return null;

            var tiposResponse = await mediator.Send(new GetTiposOcorrenciasByCategoriaIdQuery() { CategoriaId = categoriaId });
            if (!tiposResponse.Succeeded || tiposResponse.Data == null) return null;

            foreach (var tipo in tiposResponse.Data)
            {
                var tiposLocalizedResponse = await mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipo.Id });
                var tiposLocalized = tiposLocalizedResponse.Data.Where(t => t.Language == lang.Name).ToList();
                var tiposLocalizedViewModel = mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposLocalized);
                tiposViewModel.AddRange(tiposLocalizedViewModel);
            }

            return new SelectList(tiposViewModel, nameof(TipoOcorrenciaLocalizedViewModel.TipoOcorrenciaId), nameof(TipoOcorrenciaLocalizedViewModel.Name), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma SelectList de Tipos de Ocorrencias localized em Json
        /// </summary>
        /// <returns>List<TipoOcorrenciaViewModel></returns>
        
        public async Task<JsonResult> LoadTiposInCategoria(int categoriaId)
        {
            // Culture contains the information of the requested culture
            var lang = _culture.RequestCulture.Culture;
            var tiposViewModel = new List<TipoOcorrenciaLocalizedViewModel>();

            if (categoriaId > 0)
            {
                var tiposResponse = await _mediator.Send(new GetTiposOcorrenciasByCategoriaIdQuery() { CategoriaId = categoriaId });

                if (tiposResponse.Succeeded && tiposResponse.Data != null)
                {
                    foreach(var tipo in tiposResponse.Data)
                    {
                        var tiposLocalizedResponse = await _mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipo.Id });
                        var tiposLocalized = tiposLocalizedResponse.Data.Where(t => t.Language == lang.Name).ToList();
                        var tiposLocalizedViewModel = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(tiposLocalized);
                        tiposViewModel.AddRange(tiposLocalizedViewModel);
                    }

                    var tipos = new SelectList(tiposViewModel, nameof(TipoOcorrenciaLocalizedViewModel.TipoOcorrenciaId), nameof(TipoOcorrenciaLocalizedViewModel.Name), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { tiposList = tipos });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve o nome localized do tipoOcorrenciaId
        /// </summary>
        /// <returns>string=nome</returns>
        
        public static async Task<string> GetTipoOcorrenciaNomeAsync(int tipoOcorrenciaId, string lang, IMediator mediator, IMapper mapper)
        {
            var tiposLocalizedResponse = await mediator.Send(new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipoOcorrenciaId });
            var tipoLocalized = tiposLocalizedResponse.Data.Where(t => t.Language == lang).FirstOrDefault();
            var tipoLocalizedViewModel = mapper.Map<TipoOcorrenciaLocalizedViewModel>(tipoLocalized);

            return tipoLocalizedViewModel.Name;
        }


        //---------------------------------------------------------------------------------------------------

    }
}