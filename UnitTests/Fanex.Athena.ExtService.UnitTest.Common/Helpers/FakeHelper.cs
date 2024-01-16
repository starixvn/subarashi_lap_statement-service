namespace Fanex.Athena.ExtService.UnitTest.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using AutoFixture;

    public static class FakeHelper
    {
        private static readonly Fixture Fixture = new();

        public static T Create<T>(Expression<Func<T, object>> propertyPicker, object value)
        {
            return Fixture.Build<T>().With(propertyPicker, value).Create();
        }

        public static T Create<T>(IEnumerable<FakeProperty<T>> properties)
        {
            var fixture = Fixture.Build<T>();
            foreach (var property in properties)
            {
                fixture.With(property.PropertyPicker, property.Value);
            }

            return fixture.Create();
        }

        public static T Create<T>()
        {
            return Fixture.Create<T>();
        }

        public static IEnumerable<T> CreateMany<T>()
        {
            return Fixture.CreateMany<T>();
        }

        public static IEnumerable<T> CreateMany<T>(int count)
        {
            return Fixture.CreateMany<T>(count);
        }

        public static DateTime TruncateMilliseconds(DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }

    public class FakeProperty<T>
    {
        public Expression<Func<T, object>> PropertyPicker;
        public object Value;

        public FakeProperty(Expression<Func<T, object>> propertyPicker, object value)
        {
            PropertyPicker = propertyPicker;
            Value = value;
        }
    }
}