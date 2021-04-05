using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.helpers.DataReaderMapper.Mappers
{
    class GenericTypeMapper<TObject> : BaseMapper<TObject> where TObject : new()
    {
        private Dictionary<string, PropertyDescriptor> _properties;
        public GenericTypeMapper(IDataReader dataReader) : base(dataReader)
        {
            _properties = ExtractPropertiesToBeMapped();
        }
        public override TObject Map()
        {
            TObject returnObject = new TObject();
            for (int i = 0; i < DataReader.FieldCount; i++)
            {
                string columnName = DataReader.GetName(i);
                PropertyDescriptor property = _properties[columnName];
                object value = DataReader[i];
                if (value == DBNull.Value || value == null)
                    continue;
                Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(returnObject, ConvertValueType(type, value));
            }
            return returnObject;
        }
        private object ConvertValueType(Type type, object value)
        {
            return type.IsEnum
                ? Enum.ToObject(type, value)
                : Convert.ChangeType(value, type);
        }
    }
}
