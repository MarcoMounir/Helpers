using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using com.helpers.DataReaderMapper;
using com.helpers.DataReaderMapper.Mappers;
using NSubstitute;

namespace Tests
{
    [TestClass]
    public class FakeDataReaderTests
    {
        private TestTable CreateTestTableObject()
        {
            return new TestTable()
            {
                Id = 1,
                CharCol = 'C',
                DateTimeCol = new DateTime(2020, 09, 21, 12, 12, 12),
                DecimalCol = 123.44m,
                EnumCol = TestEnum.Active,
                IntCol = 33,
                MoneyCol = 333555.22m,
                NullableCol = null,
                NullableDateTimeCol = null,
                NullableDecimalCol = null,
                NullableEnumCol = null,
                NullableIntCol = 33,
                StrCol = "1234",
                StrCol2 = "555"
            };
        }

        [TestMethod]
        public void FakeDataReader_Success()
        {
            TestTable testTable = CreateTestTableObject();
            var expectedReturn = BaseMapper<TestTable>.GetMapper(MockDataReader(testTable)).Map();
            Assert.AreEqual(expectedReturn.Id, testTable.Id);
            Assert.AreEqual(expectedReturn.CharCol, testTable.CharCol);
            Assert.AreEqual(expectedReturn.DateTimeCol, testTable.DateTimeCol);
            Assert.AreEqual(expectedReturn.DecimalCol, testTable.DecimalCol);
            Assert.AreEqual(expectedReturn.EnumCol, testTable.EnumCol);
            Assert.AreEqual(expectedReturn.IntCol, testTable.IntCol);
            Assert.AreEqual(expectedReturn.MoneyCol, testTable.MoneyCol);
            Assert.AreEqual(expectedReturn.NullableIntCol, testTable.NullableIntCol);
            Assert.AreEqual(expectedReturn.NullableCol, testTable.NullableCol);
            Assert.AreEqual(expectedReturn.NullableDateTimeCol, testTable.NullableDateTimeCol);
            Assert.AreEqual(expectedReturn.NullableDecimalCol, testTable.NullableDecimalCol);
            Assert.AreEqual(expectedReturn.NullableEnumCol, testTable.NullableEnumCol);
            Assert.AreEqual(expectedReturn.StrCol, testTable.StrCol);
            Assert.AreEqual(expectedReturn.StrCol2, testTable.StrCol2);
            Assert.AreEqual(expectedReturn.BooleanCol, testTable.BooleanCol);
            Assert.AreEqual(expectedReturn.NullableBooleanCol, testTable.NullableBooleanCol);
        }

        private static IDataReader MockDataReader(TestTable testTable)
        {
            IDataReader reader = NSubstitute.Substitute.For<IDataReader>();
            reader[0].Returns(x => testTable.Id);
            reader.GetName(0).Returns(x => "Id");
            reader[1].Returns(x => testTable.CharCol);
            reader.GetName(1).Returns(x => "CharColumn");
            reader[2].Returns(x => testTable.DateTimeCol);
            reader.GetName(2).Returns(x => "DateTimeColumn");
            reader[3].Returns(x => testTable.DecimalCol);
            reader.GetName(3).Returns(x => "DecimalColumn");
            reader[4].Returns(x => testTable.EnumCol);
            reader.GetName(4).Returns(x => "EnumColumn");
            reader[5].Returns(x => testTable.IntCol);
            reader.GetName(5).Returns(x => "IntColumn");
            reader[6].Returns(x => testTable.MoneyCol);
            reader.GetName(6).Returns(x => "MoneyColumn");
            reader[7].Returns(x => testTable.NullableCol);
            reader.GetName(7).Returns(x => "NullColumn");
            reader[8].Returns(x => testTable.NullableDateTimeCol);
            reader.GetName(8).Returns(x => "NullableDateTimeColumn");
            reader[9].Returns(x => testTable.NullableDecimalCol);
            reader.GetName(9).Returns(x => "NullableDecimalColumn");
            reader[10].Returns(x => testTable.NullableEnumCol);
            reader.GetName(10).Returns(x => "NullableEnumColumn");
            reader[11].Returns(x => testTable.NullableIntCol);
            reader.GetName(11).Returns(x => "NullableIntColumn");
            reader[12].Returns(x => testTable.StrCol);
            reader.GetName(12).Returns(x => "StringColumn");
            reader[13].Returns(x => testTable.StrCol2);
            reader.GetName(13).Returns(x => "StringColumn2");
            reader[14].Returns(x => testTable.BooleanCol);
            reader.GetName(14).Returns(x => "BooleanColumn");
            reader[15].Returns(x => testTable.NullableBooleanCol);
            reader.GetName(15).Returns(x => "NullableBooleanColumn");
            reader.FieldCount.Returns(x => typeof(TestTable).GetProperties().Length);
            return reader;
        }
    }
}
