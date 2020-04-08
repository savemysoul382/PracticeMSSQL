using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DataProviderFactory.BulkImport
{
    public sealed class MyDataReader<T> : IMyDataReader<T>
    {
        #region Fields

        private Int32 field_count;
        private Int32 depth;
        private Boolean is_closed;
        private Int32 records_affected;
        private IList<T> records;
        private Int32 current_index = -1;

        private readonly PropertyInfo[] property_infos;
        private readonly Dictionary<String, Int32> name_dictionary;

        #endregion


        #region Properties

        public Int32 Depth => this.depth;

        public Boolean IsClosed => this.is_closed;

        public Int32 RecordsAffected => this.records_affected;

        public IList<T> Records
        {
            get => this.records;
            set => this.records = value;
        }

        #endregion


        #region Command

        #endregion


        #region Constructor

        public MyDataReader(IList<T> records)
        {
            Records = records;
            this.property_infos = typeof(T).GetProperties();
            this.name_dictionary = this.property_infos
                .Select((x, index) => new {x.Name, index})
                .ToDictionary(pair => pair.Name, pair => pair.index);
        }

        #endregion


        #region Events

        #endregion


        #region Privats methods

        #endregion


        #region Public methods

        public Boolean Read()
        {
            if ((this.current_index + 1) >= Records.Count) return false;
            this.current_index++;
            return true;
        }

        public Int32 FieldCount => this.property_infos.Length;

        public String GetName(Int32 i) => i >= 0 && i < FieldCount ? this.property_infos[i].Name : String.Empty;
        

        public String GetDataTypeName(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Object GetValue(Int32 i) => this.property_infos[i].GetValue(Records[this.current_index]);


        public Int32 GetOrdinal(String name) => this.name_dictionary.ContainsKey(name) ? this.name_dictionary[name] : -1;


        public Type GetFieldType(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Int32 GetValues(Object[] values)
        {
            throw new NotImplementedException();
        }

        public Boolean GetBoolean(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Byte GetByte(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Int64 GetBytes(Int32 i, Int64 field_offset, Byte[] buffer, Int32 bufferoffset, Int32 length)
        {
            throw new NotImplementedException();
        }

        public Char GetChar(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Int64 GetChars(Int32 i, Int64 fieldoffset, Char[] buffer, Int32 bufferoffset, Int32 length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Int16 GetInt16(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Int32 GetInt32(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Int64 GetInt64(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Single GetFloat(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Double GetDouble(Int32 i)
        {
            throw new NotImplementedException();
        }

        public String GetString(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Decimal GetDecimal(Int32 i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(Int32 i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Boolean IsDBNull(Int32 i)
        {
            throw new NotImplementedException();
        }

        public Object this[Int32 i] => throw new NotImplementedException();

        public Object this[String name] => throw new NotImplementedException();

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public Boolean NextResult()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}