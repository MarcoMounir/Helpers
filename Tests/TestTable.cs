using System;
using com.helpers.DataReaderMapper.Attributes;

namespace Tests
{
    public class TestTable
    {
        
        [Column("Id")]
        public long Id { get; set; }
        [Column("MoneyColumn")]
        public decimal MoneyCol { get; set; }
        [Column("DecimalColumn")]
        public decimal DecimalCol { get; set; }
        [Column("StringColumn")]
        public string StrCol { get; set; }
        [Column("CharColumn")]
        public char CharCol { get; set; }
        [Column("StringColumn2")]
        public string StrCol2 { get; set; }
        [Column("NullColumn")]
        public int? NullableCol { get; set; }
        [Column("IntColumn")]
        public int IntCol { get; set; }
        [Column("EnumColumn")]
        public TestEnum EnumCol { get; set; }
        [Column("NullableIntColumn")]
        public int? NullableIntCol { get; set; }
        [Column("NullableDecimalColumn")]
        public decimal? NullableDecimalCol { get; set; }
        [Column("NullableEnumColumn")]
        public TestEnum? NullableEnumCol { get; set; }

        [Column("NullableDateTimeColumn")]
        public DateTime? NullableDateTimeCol { get; set; }
        [Column("DateTimeColumn")]
        public DateTime DateTimeCol { get; set; }
        [Column("BooleanColumn")]
        public bool BooleanCol { get; set; }
        [Column("NullableBooleanColumn")]
        public bool? NullableBooleanCol { get; set; }

    }

    public enum TestEnum
    {
        Active = 1,
        Inactive=2
    }
}
