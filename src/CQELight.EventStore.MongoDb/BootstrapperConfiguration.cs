﻿using CQELight.Abstractions.EventStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQELight.EventStore.MongoDb
{
    /// <summary>
    /// Class that carries all options to configure MongoDb
    /// </summary>
    public class MongoDbEventStoreBootstrapperConfiguration
    {

        #region Properties

        /// <summary>
        /// Collection of URLs to use as server.
        /// </summary>
        public IEnumerable<string> ServerUrls { get; }
        /// <summary>
        /// Provider of snapshot behavior.
        /// </summary>
        public ISnapshotBehaviorProvider SnapshotBehaviorProvider{ get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Create a new options class with following server urls.
        /// </summary>
        /// <param name="serversUrls">Collection of urls to use as server.</param>
        public MongoDbEventStoreBootstrapperConfiguration(params string[] serversUrls)
        {
            if (serversUrls == null)
            {
                throw new ArgumentNullException(nameof(serversUrls));
            }
            if (serversUrls.Length == 0)
            {
                throw new ArgumentException("MongoDbEventStoreBootstrapperConfiguration.ctor() : At least one url should be provided, for main server.", nameof(serversUrls));
            }
            if (serversUrls.Any(u => !u.StartsWith("mongodb://", StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException("MongoDbEventStoreBootstrapperConfiguration.ctor() : All provided url should be formatted like 'mongodb://{ipAdress[:port]}'", nameof(serversUrls));
            }
            ServerUrls = serversUrls.AsEnumerable();
        }

        #endregion

    }
}
