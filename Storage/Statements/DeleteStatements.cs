﻿using System.Diagnostics.CodeAnalysis;

namespace Storage.Statements
{
    /// <summary>
    ///     The SQL statements for deleting data.
    /// </summary>
    public class DeleteStatements
    {
        /// <summary>
        ///     A SQL query string to delete a user from the database.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static string DeleteUser =
            @"DELETE FROM user WHERE id = @Id;";

        /// <summary>
        ///     A SQL query string to delete a database version from the database.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static string DeleteDatabaseVersion =
            @"DELETE FROM databaseversion WHERE id = @Id;";

        /// <summary>
        ///     A SQL query string to delete a blacklist item from the database.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static string DeleteBlacklistItem =
            @"DELETE FROM blacklist WHERE id = @Id;";

        /// <summary>
        ///     A SQL query string to delete a whitelist item from the database.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static string DeleteWhitelistItem =
            @"DELETE FROM whitelist WHERE id = @Id;";
    }
}
