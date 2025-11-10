using Core.Constants;
using Core.Entities.Charts;
using Core.Features.Artigos.Queries.GetAllCached;
using Core.Features.Charts.CountQueries.CountAllArtigosCached;
using Core.Features.Charts.CountQueries.CountAllClientesCached;
using Core.Features.Charts.CountQueries.CountAllLojasCached;
using Core.Features.Claims.Queries.GetAllCached;
using Core.Features.Clientes.Queries.GetAllCached;
using Core.Features.Clientes.Queries.GetByTelefone;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Gruposlojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Models;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Dashboard.Models.DashboardTops;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Dashboard.Controllers.DashboardTops
{

    [Area("Dashboard")]
    [Authorize]
    public class DashboardTopsController : BaseController<DashboardTopsController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private IWebHostEnvironment _environment;

        private readonly IStringLocalizer<DashboardTopsController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DashboardTopsController(UserManager<ApplicationUser> userManager, 
                                       IWebHostEnvironment Environment, 
                                       IStringLocalizer<DashboardTopsController> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
            _environment = Environment;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult Index()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - Index - return view");
            return View("Index");
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadAll()
        {
            // Iniciar viewModel
            var viewModel = new DashboardTopsViewModel();
            viewModel.TopArtigosList = new List<TopArtigoViewModel>();
            viewModel.TopClientesList = new List<TopClienteViewModel>();
            viewModel.TopUserResponsávelList = new List<TopUserViewModel>();
            viewModel.TopUserFecharEmLojaList = new List<TopUserViewModel>();
            viewModel.TopLojasList = new List<TopLojaViewModel>();
            viewModel.TopTrocaDiretaList = new List<TopLojaViewModel>();

            // Counter: total de artigos
            var response_artigos = await _mediator.Send( new CountAllArtigosCachedQuery() );
            viewModel.TotalArtigos = response_artigos.Data.EntityCount;

            // Counter: total de clientes
            var response_clientes = await _mediator.Send(new CountAllClientesCachedQuery());
            viewModel.TotalClientes = response_clientes.Data.EntityCount;

            // Counter: total de users
            var point_users = CountAllUsersQuery();
            viewModel.TotalUsers = point_users.EntityCount;

            // Counter: total de lojas
            var response_lojas = await _mediator.Send(new CountAllLojasCachedQuery());
            viewModel.TotalLojas = response_lojas.Data.EntityCount;

            return PartialView("_dashboardTops", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de top 10 de artigos para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetTop10Artigos()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();
                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();

                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;


                // lista de claims
                var responseAllClaims = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (!responseAllClaims.Succeeded) return new ObjectResult(new { status = "error" });
                var allClaims = _mapper.Map<List<Core.Entities.Claims.Claim>>(responseAllClaims.Data).AsQueryable();

                // filtros de claims
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);
                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allClaims = allClaims.Where(c => c.EmpresaId == intEmpresaFilter);
                };

                // lista de artigos
                var responseAllArtigos = await _mediator.Send(new GetAllArtigosCachedQuery());
                if (!responseAllArtigos.Succeeded) return new ObjectResult(new { status = "error" });
                var allArtigos = _mapper.Map<List<Core.Entities.Artigos.Artigo>>(responseAllArtigos.Data).AsQueryable();

                // lista de empresas
                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                var topArtigos = (from claim in allClaims
                                  join e in allEmpresas on claim.EmpresaId equals e.Id into elist
                                  from emp in elist.DefaultIfEmpty()
                                  join a in allArtigos on claim.ArtigoId equals a.Id into alist
                                  from art in alist.DefaultIfEmpty()
                                  select new 
                                  {
                                      Id = art.Id,
                                      Referencia = art.Referencia,
                                      EmpresaLogo = emp.LogoPicture,
                                      GenderId = art.GenderId,
                                      TotalClaims = 0
                                  }).ToList().GroupBy(x => x.Id);

                var data = topArtigos.Select(a => new TopArtigoViewModel()
                {
                    Id = a.FirstOrDefault().Id,
                    Referencia = a.FirstOrDefault().Referencia,
                    EmpresaLogo = Convert.ToBase64String(a.FirstOrDefault().EmpresaLogo),
                    GenderId = a.FirstOrDefault().GenderId,
                    TotalClaims = a.Count()
                }).OrderByDescending(x => x.TotalClaims).Take(10).ToList();

                var jsonData = new { draw = draw, recordsFiltered = 10, recordsTotal = 10, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - GetTop10Artigos - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de top 10 de clientes para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetTop10Clientes()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();
                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();

                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;

                // lista de claims
                var responseAllClaims = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (!responseAllClaims.Succeeded) return new ObjectResult(new { status = "error" });
                var allClaims = _mapper.Map<List<Core.Entities.Claims.Claim>>(responseAllClaims.Data).AsQueryable();

                // filtros de claims
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);
                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allClaims = allClaims.Where(c => c.EmpresaId == intEmpresaFilter);
                };

                // lista de clientes
                var responseAllClientes = await _mediator.Send(new GetAllClientesCachedQuery());
                if (!responseAllClientes.Succeeded) return new ObjectResult(new { status = "error" });
                var allClientes = _mapper.Map<List<Core.Entities.Clientes.Cliente>>(responseAllClientes.Data).AsQueryable();

                var topClientes = (from claim in allClaims
                                  join c in allClientes on claim.ClienteId equals c.Id into clist
                                  from cli in clist.DefaultIfEmpty()

                                  select new
                                  {
                                      Id = cli.Id,
                                      Nome = cli.Nome,
                                      Telefone = cli.Telefone,
                                      EmailAutorDecisao = claim.EmailAutorDecisão,
                                      Rejeitada = claim.Rejeitada,
                                      TotalClaims = 0,
                                      TotalClaimsAceites = 0,
                                      TotalClaimsNãoAceites = 0,
                                  }).ToList().GroupBy(x => x.Id);


                var data = topClientes.Select(a => new TopClienteViewModel()
                {
                    Id = a.FirstOrDefault().Id,
                    Nome = a.FirstOrDefault().Nome,
                    Telefone = a.FirstOrDefault().Telefone,
                    TotalClaims = a.Count(),
                    TotalClaimsAceites = a.Where(x => (!string.IsNullOrEmpty(x.EmailAutorDecisao) && !x.Rejeitada)).Count(), 
                    TotalClaimsNaoAceites = a.Where(x => (!string.IsNullOrEmpty(x.EmailAutorDecisao) && x.Rejeitada)).Count()
                }).OrderByDescending(x => x.TotalClaims).Take(10).ToList();

                var jsonData = new { draw = draw, recordsFiltered = 10, recordsTotal = 10, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - GetTop10Clientes - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de top 10 de funcionários
        /// com mais aberturas de reclamações.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetTop10Aberturas()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();
                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();

                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;

                // lista de claims
                var responseAllClaims = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (!responseAllClaims.Succeeded) return new ObjectResult(new { status = "error" });
                var allClaims = _mapper.Map<List<Core.Entities.Claims.Claim>>(responseAllClaims.Data).AsQueryable();

                // lista de empresas
                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                // lista de lojas
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allLojas = _mapper.Map<List<Core.Entities.Business.Loja>>(responseAllLojas.Data).AsQueryable();

                // filtros de claims
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);
                
                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allClaims = allClaims.Where(c => c.EmpresaId == intEmpresaFilter);
                };

                // lista de utilizadores
                var userList = new List<UserViewModel>();
                var users = _context.Users.ToList();
                foreach (ApplicationUser user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRoles = roles.ToList();
                    var uvm = new UserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RoleName = roles.Count > 0 ? roles[0] : string.Empty,
                        Email = user.Email,
                        ProfilePicture = user.ProfilePicture,
                    };
                    userList.Add(uvm);
                }

                var allUsers = userList.AsQueryable();

                var topUsers = (from claim in allClaims
                                join e in allEmpresas on claim.EmpresaId equals e.Id into elist
                                from emp in elist.DefaultIfEmpty()
                                join u in allUsers on claim.EmailAutor equals u.Email into ulist
                                from usr in ulist.DefaultIfEmpty()
                                join l in allLojas on claim.LojaId equals l.Id into llist
                                from loj in llist.DefaultIfEmpty()

                                select new
                                {
                                    CodeId = claim.CodeId == null ? string.Empty : claim.CodeId,
                                    Nome = usr == null ? string.Empty : usr.FirstName + " " + usr.LastName,
                                    EmailAutor = claim.EmailAutor == null ? string.Empty : claim.EmailAutor,
                                    Role = usr == null ? string.Empty : usr.RoleName,
                                    ProfilePicture = usr == null ? "" : ( usr.ProfilePicture == null ? "" : Convert.ToBase64String(usr.ProfilePicture) ),
                                    EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                    Loja = loj.Nome == null ? string.Empty : loj.Nome,
                                    EmailAutorDecisão = claim.EmailAutorDecisão == null ? string.Empty : claim.EmailAutorDecisão,
                                    TotalClaims = 0,
                                    TrocasDiretas = 0
                                }).ToList().GroupBy(x => x.EmailAutor);


                var data = topUsers.Select(u => new TopUserViewModel()
                {
                    Email = u.FirstOrDefault().EmailAutor,
                    Role = u.FirstOrDefault().Role,
                    Nome = u.FirstOrDefault().Nome,
                    ProfilePicture = u.FirstOrDefault().ProfilePicture,
                    EmpresaLogo = u.FirstOrDefault().EmpresaLogo,
                    Loja = u.FirstOrDefault().Loja,
                    TotalClaims = u.Count(),
                    TrocasDiretas = u.Where(x => x.EmailAutorDecisão == x.EmailAutor).Count()
                }).OrderByDescending(x => x.TotalClaims).Take(10).ToList();

                var jsonData = new { draw = draw, recordsFiltered = 10, recordsTotal = 10, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - GetTop10Aberturas - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de top 10 de funcionários
        /// com mais fechos de reclamações.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetTop10Fechos()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();
                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();

                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;

                // lista de claims
                var responseAllClaims = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (!responseAllClaims.Succeeded) return new ObjectResult(new { status = "error" });
                var allClaims = _mapper.Map<List<Core.Entities.Claims.Claim>>(responseAllClaims.Data).AsQueryable();
                allClaims = allClaims.Where(x => !string.IsNullOrEmpty(x.EmailAutorFechoEmLoja)).AsQueryable();

                // lista de empresas
                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                // lista de lojas
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allLojas = _mapper.Map<List<Core.Entities.Business.Loja>>(responseAllLojas.Data).AsQueryable();

                // filtros de claims
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);
                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allClaims = allClaims.Where(c => c.EmpresaId == intEmpresaFilter);
                };

                // lista de utilizadores
                var userList = new List<UserViewModel>();
                var users = _context.Users.ToList();
                foreach (ApplicationUser user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRoles = roles.ToList();
                    var uvm = new UserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RoleName = roles.Count > 0 ? roles[0] : string.Empty,
                        Email = user.Email,
                        ProfilePicture = user.ProfilePicture
                    };
                    userList.Add(uvm);
                }

                //var count = 0;
                //foreach (Core.Entities.Claims.Claim cl in allClaims)
                //{
                //    var u = userList.Where(x => x.Email == cl.EmailAutorFechoEmLoja).FirstOrDefault();
                //    if (u == null)
                //    {
                //        ++count;
                //    }
                //}


                var allUsers = userList.AsQueryable();

                var topUsers = (from claim in allClaims
                                join e in allEmpresas on claim.EmpresaId equals e.Id into elist
                                from emp in elist.DefaultIfEmpty()
                                join u in allUsers on claim.EmailAutorFechoEmLoja equals u.Email into ulist
                                from usr in ulist.DefaultIfEmpty()
                                join l in allLojas on claim.LojaId equals l.Id into llist
                                from loj in llist.DefaultIfEmpty()

                                select new
                                {
                                    CodeId = claim.CodeId,
                                    Nome = usr == null ? string.Empty : usr.FirstName + " " + usr.LastName,
                                    EmailAutor = claim.EmailAutor,
                                    EmailAutorDecisão = claim.EmailAutorDecisão,
                                    EmailAutorFechoEmLoja = claim.EmailAutorFechoEmLoja,
                                    Role = usr == null ? string.Empty : usr.RoleName,
                                    ProfilePicture = usr == null ? "" : (usr.ProfilePicture == null ? "" : Convert.ToBase64String(usr.ProfilePicture)),
                                    EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                    Loja = loj.Nome,
                                    TotalClaims = 0,
                                    TrocasDiretas = 0
                                }).Where(x => !string.IsNullOrEmpty(x.EmailAutorFechoEmLoja)).ToList().GroupBy(x => x.EmailAutorFechoEmLoja);


                var data = topUsers.Select(u => new TopUserViewModel()
                {
                    Email = u.FirstOrDefault().EmailAutorFechoEmLoja,
                    Role = u.FirstOrDefault().Role,
                    Nome = u.FirstOrDefault().Nome,
                    ProfilePicture = u.FirstOrDefault().ProfilePicture,
                    EmpresaLogo = u.FirstOrDefault().EmpresaLogo,
                    Loja = u.FirstOrDefault().Loja,
                    TotalClaims = u.Count(),
                    TrocasDiretas = u.Where(x => x.EmailAutorFechoEmLoja == x.EmailAutorDecisão).Count()
                }).OrderByDescending(x => x.TotalClaims).Take(10).ToList();

                var jsonData = new { draw = draw, recordsFiltered = 10, recordsTotal = 10, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - GetTop10Fechos - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de top 10 de lojas
        /// com mais reclamações.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetTop10Lojas()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();
                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();

                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;

                // lista de claims
                var responseAllClaims = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (!responseAllClaims.Succeeded) return new ObjectResult(new { status = "error" });
                var allClaims = _mapper.Map<List<Core.Entities.Claims.Claim>>(responseAllClaims.Data).AsQueryable();
                allClaims = allClaims.Where(x => !string.IsNullOrEmpty(x.EmailAutorFechoEmLoja)).AsQueryable();

                // lista de empresas
                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                // lista de lojas
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allLojas = _mapper.Map<List<Core.Entities.Business.Loja>>(responseAllLojas.Data).AsQueryable();

                // lista de agrupamentos
                var responseAllAgrupamentos = await _mediator.Send(new GetAllGruposlojasCachedQuery());
                if (!responseAllAgrupamentos.Succeeded) return new ObjectResult(new { status = "error" });
                var AllAgrupamentos = _mapper.Map<List<Core.Entities.Business.Grupoloja>>(responseAllAgrupamentos.Data).AsQueryable();

                // filtros de claims
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);
                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allClaims = allClaims.Where(c => c.EmpresaId == intEmpresaFilter);
                };

                var topClaims = (from claim in allClaims
                                join e in allEmpresas on claim.EmpresaId equals e.Id into elist
                                from emp in elist.DefaultIfEmpty()
                                join a in AllAgrupamentos on claim.GrupolojaId equals a.Id into alist
                                from agrp in alist.DefaultIfEmpty()
                                join l in allLojas on claim.LojaId equals l.Id into llist
                                from loj in llist.DefaultIfEmpty()

                                select new
                                {
                                    EmailAutorDecisão = claim.EmailAutorDecisão,
                                    EmailAutorFechoEmLoja = claim.EmailAutorFechoEmLoja,
                                    EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                    LojaId = claim.LojaId,
                                    Loja = loj.Nome,
                                    Agrupamento = agrp.Nome,
                                    TotalClaims = 0,
                                    TrocasDiretas = 0
                                }).GroupBy(x => x.LojaId);


                var data = topClaims.Select(u => new TopLojaViewModel()
                {
                    Loja = u.FirstOrDefault().Loja,
                    EmpresaLogo = u.FirstOrDefault().EmpresaLogo,
                    Agrupamento = u.FirstOrDefault().Agrupamento,
                    TotalClaims = u.Count(),
                    TrocasDiretas = u.Where(x => x.EmailAutorFechoEmLoja == x.EmailAutorDecisão).Count()
                }).OrderByDescending(x => x.TotalClaims).Take(10).ToList();

                var jsonData = new { draw = draw, recordsFiltered = 10, recordsTotal = 10, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - GetTop10Lojas - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de top 10 de lojas
        /// com mais reclamações resolvidas com trocas diretas.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetTop10TrocasDiretas()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();
                var empresaFilter = Request.Form["empresaFilter"].FirstOrDefault();

                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);
                int intEmpresaFilter = empresaFilter != null ? Convert.ToInt32(empresaFilter) : 0;

                // lista de claims
                var responseAllClaims = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (!responseAllClaims.Succeeded) return new ObjectResult(new { status = "error" });
                var allClaims = _mapper.Map<List<Core.Entities.Claims.Claim>>(responseAllClaims.Data).AsQueryable();
                allClaims = allClaims.Where(x => !string.IsNullOrEmpty(x.EmailAutorFechoEmLoja)).AsQueryable();

                // lista de empresas
                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                // lista de lojas
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allLojas = _mapper.Map<List<Core.Entities.Business.Loja>>(responseAllLojas.Data).AsQueryable();

                // lista de agrupamentos
                var responseAllAgrupamentos = await _mediator.Send(new GetAllGruposlojasCachedQuery());
                if (!responseAllAgrupamentos.Succeeded) return new ObjectResult(new { status = "error" });
                var AllAgrupamentos = _mapper.Map<List<Core.Entities.Business.Grupoloja>>(responseAllAgrupamentos.Data).AsQueryable();

                // filtros de claims
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);
                // filtrar por empresa se necessário
                if (intEmpresaFilter > 0)
                {
                    allClaims = allClaims.Where(c => c.EmpresaId == intEmpresaFilter);
                };

                var topClaims = (from claim in allClaims
                                 join e in allEmpresas on claim.EmpresaId equals e.Id into elist
                                 from emp in elist.DefaultIfEmpty()
                                 join a in AllAgrupamentos on claim.GrupolojaId equals a.Id into alist
                                 from agrp in alist.DefaultIfEmpty()
                                 join l in allLojas on claim.LojaId equals l.Id into llist
                                 from loj in llist.DefaultIfEmpty()

                                 select new
                                 {
                                     EmailAutorDecisão = claim.EmailAutorDecisão,
                                     EmailAutorFechoEmLoja = claim.EmailAutorFechoEmLoja,
                                     EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                     LojaId = claim.LojaId,
                                     Loja = loj.Nome,
                                     Agrupamento = agrp.Nome,
                                     TotalClaims = 0,
                                     TrocasDiretas = 0
                                 }).GroupBy(x => x.LojaId);


                var data = topClaims.Select(u => new TopLojaViewModel()
                {
                    Loja = u.FirstOrDefault().Loja,
                    EmpresaLogo = u.FirstOrDefault().EmpresaLogo,
                    Agrupamento = u.FirstOrDefault().Agrupamento,
                    TotalClaims = u.Count(),
                    TrocasDiretas = u.Where(x => x.EmailAutorFechoEmLoja == x.EmailAutorDecisão).Count()
                }).OrderByDescending(x => x.TrocasDiretas).Take(10).ToList();

                var jsonData = new { draw = draw, recordsFiltered = 10, recordsTotal = 10, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Tops Controller - GetTop10TrocasDiretas - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal ChartPoint CountAllUsersQuery()
        {
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = 0
            };

            try
            {
                var users = _context.Users.ToList();
                cp.EntityCount = users.Count;
                return cp;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | DashboardTops Contoller - CountAllUsersQuery: " + ex.Message);
                return cp;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}
