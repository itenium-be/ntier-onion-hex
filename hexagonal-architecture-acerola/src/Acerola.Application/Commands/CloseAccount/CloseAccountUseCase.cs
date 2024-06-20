namespace Acerola.Application.Commands.CloseAccount
{
    using System;
    using System.Threading.Tasks;
    using Acerola.Application.Repositories;
    using Acerola.Domain.Accounts;

    /// <summary>
    /// Application Layer:
    /// ICloseAccountUseCase: Incoming Port
    /// CloseAccountUseCase: Primary / Driving Adapter
    /// </summary>
    public sealed class CloseAccountUseCase : ICloseAccountUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly IAccountWriteOnlyRepository _accountWriteOnlyRepository;

        public CloseAccountUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository)
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _accountWriteOnlyRepository = accountWriteOnlyRepository;
        }

        public async Task<Guid> Execute(Guid accountId)
        {
            // Account == Domain Model
            Account account = await _accountReadOnlyRepository.Get(accountId);
			      if (account == null)
                throw new AccountNotFoundException($"The account {accountId} does not exists or is already closed.");
			
            account.Close();
            await _accountWriteOnlyRepository.Delete(account);
            return account.Id;
        }
    }
}
