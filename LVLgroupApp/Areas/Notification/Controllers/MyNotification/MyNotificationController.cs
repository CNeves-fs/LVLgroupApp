using Core.Constants;
using Core.Entities.Identity;
using Core.Entities.Notifications;
using Core.Enums;
using Core.Features.Notifications.Commands.Create;
using Core.Features.Notifications.Commands.Delete;
using Core.Features.Notifications.Queries.GetById;
using Core.Features.NotificationsSended.Commands.Create;
using Core.Features.NotificationsSended.Commands.Delete;
using Core.Features.NotificationsSended.Commands.Update;
using Core.Features.NotificationsSended.Queries.GetAllCached;
using Core.Features.NotificationsSended.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Business.Controllers.Mercado;
using LVLgroupApp.Areas.Notification.Controllers.Notification;
using LVLgroupApp.Areas.Notification.Models.MyNotification;
using LVLgroupApp.Areas.Notification.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Notification.Controllers.MyNotification
{
    [Area("Notification")]
    [Authorize]
    public class MyNotificationController : BaseController<MyNotificationController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<MyNotificationController> _localizer;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;


        //---------------------------------------------------------------------------------------------------


        public MyNotificationController(IStringLocalizer<MyNotificationController> localizer, 
                                        SignInManager<ApplicationUser> signInManager, 
                                        UserManager<ApplicationUser> userManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.View)]
        public IActionResult Index()
        {
            var model = new NotificationBoxViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a view com a tabela de notificações referentes a um user.
        /// A tabela irá iniciar o carregamento das notificações
        /// com a chamada a GetNotifications(true).
        /// </summary>
        /// <returns>_ViewAll</returns>

        [Authorize(Policy = Permissions.Notifications.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | MyNotification Contoller - LoadAll - Inicio");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.View)]
        public async Task<JsonResult> OnGetView(int id = 0)
        {
            if (id == 0) return new JsonResult(new { isValid = false, html = string.Empty });

            //obter notification sended
            var notificationSendedResult = await _mediator.Send(new GetNotificationSendedByIdQuery() { Id = id });
            if (!notificationSendedResult.Succeeded)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - OnGetView - Erro ao obter notificação enviada: " + notificationSendedResult.Message);
                return new JsonResult(new { isValid = false, html = string.Empty });
            }
            else
            {
                var notificationSended = _mapper.Map<NotificationSendedViewModel>(notificationSendedResult.Data);

                //obter notificação
                var notificationResult = await _mediator.Send(new GetNotificationByIdQuery() { Id = notificationSended.NotificationId });
                if (!notificationResult.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - OnGetView - Erro ao obter notificação: " + notificationResult.Message);
                    return new JsonResult(new { isValid = false, html = string.Empty });
                }
                else
                {
                    var notification = _mapper.Map<NotificationViewModel>(notificationResult.Data);
                    var fromUser = await NotificationController.GetFromUserDetailsAsync(notification.FromUserId, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);
                    var toUsers = await NotificationController.GetDestinationUsersAsync(notification.Id, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);
                    var viewNotificationViewModel = new ViewNotificationViewModel()
                    {
                        Id = notification.Id,
                        Date = notification.Date,
                        Subject = notification.Subject,
                        Text = notification.Text,
                        FromUser = fromUser,
                        ToUserIds = string.Join(";", toUsers.Select(x => x.Id)),
                        ToUsers = toUsers
                    };
                    // atualizar notificationSended com isRead=true
                    notificationSended.IsRead = true;
                    var updateNotificationSendedCommand = _mapper.Map<UpdateNotificationSendedCommand>(notificationSended);
                    var resultSended = await _mediator.Send(updateNotificationSendedCommand);
                    if (!resultSended.Succeeded || resultSended.Data == 0)
                    {
                        var html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel);
                        return new JsonResult(new { isValid = false, html = html });
                    }

                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_View", viewNotificationViewModel) });
                }

            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.Create)]
        public async Task<JsonResult> OnGetCreate()
        {
            // preparar estrutura de dados para a view
            var viewNotificationViewModel = await GetEmptyViewNotificationViewModel();
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.Create)]
        public async Task<JsonResult> OnPostCreate(ViewNotificationViewModel viewNotificationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //create new Notification
                    var notification = new Core.Entities.Notifications.Notification
                    {
                        FromUserId = _signInManager.UserManager.GetUserId(User),
                        Date = DateTime.Now,
                        Subject = viewNotificationViewModel.Subject,
                        Text = viewNotificationViewModel.Text
                    };
                    var createNotificationCommand = _mapper.Map<CreateNotificationCommand>(notification);
                    var result = await _mediator.Send(createNotificationCommand);
                    if (!result.Succeeded || result.Data == 0)
                    {
                        var html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel);
                        return new JsonResult(new { isValid = false, html = html });
                    }
                    else
                    {
                        // criar array de user Ids a partir do ToUserGroups
                        var arrayUserIds = Array.Empty<string>();

                        if (string.IsNullOrEmpty(viewNotificationViewModel.ToUserGroups) && string.IsNullOrEmpty(viewNotificationViewModel.ToUserIds))
                        {
                            var html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel);
                            return new JsonResult(new { isValid = false, html = html });
                        }

                        if (!string.IsNullOrEmpty(viewNotificationViewModel.ToUserGroups))
                        {
                            // remover último ";" de ToUserGroups
                            var userGroups = viewNotificationViewModel.ToUserGroups.Remove(viewNotificationViewModel.ToUserGroups.Length - 1);
                            var strGroupArray = userGroups.Split(";");

                            foreach (var groupStr in strGroupArray)
                            {
                                arrayUserIds = arrayUserIds.Union(await GetArrayFromGroup(groupStr)).ToArray<string>();
                            }
                        }

                        if(!string.IsNullOrEmpty(viewNotificationViewModel.ToUserIds))
                        {
                            // criar as NotificationSended por cada toUserId
                            // remover último ";" de ToUserIds
                            var usersIds = viewNotificationViewModel.ToUserIds.Remove(viewNotificationViewModel.ToUserIds.Length - 1);
                            var strArray = usersIds.Split(";");
                            arrayUserIds = arrayUserIds.Union(strArray).ToArray<string>();
                        }

                        // criar as NotificationSended por cada userId existente em arrayUserIds
                        foreach (var userId in arrayUserIds)
                        {
                            var notificationSended = new NotificationSended
                            {
                                NotificationId = result.Data,
                                ToUserId = userId,
                                IsRead = false,
                            };

                            var createNotificationSendedCommand = _mapper.Map<CreateNotificationSendedCommand>(notificationSended);
                            var resultSended = await _mediator.Send(createNotificationSendedCommand);
                            if (!resultSended.Succeeded || resultSended.Data == 0)
                            {
                                var html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel);
                                return new JsonResult(new { isValid = false, html = html });
                            }

                        }

                        return new JsonResult(new { isValid = true });
                    }
                }
                else
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel);
                    return new JsonResult(new { isValid = false, html = html });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - OnPostCreate - Exception vai sair e retornar Error: " + ex.Message);
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", viewNotificationViewModel);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.View)]
        public async Task<IActionResult> GetMyNotifications()
        {
            if (User == null)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyNotifications() Error: User is null");
                return new ObjectResult(new { status = "error", count = 0, notifications = "" });
            }

            try
            {
                var userId = _signInManager.UserManager.GetUserId(User);
                var myNotificationList = new List<MyNotificationViewModel>();

                var response = await _mediator.Send(new GetNotificationsSendedByToUserIdCachedQuery() { userId = userId });
                if (!response.Succeeded || response.Data == null)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyNotifications - GetNotificationsSendedByToUserIdCachedQuery() Error: " + response.Message);
                    return new ObjectResult(new { status = "error", count = 0, notifications = "" });
                }
                else
                {
                    // construir lista de MyNotificationViewModel
                    var NotificationsSendedList = _mapper.Map<List<NotificationSendedViewModel>>(response.Data).AsQueryable();
                    foreach (var notificationSended in NotificationsSendedList)
                    {
                        var myInBoxNotification = await GetMyInBoxNotificationViewModel(notificationSended);
                        if (myInBoxNotification == null)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyNotifications - GetMyInBoxNotificationViewModel - Error: myInBoxNotification is null for notification: " + notificationSended.NotificationId);
                        }
                        else
                        {
                            myNotificationList.Add(myInBoxNotification);
                        }
                    }
                }

                myNotificationList = myNotificationList.OrderByDescending(n => n.Date).ToList();
                var jsonData = new { status = "success", count = myNotificationList.Count(), notifications = myNotificationList };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyNotifications - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error", count = 0, notifications = "" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.Delete)]
        public async Task<IActionResult> DeleteNotification(int id = 0)
        {
            if (User == null)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - DeleteNotification() Error: User is null");
                return new ObjectResult(new { status = "error", count = 0, notifications = "" });
            }

            if (id == 0)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - DeleteNotification() Error: Invalid Id = 0");
                return new ObjectResult(new { status = "error", count = 0, notifications = "" });
            }

            try
            {
                //var userId = _signInManager.UserManager.GetUserId(User);

                var response = await _mediator.Send(new GetNotificationSendedByIdQuery() { Id = id });
                if (!response.Succeeded || response.Data == null)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - DeleteNotification - GetNotificationSendedByIdQuery() Error: " + response.Message);
                    return new ObjectResult(new { status = "error" });
                }
                else
                {
                    // NotificationSended com o id
                    var NotificationsSended = _mapper.Map<NotificationSendedViewModel>(response.Data);

                    // delete da NotificationSended
                    var result = await _mediator.Send(new DeleteNotificationSendedCommand() { Id = NotificationsSended.Id });
                    if (!result.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - DeleteNotification - DeleteNotificationSendedCommand() Error: " + result.Message);
                        return new ObjectResult(new { status = "error" });
                    }

                    // verificar se é o ultimo user a ler a notificação
                    var responseSendedList = await _mediator.Send(new GetNotificationsSendedByNotificationIdCachedQuery() { NotificationId = NotificationsSended.NotificationId });
                    if (!responseSendedList.Succeeded || responseSendedList.Data == null)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - DeleteNotification - GetNotificationsSendedByNotificationIdCachedQuery() Error: " + responseSendedList.Message);
                        return new ObjectResult(new { status = "error" });
                    }
                    var sendedList = _mapper.Map<List<NotificationSendedViewModel>>(responseSendedList.Data);
                    if (sendedList.Count == 0)
                    {
                        // delete da Notification
                        var resultNotification = await _mediator.Send(new DeleteNotificationCommand() { Id = NotificationsSended.NotificationId });
                        if (!resultNotification.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - DeleteNotification - DeleteNotificationCommand() Error: " + resultNotification.Message);
                            return new ObjectResult(new { status = "error" });
                        }
                    }
                }
                var jsonData = new { status = "success" };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyNotifications - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error", count = 0, notifications = "" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara 
        /// a tabela de notificações é carregada com esta lista em _ViewAll
        /// é a própria tabela que faz o ajax call a GetNotifications().
        /// </summary>
        /// <returns>ViewNotificationViewModel</returns>

        internal async Task<ViewNotificationViewModel> GetEmptyViewNotificationViewModel()
        {
            if (User == null) return null;

            try
            { 
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                var RolesList = roles.ToList();
                var userId = user.Id;

                var fromUserDetails = await NotificationController.GetFromUserDetailsAsync(userId, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);
                var myViewNotificationViewModel = new ViewNotificationViewModel()
                {
                    Subject = string.Empty,
                    Text = string.Empty,
                    Date = DateTime.Now,
                    FromUser = new FromUserDetailViewModel()
                    {
                        Id = fromUserDetails.Id,
                        Name = fromUserDetails.Name,
                        Email = fromUserDetails.Email,
                        ProfilePicture = fromUserDetails.ProfilePicture,
                        RoleName = string.IsNullOrEmpty(fromUserDetails.RoleName) ? "" : fromUserDetails.RoleName,
                        Local = string.IsNullOrEmpty(fromUserDetails.Local) ? "" : fromUserDetails.Local
                    },
                    ToUserIds = string.Empty,
                    ToUsers = new List<ToUserDetailViewModel>(),
                    EmpresaId = user.EmpresaId == null ? 0 : (int)user.EmpresaId,
                    GrupolojaId = user.GrupolojaId == null ? 0 : (int)user.GrupolojaId,
                    LojaId = user.LojaId == null ? 0 : (int)user.LojaId,
                    IsSuperAdmin = roles.Contains(Roles.SuperAdmin.ToString()),
                    IsAdmin = roles.Contains(Roles.Admin.ToString()),
                    IsRevisor = roles.Contains(Roles.Revisor.ToString()),
                    IsSupervisor = roles.Contains(Roles.Supervisor.ToString()),
                    IsGerenteLoja = roles.Contains(Roles.GerenteLoja.ToString()),
                    IsColaborador = roles.Contains(Roles.Colaborador.ToString()),
                    IsBasic = roles.Contains(Roles.Basic.ToString())
                };

                myViewNotificationViewModel.MercadoId = await MercadoController.GetMercadoIdAsync(myViewNotificationViewModel.LojaId, _mapper, _mediator);
                
                myViewNotificationViewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(myViewNotificationViewModel.MercadoId, _mapper, _mediator);
                myViewNotificationViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(myViewNotificationViewModel.EmpresaId, _mapper, _mediator);
                myViewNotificationViewModel.Gruposlojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(myViewNotificationViewModel.EmpresaId, myViewNotificationViewModel.GrupolojaId, _mapper, _mediator);
                myViewNotificationViewModel.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(myViewNotificationViewModel.GrupolojaId, myViewNotificationViewModel.LojaId, _mapper, _mediator);

                return myViewNotificationViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetEmptyViewNotificationViewModel - Exception: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// converte a string com a descrição de um grupo num
        /// array de user Ids.
        /// </summary>
        /// <returns>string[]</returns>
        
        internal async Task<string[]> GetArrayFromGroup(string ToUserGroup)
        {
            // ex. "Colaboradores*0*1*1*15"
            var arrayUserIds = new string[] { };
            if (string.IsNullOrEmpty(ToUserGroup)) return arrayUserIds;
            var arrayGroup = ToUserGroup.Split('*');
            if (arrayGroup.Length < 5) return arrayUserIds;
            
            // defenição do grupo de destino
            var lojaId = int.Parse(arrayGroup[4]);
            var grupolojaId = int.Parse(arrayGroup[3]);
            var empresaId = int.Parse(arrayGroup[2]);
            var mercadoId = int.Parse(arrayGroup[1]);
            var role = arrayGroup[0];
            var roleName = "";

            switch (role)
            {
                case "SuperAdmins":
                roleName = Roles.SuperAdmin.ToString();
                break;
            case "Administradores":
                roleName = Roles.Admin.ToString();
                break;
            case "Revisores":
                roleName = Roles.Revisor.ToString();
                break;
            case "Supervisores":
                roleName = Roles.Supervisor.ToString();
                break;
            case "Gerentes de lojas":
                roleName = Roles.GerenteLoja.ToString();
                break;
            case "Colaboradores":
                roleName = Roles.Colaborador.ToString();
                break;
            default:
                roleName = "";
                    break;
            }

            // obter todos os users do role
            var allusers = await _userManager.GetUsersInRoleAsync(roleName);
            if(allusers == null) return arrayUserIds;

            // SuperAdmins, Administradores e Revisores não têm loja de referência
            if (roleName == Roles.SuperAdmin.ToString() || roleName == Roles.Admin.ToString() || roleName == Roles.Revisor.ToString())
            {
                arrayUserIds = allusers.Select(u => u.Id).ToArray();
                return arrayUserIds;
            };

            // Supervisores têm referência apenas ao GrupoLoja
            if (roleName == Roles.Supervisor.ToString())
            {
                if (grupolojaId > 0)
                {
                    // todos os supervisores deste agrupamento grupolojaId
                    allusers = allusers.Where(u => u.GrupolojaId == grupolojaId).ToList();
                    if (allusers == null) return arrayUserIds;
                    arrayUserIds = allusers.Select(u => u.Id).ToArray();
                    return arrayUserIds;
                }
                if (empresaId > 0)
                {
                    // todos os supervisores desta empresa empresaId
                    allusers = allusers.Where(u => u.EmpresaId == empresaId).ToList();
                    if (allusers == null) return arrayUserIds;
                    arrayUserIds = allusers.Select(u => u.Id).ToArray();
                    return arrayUserIds;
                }
                if (mercadoId > 0)
                {
                    allusers = allusers.Where(u => u.MercadoId == mercadoId).ToList();
                    if (allusers == null) return arrayUserIds;
                    arrayUserIds = allusers.Select(u => u.Id).ToArray();
                    return arrayUserIds;
                }
                return arrayUserIds;
            };

            // Colaboradores ou Gerentes de loja 
            if (lojaId > 0)
            {          
                var usersList = allusers.Where(u => u.LojaId == lojaId).ToList();
                if (usersList == null) return arrayUserIds;
                arrayUserIds = usersList.Select(u => u.Id).ToArray();
                return arrayUserIds;
            }
            return arrayUserIds;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara 
        /// a tabela de notificações é carregada com esta lista em _ViewAll
        /// é a própria tabela que faz o ajax call a GetNotifications().
        /// </summary>
        /// <returns>ViewNotificationViewModel</returns>

        internal async Task<MyNotificationViewModel> GetMyInBoxNotificationViewModel(NotificationSendedViewModel notificationSended)
        {
            if (notificationSended == null) return null;

            try
            {
                var response = await _mediator.Send(new GetNotificationByIdQuery() { Id = notificationSended.NotificationId });
                if (!response.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyInBoxNotificationViewModel - GetNotificationByIdQuery - Error: " + response.Message);
                    return null;
                }
                var notification = _mapper.Map<NotificationViewModel>(response.Data);
                var fromUserDetails = await NotificationController.GetFromUserDetailsAsync(notification.FromUserId, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);

                var myNotificationViewModel = new MyNotificationViewModel()
                {
                    NotificationId = notification.Id,
                    Date = notification.Date,
                    Subject = notification.Subject,
                    Text = notification.Text,
                    FromUser = new FromUserDetailViewModel()
                    {
                        Id = fromUserDetails.Id,
                        Name = fromUserDetails.Name,
                        Email = fromUserDetails.Email,
                        ProfilePicture = fromUserDetails.ProfilePicture,
                        RoleName = fromUserDetails.RoleName,
                        Local = fromUserDetails.Local
                    },
                    NotificationSended = new NotificationSendedViewModel()
                    {
                        Id = notificationSended.Id,
                        NotificationId = notificationSended.NotificationId,
                        ToUserId = notificationSended.ToUserId,
                        IsRead = notificationSended.IsRead
                    }
                };

                return myNotificationViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | MyNotification Contoller - GetMyInBoxNotificationViewModel - Exception: " + ex.Message);
                return null;
            }

        }


        //---------------------------------------------------------------------------------------------------

    }
}
