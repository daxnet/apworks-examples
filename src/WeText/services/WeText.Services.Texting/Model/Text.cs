using Apworks;
using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Shared.Events;

namespace WeText.Services.Texting.Model
{
    public class Text : AggregateRootWithEventSourcing<Guid>
    {
        public Text(Guid id) : base(id)
        {
        }

        public Text(Guid id, string accountName, string content, bool isPublic)
            : this(id)
        {
            this.Apply<TextAccountNameChangedEvent>(new TextAccountNameChangedEvent(accountName));
            this.Apply<TextContentChangedEvent>(new TextContentChangedEvent(content));
            this.Apply<TextIsPublicChangedEvent>(new TextIsPublicChangedEvent(isPublic));
        }

        public void UpdateContent(string content)
        {
            this.Apply<TextContentChangedEvent>(new TextContentChangedEvent(content));
        }

        public void Publish()
        {
            this.Apply<TextIsPublicChangedEvent>(new TextIsPublicChangedEvent(true));
        }

        public void UnPublish()
        {
            this.Apply<TextIsPublicChangedEvent>(new TextIsPublicChangedEvent(false));
        }

        public string AccountName { get; private set; }

        public string Content { get; private set; }

        public bool IsPublic { get; private set; }

        [Handles(typeof(TextAccountNameChangedEvent))]
        private void HandlesAccountNameChangedEvent(TextAccountNameChangedEvent e)
        {
            this.AccountName = e.AccountName;
        }

        [Handles(typeof(TextContentChangedEvent))]
        private void HandlesContentChangedEvent(TextContentChangedEvent e)
        {
            this.Content = e.Content;
        }

        [Handles(typeof(TextIsPublicChangedEvent))]
        private void HandlesIsPublicChangedEvent(TextIsPublicChangedEvent e)
        {
            this.IsPublic = e.IsPublic;
        }
    }
}
