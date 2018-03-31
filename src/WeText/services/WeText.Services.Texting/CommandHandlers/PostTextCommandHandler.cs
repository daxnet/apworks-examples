using Apworks.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Texting.Commands;
using System.Threading;
using Apworks.Repositories;
using WeText.Services.Texting.Model;

namespace WeText.Services.Texting.CommandHandlers
{
    public class PostTextCommandHandler : CommandHandler<PostTextCommand>
    {
        private readonly IDomainRepository domainRepository;

        public PostTextCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task<bool> HandleAsync(PostTextCommand message, CancellationToken cancellationToken = default(CancellationToken))
        {
            var text = new Text(Guid.NewGuid(), message.AccountName, message.Text, message.IsPublic);

            await domainRepository.SaveAsync<Guid, Text>(text, cancellationToken);

            return true;
        }
    }
}
