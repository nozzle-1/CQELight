using CQELight.DAL.Mapping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CQELight.DAL.MongoDb.Mapping
{
    public static class MongoDbMapper
    {
        #region Static properties

        private static readonly ConcurrentBag<MappingInfo> _mappings = new ConcurrentBag<MappingInfo>();

        internal static IEnumerable<MappingInfo> mappings => _mappings.AsEnumerable();

        #endregion

        #region Static methods

        internal static MappingInfo GetMapping<T>()
            => GetMapping(typeof(T));

        internal static MappingInfo GetMapping(Type t)
        {
            var mapping = _mappings.FirstOrDefault(m => m.EntityType == t);
            if (mapping == null)
            {
                mapping = new MappingInfo(t);
            }
            return mapping;
        }

        public static void CreateMappingFor<T>(Action<TypeMapper<T>> mapperConf)
        {
            if(mapperConf == null)
            {
                throw new InvalidOperationException("If you want to create a MongoDb mapping with attributes, you must specify the TypeMapper configuration");
            }
            var typeMapper = new TypeMapper<T>();
            mapperConf(typeMapper);
            _mappings.Add(typeMapper.GetMapping());
        }

        #endregion

    }
}
