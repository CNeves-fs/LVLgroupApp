using Core.Features.Logs.Response;
using Core.Interfaces.Shared;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account
{
    public class AuditLogModel : BasePageModel<AuditLogModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IAuthenticatedUserService _userService;

        public List<AuditLogResponse> AuditLogResponses;

        public List<string> Roles { get; set; }

        public string FirstName;

        public string LastName;

        public string Email;

        public string UserId;

        public string Loja;

        public string GrupoLoja;

        public string Empresa;

        private IViewRenderService _viewRenderer;


        //---------------------------------------------------------------------------------------------------


        public AuditLogModel(IAuthenticatedUserService userService, IViewRenderService viewRenderer, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _viewRenderer = viewRenderer;
            _userManager = userManager;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task OnGetAsync(string userId)
        {
            try
            {
                var loggedIn = _userService.UserId;
                var auditUser = await _userManager.FindByIdAsync(userId);
                if (auditUser == null)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | AuditLOg Page - OnGetAsync - Error: Utilizador não existe e vai sair | User: " + userId);
                    return;
                };

                //var response = await _mediator.Send(new GetAuditLogsByUserIdQuery() { userId = userId });
                //AuditLogResponses = response.Data;
                var roles = await _userManager.GetRolesAsync(auditUser);
                Roles = roles.ToList();
                FirstName = auditUser.FirstName;
                LastName = auditUser.LastName;
                Email = auditUser.Email;
                UserId = auditUser.Id;
                if (auditUser.LojaId != null)
                {
                    Loja = await LojaController.GetLojaNomeAsync((int)auditUser.LojaId, _mapper, _mediator);
                    GrupoLoja = await GrupolojaController.GetGrupolojaNomeFromLojaIdAsync((int)auditUser.LojaId, _mapper, _mediator);
                    Empresa = await EmpresaController.GetEmpresaNomeAsync((int)auditUser.LojaId, _mapper, _mediator);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | AuditLOg Page - OnGetAsync - Exception vai sair e retornar Error: " + ex.Message);
                return;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}