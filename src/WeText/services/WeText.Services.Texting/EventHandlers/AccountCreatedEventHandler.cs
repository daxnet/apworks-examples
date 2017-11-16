using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Threading;
using System.Threading.Tasks;
using WeText.Services.Shared.Events;
using WeText.Services.Texting.Model;
using Apworks.Events;

namespace WeText.Services.Texting.EventHandlers
{
    internal class AccountCreatedEventHandler : EventHandler<AccountCreatedEvent>
    {
        private readonly IConfigurationRoot config;
        private readonly string connectionString;

        public AccountCreatedEventHandler(IConfigurationRoot configuration)
        {
            this.config = configuration;
            this.connectionString = config["mssql:query.db"];
        }

        public override async Task<bool> HandleAsync(AccountCreatedEvent message, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                var sql = "INSERT INTO [dbo].[Accounts] ([Name], [Email], [NickName], [DateCreated]) VALUES (@Name, @Email, @NickName, @DateCreated)";
                var model = new Account
                {
                    Name = message.AccountName,
                    NickName = message.DisplayName,
                    Email = message.Email,
                    DateCreated = message.Timestamp
                };

                return await connection.ExecuteAsync(sql, model) == 1;
            }
        }
    }
}
