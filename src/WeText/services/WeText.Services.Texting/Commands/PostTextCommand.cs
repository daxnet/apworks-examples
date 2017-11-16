using Apworks.Commands;

namespace WeText.Services.Texting.Commands
{
    /// <summary>
    /// Represents the text posting command.
    /// </summary>
    /// <seealso cref="Apworks.Commands.Command" />
    public class PostTextCommand : Command
    {
        public PostTextCommand(string accountName, bool isPublic, string text)
        {
            this.AccountName = accountName;
            this.IsPublic = isPublic;
            this.Text = text;
        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>
        /// The name of the account.
        /// </value>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the text is public to all users.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the text is public; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets the text to be posted.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
    }
}
