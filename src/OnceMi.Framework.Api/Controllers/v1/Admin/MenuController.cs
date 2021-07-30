﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnceMi.Framework.Entity.Admin;
using OnceMi.Framework.Extension.Authorizations;
using OnceMi.Framework.IService.Admin;
using OnceMi.Framework.Model.Dto;
using OnceMi.Framework.Model.Enums;
using OnceMi.Framework.Model.Exception;
using OnceMi.Framework.Util.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnceMi.Framework.Api.Controllers.v1.Admin
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenusService _service;
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public MenuController(ILogger<MenuController> logger
            , IMenusService service
            , IPermissionService permissionService
            , IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 获取所有Api版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetMenuTypes))]
        public async Task<List<ISelectResponse<int>>> GetMenuTypes()
        {
            return await _service.QueryMenuTypes();
        }

        /// <summary>
        /// 获取下一排序顺序
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(GetNextSortValue))]
        public async Task<int> GetNextSortValue([FromQuery] long? parentId)
        {
            return await _service.QueryNextSortValue(parentId);
        }

        /// <summary>
        /// 根据角色查询菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(QueryViewMenu))]
        [NoPermission]
        public async Task<List<UserMenuResponse>> QueryViewMenu()
        {
            List<long> ids = HttpContext.User.GetRoles()?.Distinct().ToList();
            if (ids == null || ids.Count == 0)
            {
                throw new BusException(-1, "获取用户角色失败！");
            }
            return await _permissionService.QueryUserMenus(ids);
        }

        /// <summary>
        /// 查询级联选择器数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(CascaderList))]
        public async Task<List<ICascaderResponse>> CascaderList()
        {
            var data = await _service.Query(new IPageRequest()
            {
                Page = 1,
                Size = 999999,
                OrderBy = new string[] { "id,asc" },
            }, true);
            if (data != null && data.PageData != null && data.PageData.Any())
            {
                return _mapper.Map<List<ICascaderResponse>>(data.PageData);
            }
            return new List<ICascaderResponse>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryByPage")]
        public async Task<IPageResponse<MenuItemResponse>> Get([FromQuery] IPageRequest request)
        {
            return await _service.Query(request);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryById/{id}")]
        public async Task<MenuItemResponse> Get(long id)
        {
            return await _service.Query(id);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MenuItemResponse> Post(CreateMenuRequest request)
        {
            switch (request.Type)
            {
                case MenuType.Api:
                    {
                        if (request.ApiId == null)
                        {
                            throw new BusException(-1, "当才菜单类型为接口时，接口不能为空。");
                        }
                        request.ViewId = null;
                    }
                    break;
                case MenuType.Group:
                case MenuType.View:
                    {
                        if (request.ViewId == null || request.ViewId == 0)
                        {
                            throw new BusException(-1, "当才菜单类型为视图或分组时，视图不能为空。");
                        }
                        request.ApiId = null;
                    }
                    break;
                default:
                    throw new BusException(-1, "未知的菜单类型。");
            }
            return await _service.Insert(request);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task Put(UpdateMenuRequest request)
        {
            switch (request.Type)
            {
                case MenuType.Api:
                    {
                        if (request.ApiId == null)
                        {
                            throw new BusException(-1, "当才菜单类型为接口时，接口不能为空。");
                        }
                        request.ViewId = null;
                    }
                    break;
                case MenuType.Group:
                case MenuType.View:
                    {
                        if (request.ViewId == null || request.ViewId == 0)
                        {
                            throw new BusException(-1, "当才菜单类型为视图或分组时，视图不能为空。");
                        }
                        request.ApiId = null;
                    }
                    break;
                default:
                    throw new BusException(-1, "未知的菜单类型。");
            }
            await _service.Update(request);
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        [HttpDelete]
        public async Task Delete(List<long> ids)
        {
            await _service.Delete(ids);
        }
    }
}