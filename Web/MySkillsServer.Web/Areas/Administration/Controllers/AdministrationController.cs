namespace MySkillsServer.Web.Areas.Administration.Controllers
{
    using MySkillsServer.Common;
    using MySkillsServer.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
