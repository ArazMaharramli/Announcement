using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Roles.Commands.Create;
using Application.CQRS.Roles.Commands.Update;
using Application.CQRS.Roles.Queries.GetAll;
using Application.CQRS.Roles.Queries.GetRoleById;
using Application.CQRS.Roles.Queries.GetRoleByName;
using Application.CQRS.Roles.Queries.GetRoleClaims;
using Application.CQRS.Roles.Queries.SearchUsersByRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Common.Models;
using WebUI.Areas.Admin.ViewModels.Roles;
using WebUI.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Policy = SystemClaims.Roles.Show)]
        public async Task<IActionResult> Index()
        {
            var model = new IndexRoleViewModel();
            model.Roles = await _mediator.Send(new GetAllRolesQuery());

            return View(model);
        }

        public async Task<IActionResult> Get()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            var model = roles.Select(x => new SelectListModel
            {
                Id = x.Id,
                Text = x.Name
            }).ToList();

            return Ok(model);
        }

        public async Task<IActionResult> GetRole([FromRoute] string id)
        {
            var roles = await _mediator.Send(new FindByRoleNameOrIdQuery { Id = id });
            var model = roles.Claims.Select(x => new SelectListModel
            {
                Id = x,
                Text = x
            }).ToList();

            return Ok(model);
        }

        [HttpGet]
        //[Authorize(Policy = SystemClaims.Roles.Create)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Policy = SystemClaims.Roles.Create)]
        public async Task<IActionResult> Create(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var resp = await _mediator.Send(command, cancellationToken);
            return RedirectToAction(nameof(Index));
        }


        //[Authorize(Policy = SystemClaims.Roles.Edit)]
        public async Task<IActionResult> EditAsync(string id)
        {
            var role = await _mediator.Send(new FindByRoleNameOrIdQuery { Id = id });
            var model = new UpdateRoleCommand
            {
                Id = role.Id,
                Name = role.Name,
                Claims = role.Claims
            };

            return View(model);
        }

        [HttpPost]
        //[Authorize(Policy = SystemClaims.Roles.Edit)]
        public async Task<IActionResult> Edit(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return RedirectToAction("Index", "Roles", new { Area = "Admin" });
        }

        //[Authorize(Policy = SystemClaims.Roles.Show)]
        public async Task<IActionResult> Detail(GetRoleByNameQuery request)
        {
            var roleDto = await _mediator.Send(request);

            var model = new DetailRoleViewModel
            {
                RoleId = roleDto.Id,
                RoleName = roleDto.Name,
                UserCount = roleDto.UsersCount,
                Claims = roleDto.Claims
            };


            return View(model);
        }

        //[Authorize(Policy = SystemClaims.Roles.Show)]
        public async Task<IActionResult> DatatableAsync(string roleName, CancellationToken cancellationToken)
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

            var query = new SearchUsersByroleQuery
            {
                Deleted = false,
                PageSize = pageSize,
                SearchValue = searchValue,
                Skip = skip,
                SortColumnDirection = sortColumnDirection,
                SortColumn = sortColumn,
                RoleName = roleName
            };
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        //[HttpPost]
        //public async Task<IActionResult> RemoveUserFromRole(string roleName, string[] ids, CancellationToken cancellationToken)
        //{
        //    var res = await _mediator.Send(new RemoveUserFromRoleCommand { UserId = ids, RoleName = roleName }, cancellationToken);

        //    if (res)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest();

        //}

    }
}

