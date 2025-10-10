using AutoMapper;
using Core.Constants;
using Core.Entities.Identity;
using Core.Enums;
using Core.Entities.Notifications;
using Core.Features.Notifications.Commands.Create;
using Core.Features.Notifications.Commands.Delete;
using Core.Features.Notifications.Queries.GetAllCached;
using Core.Features.Notifications.Queries.GetById;
using Core.Features.Notifications.Response;
using Core.Features.NotificationsSended.Commands.Create;
using Core.Features.NotificationsSended.Commands.Delete;
using Core.Features.NotificationsSended.Queries.GetAllCached;
using Core.Features.NotificationsSended.Response;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Notification.Models.Notification;
using MediatR;
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
using Infrastructure.Data.DbContext;
using Infrastructure.Data.Seeds;

namespace LVLgroupApp.Areas.Notification.Controllers.Notification
{
    [Area("Notification")]
    [Authorize]
    public class NotificationController : BaseController<NotificationController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<NotificationController> _localizer;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;


        //---------------------------------------------------------------------------------------------------


        public NotificationController(IStringLocalizer<NotificationController> localizer, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.View)]
        public IActionResult Index()
        {
            //var model = new NotificationViewModel();
            return View();
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a view com a tabela de todas as notificações.
        /// A tabela irá iniciar o carregamento das notificações
        /// com a chamada a GetNotifications().
        /// </summary>
        /// <returns>_ViewAll</returns>

        [Authorize(Policy = Permissions.Notifications.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Notification Contoller - LoadAll - Inicio");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de todas as notificações para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Notifications.View)]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // lista de notificações não lidas do current user
                var allNotifications = await GetAllNotificationsListAsync();


                // filtrar searchValue
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allNotifications = allNotifications.Where(n => n.Subject.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                                   n.Text.Contains(searchValue, StringComparison.OrdinalIgnoreCase));
                }

                // ordenar lista
                // var sortedNotificationsData = allNotifications.AsQueryable();
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    allNotifications = allNotifications.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // retornar lista para a datatable
                recordsTotal = allNotifications.Count();
                var data = allNotifications.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - GetClaims - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Notifications.View)]
        public async Task<JsonResult> OnGetView(int id = 0)
        {
            if (id == 0) return new JsonResult(new { isValid = false, html = string.Empty });

            //obter notificação
            var notificationResult = await _mediator.Send(new GetNotificationByIdQuery() { Id = id });
            if (!notificationResult.Succeeded)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Notification Contoller - OnGetView - Erro ao obter notificação: " + notificationResult.Message);
                return new JsonResult(new { isValid = false, html = string.Empty });
            }
            else
            {
                var notification = _mapper.Map<NotificationViewModel>(notificationResult.Data);
                var fromUser = await GetFromUserDetailsAsync(notification.FromUserId, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);
                var toUsers = await GetDestinationUsersAsync(notification.Id, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);
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

                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_View", viewNotificationViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Notifications.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            if (id == 0) return new JsonResult(new { isValid = false, html = string.Empty });

            var deleteCommand = await _mediator.Send(new DeleteNotificationCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Notificação com ID"]} {id} {_localizer[" removida."]}");
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ViewNotificationViewModel() );
                return new JsonResult(new { isValid = true, html = html });

            }
            else
            {
                _notify.Error(deleteCommand.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Notifications.Delete)]
        public async Task<JsonResult> OnPostCleanAsync()
        {
            try
            {
                var responseAllNotifications = await _mediator.Send(new GetNotificationsCachedQuery());
                if (!responseAllNotifications.Succeeded) return Json(new { status = "error" });
                var allNotifications = responseAllNotifications.Data.AsQueryable();
                var oldNotifications = allNotifications.Where(l => l.Date < DateTime.Now.AddDays(-60));

                foreach (NotificationCachedResponse notification in oldNotifications)
                {
                    var responseAllNotificationsSended = await _mediator.Send(new GetNotificationsSendedByNotificationIdCachedQuery() { NotificationId = notification.Id});
                    if (!responseAllNotificationsSended.Succeeded) return Json(new { status = "error" });
                    var allNotificationsSended = responseAllNotificationsSended.Data.AsQueryable();
                    foreach (NotificationSendedCachedResponse notificationSended in allNotificationsSended)
                    {
                        var deleteNotificationSendedCommand = await _mediator.Send(new DeleteNotificationSendedCommand() { Id = notificationSended.Id });
                    }
                    var deleteNotificationCommand = await _mediator.Send(new DeleteNotificationCommand() { Id = notification.Id });
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Notification Contoller - OnPostCleanAsync - Notification deleted=" + notification.Id);
                }

                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | AuditLOg Contoller - OnPostCleanAsync - Exception vai sair e retornar Error: " + ex.Message);
                return Json(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// envia uma notificação para uma lista de destinatários
        /// </summary>
        /// <returns>success = bool</returns>
        
        public static async Task<bool> SendNotification(Core.Entities.Notifications.Notification notification, List<string> userIds, IMapper mapper, IMediator mediator, ILogger logger)
        {
            try
            {
                //Notification is valid
                if (notification == null || userIds == null) return false;
                if (userIds.Count == 0) return false;

                //criar Notification
                var createNotificationCommand = mapper.Map<CreateNotificationCommand>(notification);
                var result = await mediator.Send(createNotificationCommand);
                if (!result.Succeeded || result.Data == 0) return false;

                // criar as NotificationSended por cada userId existente em userIds
                foreach (var userId in userIds)
                {
                    var notificationSended = new NotificationSended()
                    {
                        NotificationId = result.Data,
                        ToUserId = userId,
                        IsRead = false,
                    };

                    var createNotificationSendedCommand = mapper.Map<CreateNotificationSendedCommand>(notificationSended);
                    var resultSended = await mediator.Send(createNotificationSendedCommand);
                    //if (!resultSended.Succeeded || resultSended.Data == 0)
                    //{
                    //    return false;
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Notification Contoller - SendNotification - Exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara a lista de todas as notificações do sistema
        /// a tabela de notificações é carregada com esta lista em _ViewAll
        /// é a própria tabela que faz o ajax call a GetNotifications().
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<ViewNotificationViewModel>> GetAllNotificationsListAsync()
        {
            // lista de todas as notificações do sistema lidas e não lidas
            var allViewNotificationsList = new List<ViewNotificationViewModel>();

            var response = await _mediator.Send(new GetNotificationsCachedQuery());
            if (response.Succeeded)
            {
                // lista de notificações a percorrer
                var allNotificationsList = _mapper.Map<List<NotificationViewModel>>(response.Data).AsQueryable();
                
                // para cada notificação, obter o user que enviou e os users que a receberam
                foreach (var item in allNotificationsList)
                {
                    // preencher viewNotification para adionar à lista allViewNotificationsList
                    var fromUser = await GetFromUserDetailsAsync(item.FromUserId, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);
                    var toUsers = await GetDestinationUsersAsync(item.Id, _mapper, _mediator, _signInManager, _userManager, _logger, _sessionId, _sessionName);

                    allViewNotificationsList.Add(new ViewNotificationViewModel()
                    {
                        Id = item.Id,
                        Date = item.Date,
                        Subject = item.Subject,
                        Text = item.Text,
                        FromUser = fromUser,
                        ToUserIds = string.Join(";", toUsers.Select(x => x.Id)),
                        ToUsers = toUsers
                    });
                }
                return allViewNotificationsList.AsQueryable();
            }
            else
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Notification Contoller - GetInBoxListFromUserAsync - Erro ao obter notificações recebidas: " + response.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma estrutura com os detalhes do user que
        /// envia a notificação, a partir do seu Id.
        /// </summary>
        /// <returns>FromUserDetailViewModel</returns>

        internal static async Task<List<ToUserDetailViewModel>> GetDestinationUsersAsync(
                        int notificationId, IMapper mapper, IMediator mediator,
                        SignInManager<ApplicationUser> signInManager,
                        UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {
            try
            {
                if (notificationId < 1)
                {
                    logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetDestinationUsersAsync - Notification Id: " + notificationId);
                    return null;
                }
                else
                {
                    // obter os users que receberam esta notificação: notificationId
                    var responseNotificationSendedList = await mediator.Send(new GetNotificationsSendedByNotificationIdCachedQuery() { NotificationId = notificationId });
                    if (!responseNotificationSendedList.Succeeded || responseNotificationSendedList.Data == null)
                    {
                        logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetDestinationUsersAsync - Erro ao obter notificaçõesSended: " + responseNotificationSendedList.Message);
                        return null;
                    }
                    else
                    {
                        var notificationSendedList = mapper.Map<List<NotificationSendedViewModel>>(responseNotificationSendedList.Data).AsQueryable();
                        var toUsers = new List<ToUserDetailViewModel>();
                        // para cada notificaçãoSended, obter o user de destino associado
                        foreach (var itemSended in notificationSendedList)
                        {
                            var destinationUser = await signInManager.UserManager.FindByIdAsync(itemSended.ToUserId);
                            if (destinationUser == null)
                            {
                                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetDestinationUsersAsync - Erro ao obter user destino de notificaçãoSended.");
                                //return null;
                            }
                            else
                            {
                                var toUserDetails = await GetToUserDetailsAsync(destinationUser.Id, mapper, mediator, signInManager, userManager, logger, sessionId, sessionName);
                                toUserDetails.NotificationId = notificationId;
                                toUserDetails.IsRead = itemSended.IsRead;

                                // adicionar user de destino à lista de users que receberam esta notificação
                                toUsers.Add(toUserDetails);
                            }
                        }
                        return toUsers;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetDestinationUsersAsync - Exception: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma estrutura com os detalhes do user que
        /// envia a notificação, a partir do seu Id.
        /// </summary>
        /// <returns>FromUserDetailViewModel</returns>

        internal static async Task<FromUserDetailViewModel> GetFromUserDetailsAsync(
                        string userId, IMapper mapper, IMediator mediator,
                        SignInManager<ApplicationUser> signInManager,
                        UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            try
            {
                // user que enviou
                var user = await signInManager.UserManager.FindByIdAsync(userId);

                var userRoles = await userManager.GetRolesAsync(user);
                var userRolesList = userRoles.ToList();
                var roleName = userRolesList.Count > 0 ? userRolesList[0] : string.Empty;

                var userDetails = new FromUserDetailViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.FirstName + " " + user.LastName,
                    ProfilePicture = user.ProfilePicture,
                    RoleName = roleName,
                    Local = await GetUserLocalAsync(user, mapper, mediator, signInManager, logger, sessionId, sessionName)
                };
                return userDetails;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetFromUserDetailsAsync - Exception: " + ex.Message);
                return null;
            }  
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma estrutura com os detalhes de um user que
        /// recebe a notificação, a partir do seu Id.
        /// </summary>
        /// <returns>ToUserDetailViewModel</returns>

        internal static async Task<ToUserDetailViewModel> GetToUserDetailsAsync(
                        string userId, IMapper mapper, IMediator mediator,
                        SignInManager<ApplicationUser> signInManager,
                        UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            try
            {
                // user que enviou
                var user = await signInManager.UserManager.FindByIdAsync(userId);

                var userRoles = await userManager.GetRolesAsync(user);
                var userRolesList = userRoles.ToList();
                var roleName = userRolesList.Count > 0 ? userRolesList[0] : string.Empty;

                var userDetails = new ToUserDetailViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.FirstName + " " + user.LastName,
                    ProfilePicture = user.ProfilePicture,
                    RoleName = roleName,
                    Local = await GetUserLocalAsync(user, mapper, mediator, signInManager, logger, sessionId, sessionName),
                    NotificationId = 0,
                    IsRead = false
                };
                return userDetails;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToUserDetailsAsync - Exception: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma string representante do Local onde o user se encontra.
        /// </summary>
        /// <returns>string</returns>

        internal static async Task<string> GetUserLocalAsync(
                        ApplicationUser user, IMapper mapper, IMediator mediator,
                        SignInManager<ApplicationUser> signInManager,
                        ILogger logger, string sessionId, string sessionName)
        {
            if (user == null) return string.Empty;

            try
            {
                var isSupervisor = await signInManager.UserManager.IsInRoleAsync(user, Roles.Supervisor.ToString());
                var isGerenteLoja = await signInManager.UserManager.IsInRoleAsync(user, Roles.GerenteLoja.ToString());
                var isColaborador = await signInManager.UserManager.IsInRoleAsync(user, Roles.Colaborador.ToString());

                if (isSupervisor) return await GrupolojaController.GetGrupolojaNomeAsync((int)user.GrupolojaId, mapper, mediator);
                if (isGerenteLoja || isColaborador) return await LojaController.GetLojaNomeAsync((int)user.LojaId, mapper, mediator);
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetUserLocalAsync - Exception: " + ex.Message);
                return string.Empty;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings dos users pertencentes a uma loja.
        /// </summary>
        /// <returns>string</returns>

        internal static List<string> GetToMyLojaUsers(
                        int lojaId, LVLgroupDbContext context,
                        ILogger logger, string sessionId, string sessionName)
        {
            if (lojaId == 0) return new List<string>();
            var toMyLojaUsers = new List<string>();

            try
            {
                var users = context.Users.ToList();
                toMyLojaUsers = users.Where(u => (u.LojaId == lojaId) && u.IsActive).Select(u => u.Id).ToList();
                return toMyLojaUsers;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToMyLojaUsersAsync - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings dos users pertencentes a uma grupoloja.
        /// </summary>
        /// <returns>string</returns>

        internal static List<string> GetToMyGrupoLojaUsers(
                        int grupoLojaId, LVLgroupDbContext context,
                        ILogger logger, string sessionId, string sessionName)
        {
            if (grupoLojaId == 0) return new List<string>();
            var toMyGrupoLojaUsers = new List<string>();

            try
            {
                var users = context.Users.ToList();
                toMyGrupoLojaUsers = users.Where(u => (u.GrupolojaId == grupoLojaId) && u.IsActive).Select(u => u.Id).ToList();
                return toMyGrupoLojaUsers;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToMyGrupoLojaUsers - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings dos users pertencentes a uma empresa.
        /// </summary>
        /// <returns>string</returns>

        internal static List<string> GetToMyEmpresaUsers(
                        int empresaId, LVLgroupDbContext context,
                        ILogger logger, string sessionId, string sessionName)
        {
            if (empresaId == 0) return new List<string>();
            var toMyEmpresaUsers = new List<string>();

            try
            {
                var users = context.Users.ToList();
                toMyEmpresaUsers = users.Where(u => (u.EmpresaId == empresaId) && u.IsActive).Select(u => u.Id).ToList();
                return toMyEmpresaUsers;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToMyEmpresaUsers - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings de todos os gerentes de loja de uma loja.
        /// </summary>
        /// <returns>string</returns>

        internal static async Task<List<string>> GetToMyGerentelojaUsersAsync(
                        int lojaId, UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {

            try
            {
                var roleName = Roles.GerenteLoja.ToString();
                var gerentesLojas = await userManager.GetUsersInRoleAsync(roleName);
                var gerenteLojaUserIds = gerentesLojas.Where(g => (g.LojaId == lojaId) && g.IsActive).Select(g => g.Id).ToList();

                return gerenteLojaUserIds;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToMyGerentelojaUsersAsync - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings de todos os supervisores de uma loja.
        /// </summary>
        /// <returns>string</returns>

        internal static async Task<List<string>> GetToMySupervisorUsersAsync(
                        int grupoLojaId, UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {

            try
            {
                var roleName = Roles.Supervisor.ToString();
                var supervisores = await userManager.GetUsersInRoleAsync(roleName);
                var supervisorUserIds = supervisores.Where(g => (g.GrupolojaId == grupoLojaId) && g.IsActive).Select(g => g.Id).ToList();

                return supervisorUserIds;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToMySupervisorUsersAsync - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings de todos os revisores.
        /// </summary>
        /// <returns>string</returns>

        internal static async Task<List<string>> GetToRevisoresUsersAsync(
                        UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {

            try
            {
                var roleName = Roles.Revisor.ToString();
                var revisores = await userManager.GetUsersInRoleAsync(roleName);
                var revisorUserIds = revisores.Where(g => g.IsActive == true).Select(g => g.Id).ToList();

                return revisorUserIds;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToRevisoresUsersAsync - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings de todos os admins.
        /// </summary>
        /// <returns>string</returns>

        internal static async Task<List<string>> GetToAdminsUsersAsync(
                        UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {

            try
            {
                var roleName = Roles.Admin.ToString();
                var admins = await userManager.GetUsersInRoleAsync(roleName);
                var adminsUserIds = admins.Where(g => g.IsActive == true).Select(g => g.Id).ToList();

                return adminsUserIds;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToAdminsUsersAsync - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings de todos os superadmins.
        /// </summary>
        /// <returns>string</returns>

        internal static async Task<List<string>> GetToSuperAdminsUsersAsync(
                        UserManager<ApplicationUser> userManager,
                        ILogger logger, string sessionId, string sessionName)
        {

            try
            {
                var roleName = Roles.SuperAdmin.ToString();
                var superadmins = await userManager.GetUsersInRoleAsync(roleName);
                var superadminsUserIds = superadmins.Where(g => g.IsActive == true).Select(g => g.Id).ToList();

                return superadminsUserIds;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToAdminsUsersAsync - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma lista de strings de todos users.
        /// </summary>
        /// <returns>string</returns>

        internal static List<string> GetToAllUsers(
                        LVLgroupDbContext context,
                        ILogger logger, string sessionId, string sessionName)
        {
            var toAllUsers = new List<string>();

            try
            {
                var users = context.Users.ToList();
                toAllUsers = users.Where(u => u.IsActive == true).Select(u => u.Id).ToList();
                return toAllUsers;
            }
            catch (Exception ex)
            {
                logger.LogError(sessionId + " | " + sessionName + " | Notification Contoller - GetToAllUsers - Exception: " + ex.Message);
                return new List<string>();
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}
