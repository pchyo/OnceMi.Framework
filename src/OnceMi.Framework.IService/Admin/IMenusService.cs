﻿using OnceMi.Framework.Entity.Admin;
using OnceMi.Framework.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnceMi.Framework.IService.Admin
{
    public interface IMenusService : IBaseService<Menus, long>
    {
        /// <summary>
        /// 获取菜单类型
        /// </summary>
        /// <returns></returns>
        Task<List<ISelectResponse<int>>> QueryMenuTypes();

        /// <summary>
        /// 查询当前菜单的下溢菜单Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> QueryNextSortValue(long? parentId);

        /// <summary>
        /// 查询详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MenuItemResponse> Query(long id);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onlyQueryEnabled"></param>
        /// <returns></returns>
        Task<IPageResponse<MenuItemResponse>> Query(IPageRequest request, bool onlyQueryEnabled = false);

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<Menus>> Query(List<long> menuIds);

        Task<MenuItemResponse> Insert(CreateMenuRequest request);

        Task Update(UpdateMenuRequest request);

        Task Delete(List<long> ids);
    }
}
