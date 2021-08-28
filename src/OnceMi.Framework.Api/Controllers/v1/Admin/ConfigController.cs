﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnceMi.Framework.IService.Admin;
using OnceMi.Framework.Model.Dto;
using OnceMi.Framework.Model.Enums;
using System;
using System.Threading.Tasks;


namespace OnceMi.Framework.Api.Controllers.v1.Admin
{
    /// <summary>
    /// 配置管理
    /// </summary>
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IConfigsService _configsService;

        public ConfigController(ILogger<ConfigController> logger
            , IConfigsService configsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<ConfigController>));
            _configsService = configsService ?? throw new ArgumentNullException(nameof(IConfigsService));
        }

        /// <summary>
        /// 获取应用程序信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<SystemHardwareInfo> SystemHardwareInfo()
        {
            return await _configsService.SystemHardwareInfo();
        }
    }
}
