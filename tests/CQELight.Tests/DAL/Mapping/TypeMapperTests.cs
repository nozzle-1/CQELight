using CQELight.DAL.Mapping;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CQELight.Tests.DAL.Mapping
{
    public class TypeMapperTests
    {
        private class MapperTest
        {
            public int MapperId { get; set; }
        }

        [Fact]
        public void Mapper_Ctor_Should_Capture_Type()
        {
            var mapper = new TypeMapper<MapperTest>();

            mapper.objectType.Should().Be(typeof(MapperTest));
        }

        [Fact]
        public void Mapper_SetIdField_Should_Define_Property()
        {
            var mapper = new TypeMapper<MapperTest>();

            mapper.HasId(c => c.MapperId);

            mapper.idField.Name.Should().Be("MapperId");
        }
    }
}
