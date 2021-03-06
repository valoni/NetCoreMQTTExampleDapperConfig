﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="Haemmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   The user controller class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NetCoreMQTTExampleDapperConfig.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using NetCoreMQTTExampleDapperConfig.Controllers.Extensions;

    using NSwag.Annotations;

    using Serilog;

    using Storage.Database;
    using Storage.Dto;
    using Storage.Repositories.Interfaces;

    /// <summary>
    ///     The user controller class.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/user")]
    [ApiController]
    [OpenApiTag("User", Description = "User management.")]
    public class UserController : ControllerBase
    {
        /// <summary>
        ///     The auto mapper.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        private readonly IMapper autoMapper;

        /// <summary>
        ///     The password hasher.
        /// </summary>
        private readonly IPasswordHasher<User> passwordHasher;

        /// <summary>
        ///     The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="autoMapper">The <see cref="IMapper" />.</param>
        /// <param name="userRepository">The <see cref="IUserRepository" />.</param>

        // ReSharper disable once StyleCop.SA1650
        public UserController(IMapper autoMapper, IUserRepository userRepository)
        {
            this.autoMapper = autoMapper;
            this.passwordHasher = new PasswordHasher<User>();
            this.userRepository = userRepository;
        }

        /// <summary>
        ///     Gets all users.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task" /> representing any asynchronous operation.
        /// </returns>
        /// <remarks>
        ///     Gets all users.
        /// </remarks>
        /// <response code="200">Users found.</response>
        /// <response code="500">Internal server error.</response>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DtoReadUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DtoReadUser>>> GetUsers()
        {
            try
            {
                Log.Information("Executed GetUsers().");

                var users = await this.userRepository.GetUsers();
                var usersList = users?.ToList();

                if (usersList?.Count == 0)
                {
                    return this.Ok("[]");
                }

                var returnUsers = this.autoMapper.Map<IEnumerable<DtoReadUser>>(users);
                return this.Ok(returnUsers);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        ///     Gets a user by their id.
        /// </summary>
        /// <param name="userId">
        ///     The user identifier.
        /// </param>
        /// <returns>
        ///     A <see cref="Task" /> representing any asynchronous operation.
        /// </returns>
        /// <remarks>
        ///     Gets a user by their id.
        /// </remarks>
        /// <response code="200">User found.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(DtoReadUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DtoReadUser>> GetUserById(Guid userId)
        {
            try
            {
                Log.Information($"Executed GetUserById({userId}).");

                var user = await this.userRepository.GetUserById(userId);

                if (user == null)
                {
                    Log.Warning($"User with identifier {userId} not found.");
                    return this.NotFound(userId);
                }

                var returnUser = this.autoMapper.Map<DtoReadUser>(user);
                return this.Ok(returnUser);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        ///     Creates a user.
        /// </summary>
        /// <param name="createUser">
        ///     The create user.
        /// </param>
        /// <returns>
        ///     A <see cref="Task" /> representing any asynchronous operation.
        /// </returns>
        /// <remarks>
        ///     Creates a user.
        /// </remarks>
        /// <response code="200">User created.</response>
        /// <response code="400">User not created.</response>
        /// <response code="409">User already exists.</response>
        /// <response code="500">Internal server error.</response>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [ProducesResponseType(typeof(DtoReadUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DtoCreateUpdateUser), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DtoCreateUpdateUser), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUser([FromBody] DtoCreateUpdateUser createUser)
        {
            try
            {
                Log.Information($"Executed CreateUser({createUser}).");

                var user = this.autoMapper.Map<User>(createUser);
                user.Id = Guid.NewGuid();

                var userExists = await this.userRepository.UserNameExists(createUser.UserName);

                if (userExists)
                {
                    return this.Conflict(createUser);
                }

                var inserted = await this.userRepository.InsertUser(user);

                if (!inserted)
                {
                    return this.BadRequest(createUser);
                }

                var returnUser = this.autoMapper.Map<DtoReadUser>(user);
                return this.Ok(returnUser);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        ///     Updates a user.
        /// </summary>
        /// <param name="userId">
        ///     The user identifier.
        /// </param>
        /// <param name="updateUser">
        ///     The update user.
        /// </param>
        /// <returns>
        ///     A <see cref="Task" /> representing any asynchronous operation.
        /// </returns>
        /// <remarks>
        ///     Updates a user.
        /// </remarks>
        /// <response code="200">User updated.</response>
        /// <response code="400">User not updated.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:ElementDocumentationMustNotBeCopiedAndPasted", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(DtoReadUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUser(Guid userId, [FromBody] DtoCreateUpdateUser updateUser)
        {
            try
            {
                Log.Information($"Executed UpdateUser({updateUser}) for user identifier: {userId}.");

                var resultUser = await this.userRepository.GetUserById(userId);

                if (resultUser == null)
                {
                    Log.Warning($"User with identifier {userId} not found.");
                    return this.NotFound(userId);
                }

                resultUser = this.autoMapper.Map<User>(updateUser);
                resultUser.Id = userId;
                resultUser.PasswordHash = this.passwordHasher.HashPassword(resultUser, updateUser.Password);

                var updated = await this.userRepository.UpdateUser(resultUser);

                if (!updated)
                {
                    return this.BadRequest(userId);
                }

                var returnUser = this.autoMapper.Map<DtoReadUser>(resultUser);
                return this.Ok(returnUser);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        ///     Deletes the user by their id.
        /// </summary>
        /// <param name="userId">
        ///     The user identifier.
        /// </param>
        /// <returns>
        ///     A <see cref="Task" /> representing any asynchronous operation.
        /// </returns>
        /// <remarks>
        ///     Deletes a user by their id.
        /// </remarks>
        /// <response code="200">User deleted.</response>
        /// <response code="400">User not deleted.</response>
        /// <response code="500">Internal server error.</response>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserById(Guid userId)
        {
            try
            {
                Log.Information($"Executed DeleteUserById({userId}).");
                var deleted = await this.userRepository.DeleteUser(userId);

                if (deleted)
                {
                    return this.Ok(userId);
                }

                return this.BadRequest(userId);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
                return this.InternalServerError(ex);
            }
        }
    }
}
