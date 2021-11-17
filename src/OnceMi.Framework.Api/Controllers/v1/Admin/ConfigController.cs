﻿using Microsoft.AspNetCore.Mvc;
using OnceMi.Framework.IService.Admin;
using OnceMi.Framework.Model.Dto;
using OnceMi.Framework.Model.Enums;


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
        private readonly IConfigService _configsService;

        public ConfigController(ILogger<ConfigController> logger
            , IConfigService configsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<ConfigController>));
            _configsService = configsService ?? throw new ArgumentNullException(nameof(IConfigService));
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
