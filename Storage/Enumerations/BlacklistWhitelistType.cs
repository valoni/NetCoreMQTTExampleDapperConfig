﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlacklistWhitelistType.cs" company="Haemmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   An enumeration representing the available blacklist or whitelist types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Storage.Enumerations
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    ///     An enumeration representing the available blacklist or whitelist types.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BlacklistWhitelistType
    {
        /// <summary>
        ///     The subscribe blacklist or whitelist type.
        /// </summary>
        Subscribe,

        /// <summary>
        ///     The publish blacklist or whitelist type.
        /// </summary>
        Publish
    }
}
