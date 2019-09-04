using System;
using System.Collections.Generic;
using System.Text;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected void AssignCommonViewModelData(BaseMetaModel metaModel, BaseViewModel viewModel)
        {
            viewModel.BaseCurrency = metaModel.BaseCurrency;
            viewModel.AccessToken = HttpContext.Request.Query["access_token"];
            viewModel.Lang = metaModel.Lang;
            
        }
    }
}
