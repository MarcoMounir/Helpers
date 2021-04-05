using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.helpers.DataReaderMapper.Mappers
{
    class EnumTypeMapper<TObject> : BaseMapper<TObject> where TObject : new()
    {
        public EnumTypeMapper(IDataReader dataReader) : base(dataReader)
        {
        }
        public override TObject Map()
        {
            TObject returnObject = default(TObject);
            if (DataReader.FieldCount != 1)
                throw new Exception("Invalid SQL Command, it must return a primitive value");
            object value = DataReader[0];
            if (value == DBNull.Value || value == null)
                return returnObject;
            return (TObject)Enum.ToObject(typeof(TObject), value);
        }
    }
}
