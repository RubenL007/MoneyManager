using System.Reflection;

namespace MoneyManager.Shared.Components
{
    public class PropertyWrapper<T>
    {
        private readonly object _item;
        private readonly PropertyInfo _property;

        public PropertyWrapper(object item, PropertyInfo property)
        {
            _item = item;
            _property = property;
        }

        public T Value
        {
            get => (T)_property.GetValue(_item)!;
            set => _property.SetValue(_item, value);
        }

    }
    public static class PropertyWrapperHelper
    {
        //this changes the original Value given and not the copy, this ensures the bind and update are correctly done to the original value
        public static PropertyWrapper<string> GetStringWrapper(object item, PropertyInfo property)
        {
            return new PropertyWrapper<string>(item, property);
        }
        public static PropertyWrapper<double> GetDoubleWrapper(object item, PropertyInfo property)
        {
            return new PropertyWrapper<double>(item, property);
        }
        public static PropertyWrapper<bool> GetBoolWrapper(object item, PropertyInfo property)
        {
            return new PropertyWrapper<bool>(item, property);
        }
        public static PropertyWrapper<DateTime> GetDateTimeWrapper(object item, PropertyInfo property)
        {
            return new PropertyWrapper<DateTime>(item, property);
        }
        public static PropertyWrapper<object> GetObjectWrapper(object item, PropertyInfo property)
        {
            return new PropertyWrapper<object>(item, property);
        }
    }
}
