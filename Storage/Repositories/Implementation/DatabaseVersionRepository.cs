﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Storage.Database;
using Storage.Repositories.Interfaces;
using Storage.Statements;

namespace Storage.Repositories.Implementation
{
    /// <inheritdoc cref="IDatabaseVersionRepository" />
    /// <summary>
    ///     An implementation supporting the repository pattern to work with <see cref="DatabaseVersion" />s.
    /// </summary>
    /// <seealso cref="IDatabaseVersionRepository" />
    public class DatabaseVersionRepository : IDatabaseVersionRepository
    {
        /// <summary>
        ///     The connection settings to use.
        /// </summary>
        private readonly DatabaseConnectionSettings _connectionSettings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseVersionRepository" /> class.
        /// </summary>
        /// <param name="connectionSettings">The connection settings to use.</param>
        public DatabaseVersionRepository(DatabaseConnectionSettings connectionSettings)
        {
            this._connectionSettings = connectionSettings;
        }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        /// Gets a <see cref="List{T}"/> of all <see cref="DatabaseVersion"/>s.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<IEnumerable<DatabaseVersion>> GetDatabaseVersions()
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            return await connection.QueryAsync<DatabaseVersion>(SelectStatements.SelectAllDatabaseVersions);
        }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        ///     Gets a <see cref="DatabaseVersion" /> by its id.
        /// </summary>
        /// <param name="databaseVersionId">The <see cref="DatabaseVersion"/>'s id.</param>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<DatabaseVersion> GetDatabaseVersionById(Guid databaseVersionId)
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<DatabaseVersion>(SelectStatements.SelectDatabaseVersionById, new { Id = databaseVersionId });
                    }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        ///     Gets a <see cref="DatabaseVersion" /> by its name.
        /// </summary>
        /// <param name="databaseVersionName">The <see cref="DatabaseVersion"/>'s name to query for.</param>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<DatabaseVersion> GetDatabaseVersionByName(string databaseVersionName)
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<DatabaseVersion>(SelectStatements.SelectDatabaseVersionByName, new { DatabaseVersionName = databaseVersionName });
        }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        /// Sets the <see cref="DatabaseVersion"/>'s state to deleted. (It will still be present in the database, but with a deleted timestamp).
        /// Returns the number of affected rows.
        /// </summary>
        /// <param name="databaseVersionId">The <see cref="DatabaseVersion"/>'s id.</param>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation providing the number of affected rows.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<bool> DeleteDatabaseVersion(Guid databaseVersionId)
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(UpdateStatements.MarkDatabaseVersionAsDeleted, new { Id = databaseVersionId });
            return result == 1;
        }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        /// Deletes a <see cref="DatabaseVersion"/> from the database.
        /// Returns the number of affected rows.
        /// </summary>
        /// <param name="databaseVersionId">The <see cref="DatabaseVersion"/>'s id.</param>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation providing the number of affected rows.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<bool> DeleteDatabaseVersionFromDatabase(Guid databaseVersionId)
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(DeleteStatements.DeleteDatabaseVersion, new { Id = databaseVersionId });
            return result == 1;
        }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        ///     Inserts a <see cref="DatabaseVersion" /> to the database.
        /// </summary>
        /// <param name="package">The <see cref="DatabaseVersion" /> to insert.</param>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<bool> InsertDatabaseVersion(DatabaseVersion package)
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(InsertStatements.InsertDatabaseVersion, package);
            return result == 1;
        }

        /// <inheritdoc cref="IDatabaseVersionRepository" />
        /// <summary>
        ///     Updates a <see cref="DatabaseVersion" /> in the database.
        /// </summary>
        /// <param name="package">The updated <see cref="DatabaseVersion" />.</param>
        /// <returns>A <see cref="Task" /> representing any asynchronous operation.</returns>
        /// <seealso cref="IDatabaseVersionRepository" />
        public async Task<bool> UpdateDatabaseVersion(DatabaseVersion package)
        {
            await using var connection = new NpgsqlConnection(this._connectionSettings.ToConnectionString());
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(UpdateStatements.UpdateDatabaseVersion, package);
            return result == 1;
        }
    }
}
