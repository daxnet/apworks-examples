using System;

namespace Apworks.Examples.TaskList.Models
{
    /// <summary>
    /// Represents a task item in the task list domain.
    /// </summary>
    /// <seealso cref="Apworks.IAggregateRoot{System.Guid}" />
    public sealed class TaskItem : IAggregateRoot<Guid>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the task.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current task item has been done.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the task has been done; otherwise, <c>false</c>.
        /// </value>
        public bool Done { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Title;
    }
}
