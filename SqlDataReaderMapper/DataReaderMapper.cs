using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using SqlDataReaderMapper.Attributes;

namespace SqlDataReaderMapper
{
    public class DataReaderMapper<TObject> where TObject : new()
    {
        private IDataReader _dataReader;

        public DataReaderMapper(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        private object ConvertValueType(Type type, object value)
        {
            return type.IsEnum
                ? Enum.ToObject(type, value)
                : Convert.ChangeType(value, type);
        }
        public TObject Map()
        {
            TObject returnObject = default;
            Type objectType = Nullable.GetUnderlyingType(typeof(TObject)) ?? typeof(TObject);
            if (objectType.IsPrimitive || objectType.IsGenericType || objectType.IsEnum)
                return MapNonClassType(returnObject, objectType);
            else
                returnObject = MapClassType();
            return returnObject;
        }

        private TObject MapClassType()
        {
            TObject returnObject = new TObject();
            Dictionary<string, PropertyInfo> customAttributesWithPropDictionary = returnObject.GetType()
                .GetProperties()
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<ColumnAttribute>() != null)
                .ToDictionary(propertyInfo => propertyInfo.GetCustomAttribute<ColumnAttribute>().Name,
                    propertyInfo => propertyInfo);
            for (int i = 0; i < _dataReader.FieldCount; i++)
            {
                string columnName = _dataReader.GetName(i);
                PropertyInfo property = customAttributesWithPropDictionary[columnName];
                object value = _dataReader[i];
                if (value == DBNull.Value || value == null)
                    continue;
                Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(returnObject, ConvertValueType(type, value));
            }

            return returnObject;
        }

        private TObject MapNonClassType(TObject returnObject, Type objectType)
        {
            if (_dataReader.FieldCount != 1)
                throw new Exception("Invalid SQL Command, it must return a primitive value");
            object value = _dataReader[0];
            if (value == DBNull.Value || value == null)
                return returnObject;
            return (TObject) ConvertValueType(objectType, value);
        }
    }
}
