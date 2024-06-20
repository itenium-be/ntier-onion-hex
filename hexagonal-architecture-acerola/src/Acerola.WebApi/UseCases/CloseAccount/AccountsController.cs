namespace Acerola.WebApi.UseCases.CloseAccount
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Acerola.Application.Commands.CloseAccount;

    /// <summary>
    /// User Interface
    /// </summary>
    [Route("api/[controller]")]
    public sealed class AccountsController : Controller
    {
        private readonly ICloseAccountUseCase _closeService;

        public AccountsController(ICloseAccountUseCase closeService)
        {
            _closeService = closeService;
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> Close(Guid accountId)
        {
            Guid closeResult = await _closeService.Execute(accountId);
            if (closeResult == Guid.Empty)
            {
                return new NoContentResult();
            }
            return Ok();
        }
    }
}
