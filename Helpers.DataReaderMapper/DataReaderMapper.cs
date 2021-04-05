using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using com.helpers.DataReaderMapper.Attributes;

namespace com.helpers.DataReaderMapper
{
    public class DataReaderMapper<TObject> where TObject : new()
    {
        private IDataReader _dataReader;
        private Dictionary<string, PropertyInfo> _propertiesDictionary;
        private bool _isTObjectOfNonGenericClassType;
        private Type _TObjectType;
        public DataReaderMapper(IDataReader dataReader)
        {
            //ExtractPropertiesToBeMapped();
            _TObjectType = Nullable.GetUnderlyingType(typeof(TObject)) ?? typeof(TObject);
            _isTObjectOfNonGenericClassType = _TObjectType.IsPrimitive || _TObjectType.IsGenericType || _TObjectType.IsEnum;
        }

        
        private object ConvertValueType(Type type, object value)
        {
            return type.IsEnum
                ? Enum.ToObject(type, value)
                : Convert.ChangeType(value, type);
        }
        public TObject Map()
        {
            TObject returnObject = default(TObject);
            if (_isTObjectOfNonGenericClassType)
                return MapNonClassType(returnObject);
            else
                returnObject = MapClassType();
            return returnObject;
        }

        private TObject MapClassType()
        {
            TObject returnObject = new TObject();
            for (int i = 0; i < _dataReader.FieldCount; i++)
            {
                string columnName = _dataReader.GetName(i);
                PropertyInfo property = _propertiesDictionary[columnName];
                object value = _dataReader[i];
                if (value == DBNull.Value || value == null)
                    continue;
                Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(returnObject, ConvertValueType(type, value));
            }

            return returnObject;
        }

        private TObject MapNonClassType(TObject returnObject)
        {
            if (_dataReader.FieldCount != 1)
                throw new Exception("Invalid SQL Command, it must return a primitive value");
            object value = _dataReader[0];
            if (value == DBNull.Value || value == null)
                return returnObject;
            return (TObject) ConvertValueType(_TObjectType, value);
        }
    }
}
