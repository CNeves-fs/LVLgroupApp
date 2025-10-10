using Core.Constants;
using Core.Entities.Identity;
using Core.Features.Logs.Commands.Delete;
using Core.Features.Logs.Queries.GetAllAuditLogs;
using Core.Features.Logs.Queries.GetByUserIdUserLogs;
using Core.Features.Logs.Response;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuditLogController : BaseController<AuditLogController>
    {

        //---------------------------------------------------------------------------------------------------


        private IWebHostEnvironment _environment;

        private readonly IStringLocalizer<AuditLogController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public AuditLogController(IWebHostEnvironment Environment, IStringLocalizer<AuditLogController> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _environment = Environment;
        }

        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.AuditLogs.View)]
        public IActionResult Index()
        {
            var model = new AuditLogViewModel();
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | AuditLog Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.AuditLogs.View)]
        public IActionResult LoadAll(string id)
        {
            var model = new AuditLogViewModel();
            model.UserId = id;
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | AuditLog Contoller - LoadAll - return lista de AuditLogviewModel");
            return PartialView("_ViewAll", model);
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.AuditLogs.View)]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var desdedateLog = Request.Form["desdedateLog"].FirstOrDefault();
                var atedateLog = Request.Form["atedateLog"].FirstOrDefault();
                var userId = Request.Form["userId"].FirstOrDefault();

                DateTime dateDesde = System.String.IsNullOrEmpty(desdedateLog) ? DateTime.MinValue : DateTime.Parse(desdedateLog);
                DateTime dateAte = System.String.IsNullOrEmpty(atedateLog) ? DateTime.MaxValue : DateTime.Parse(atedateLog);

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int recordsFiltered = 0;

                // construir lista para View Model
                var responseAllLogs = System.String.IsNullOrEmpty(userId) ? 
                        await _mediator.Send(new GetAllAuditLogsQuery()) : 
                        await _mediator.Send(new GetAuditLogsByUserIdQuery() { userId = userId } );
                if (!responseAllLogs.Succeeded) return new ObjectResult(new { status = "error" });
                var allLogs = responseAllLogs.Data.AsQueryable();
                recordsTotal = allLogs.Count();

                // filtrar por data
                allLogs = allLogs.Where(c => c.DateTime >= dateDesde && c.DateTime <= dateAte);

                // filtrar por searchValue
                if (!string.IsNullOrEmpty(searchValue))
                {
                    var queryUserId = allLogs.Where(x => x.UserId.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryEmail = allLogs.Where(c => c.Email != null);
                    queryEmail = queryEmail.Where(c => c.Email.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryType = allLogs.Where(c => c.Type != null);
                    queryType = queryType.Where(c => c.Type.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryTableName = allLogs.Where(c => c.TableName != null);
                    queryTableName = queryTableName.Where(c => c.TableName.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryAffectedColumns = allLogs.Where(c => c.AffectedColumns != null);
                    queryAffectedColumns = queryAffectedColumns.Where(c => c.AffectedColumns.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryOldValues = allLogs.Where(c => c.OldValues != null);
                    queryOldValues = queryOldValues.Where(c => c.OldValues.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryNewValues = allLogs.Where(c => c.NewValues != null);
                    queryNewValues = queryNewValues.Where(c => c.NewValues.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    var queryPrimaryKey = allLogs.Where(c => c.PrimaryKey != null);
                    queryPrimaryKey = queryPrimaryKey.Where(c => c.PrimaryKey.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                    allLogs = queryUserId.Union(queryEmail)
                                         .Union(queryType)
                                         .Union(queryTableName)
                                         .Union(queryAffectedColumns)
                                         .Union(queryOldValues)
                                         .Union(queryNewValues)
                                         .Union(queryPrimaryKey).AsQueryable();
                }

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    allLogs = allLogs.OrderBy(sortColumn + " " + sortColumnDirection).AsQueryable();
                }

                // retornar lista para a datatable
                recordsFiltered = allLogs.Count();
                var data = allLogs.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsFiltered, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | AuditLOg Contoller - GetLogs - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.AuditLogs.Delete)]
        public async Task<JsonResult> OnPostCleanAsync()
        {
            try
            {
                var responseAllLogs = await _mediator.Send(new GetAllAuditLogsQuery());
                if (!responseAllLogs.Succeeded) return Json(new { status = "error" });
                var allLogs = responseAllLogs.Data.AsQueryable();
                var oldLogs = allLogs.Where(l => l.DateTime < DateTime.Now.AddMonths(-1));
                
                foreach(AuditLogResponse log in oldLogs)
                {
                    var deleteAuditLogCommand = await _mediator.Send(new DeleteAuditLogCommand() { Id = log.Id });
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | AuditLog Contoller - OnPostCleanAsync - AuditLog deleted=" + log.Id);
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

    }
}