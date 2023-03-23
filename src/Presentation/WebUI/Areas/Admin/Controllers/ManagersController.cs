using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.CQRS.Managers.Commands.Create;
using Application.CQRS.Managers.Commands.Delete;
using Application.CQRS.Managers.Commands.Recover;
using Application.CQRS.Managers.Commands.Update;
using Application.CQRS.Managers.Commands.UpdateManagerRoles;
using Application.CQRS.Managers.Queries.FindById;
using Application.CQRS.Managers.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class ManagersController : Controller
    {
        private readonly IMediator _mediator;

        public ManagersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = SystemClaims.Managers.Show)]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Deleted()
        {
            return View();
        }

        [Authorize(Policy = SystemClaims.Managers.Show)]
        public async Task<IActionResult> DatatableAsync(CancellationToken cancellationToken, bool deleted = false)
        {
            //var searchLang = Request.Form["query[language]"].FirstOrDefault();
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            var sortColumnIndex = Request.Query["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Query[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();
            int pageSize = string.IsNullOrEmpty(length) ? 10 : Convert.ToInt32(length);
            int skip = string.IsNullOrEmpty(start) ? 0 : Convert.ToInt32(start);

            var query = new SearchManagersQuery
            {
                Deleted = deleted,
                PageSize = pageSize,
                SearchValue = searchValue,
                Skip = skip,
                SortColumnDirection = sortColumnDirection,
                SortColumn = sortColumn
            };
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [Authorize(Policy = SystemClaims.Managers.Create)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = SystemClaims.Managers.Create)]
        public async Task<IActionResult> Create(CreateManagerCommand command)
        {
            var res = await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = SystemClaims.Managers.Edit)]
        public async Task<IActionResult> EditAsync(string id, CancellationToken cancellationToken)
        {
            var query = new FindByManagerIdQuery
            {
                Id = id
            };
            var manager = await _mediator.Send(query, cancellationToken);

            var model = new UpdateManagerCommand
            {
                Id = manager.Id,
                Name = manager.Name,
                Phone = manager.Phone,
                RoleIds = manager.Roles.Select(x => x.Id).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = SystemClaims.Managers.Edit)]
        public async Task<IActionResult> EditAsync(UpdateManagerCommand command, CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = SystemClaims.Managers.EditPermissions)]
        public async Task<IActionResult> EditPermissionsAsync(string id, CancellationToken cancellationToken)
        {
            var query = new FindByManagerIdQuery
            {
                Id = id
            };
            var manager = await _mediator.Send(query, cancellationToken);

            var model = new UpdateManagerRolesAndClaimsCommand
            {
                Id = manager.Id,
                RoleIds = manager.Roles.Select(x => x.Id).ToList(),
                Claims = manager.Claims
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = SystemClaims.Managers.EditPermissions)]
        public async Task<IActionResult> EditPermissionsAsync(UpdateManagerRolesAndClaimsCommand command, CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Authorize(Policy = SystemClaims.Managers.Delete)]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteManagersCommand command, CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(command, cancellationToken);

            return Ok();
        }
        [HttpPost]
        [Authorize(Policy = SystemClaims.Managers.Recover)]
        public async Task<IActionResult> Recover([FromBody] RecoverManagersCommand command, CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(command, cancellationToken);

            return Ok();
        }
        //[Authorize(Policy = SystemClaims.Managers.Show)]
        //public IActionResult Detail(string id, string roleName)
        //{
        //    var model = new DetailManagerViewModel();

        //    model.Id = id;
        //    model.RoleName = roleName;

        //    return View(model);
        //}
    }
}

