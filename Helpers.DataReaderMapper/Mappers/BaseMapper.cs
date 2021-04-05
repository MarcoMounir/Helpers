using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using com.helpers.DataReaderMapper.Attributes;

namespace com.helpers.DataReaderMapper.Mappers
{
    public abstract class BaseMapper<TObject> where TObject : new()
    {
        protected IDataReader DataReader;
        protected BaseMapper(IDataReader dataReader)
        {
            DataReader = dataReader;
        }

        public abstract TObject Map();

        public static BaseMapper<TObject> GetMapper(IDataReader dataReader)
        {
            Type objectType = Nullable.GetUnderlyingType(typeof(TObject)) ?? typeof(TObject);
            if (objectType.IsPrimitive)
                return new PrimitiveTypeMapper<TObject>(dataReader);
            else if (objectType.IsEnum)
                return new EnumTypeMapper<TObject>(dataReader);
            else
                return new GenericTypeMapper<TObject>(dataReader);
        }
        
        protected Dictionary<string, PropertyDescriptor> ExtractPropertiesToBeMapped()
        {
            return TypeDescriptor.GetProperties(typeof(TObject)).Cast<PropertyDescriptor>().ToDictionary(
                descriptorKey => descriptorKey.Attributes.OfType<ColumnAttribute>().FirstOrDefault()?.Name,
                descriptorValue => descriptorValue);
        }

    }
}
