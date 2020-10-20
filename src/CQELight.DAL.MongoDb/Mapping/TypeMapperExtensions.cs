using CQELight.DAL.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQELight.DAL.MongoDb.Mapping
{
    static class TypeMapperExtensions
    {
        public static MappingInfo GetMapping<T>(this TypeMapper<T> typeMapper)
        {
            var mapping = new MappingInfo(typeof(T))
            {
                IdProperty = typeMapper.idField.Name
            };
            return mapping;
        }
    }
}
