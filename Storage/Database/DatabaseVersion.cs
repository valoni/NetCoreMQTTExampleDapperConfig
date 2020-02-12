﻿using System;

namespace Storage.Database
{
    /// <summary>
    /// The database version class. It contains information about the database version used.
    /// </summary>
    public class DatabaseVersion
    {
        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the version name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Gets or sets the created at timestamp.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the deleted at timestamp.
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at timestamp.
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; } = null;

        /// <summary>
        /// Returns a <see cref="string"></see> representation of the <see cref="DatabaseVersion"/> class.
        /// </summary>
        /// <returns>A <see cref="string"></see> representation of the <see cref="DatabaseVersion"/> class.</returns>
        public override string ToString()
        {
            return $"{{{nameof(this.Id)}: {this.Id}, {nameof(this.Name)}: {this.Name}, {nameof(this.Number)}: {this.Number}, {nameof(this.CreatedAt)}: {this.CreatedAt}, {nameof(this.DeletedAt)}: {this.DeletedAt}, {nameof(this.UpdatedAt)}: {this.UpdatedAt}}}";
        }
    }
}