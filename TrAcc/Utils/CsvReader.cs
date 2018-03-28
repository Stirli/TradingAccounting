using System;
using System.Data;
using System.IO;

namespace TrAcc.Utils
{
    public class CsvReader : IDataReader
    {
        readonly StreamReader _streamReader;

        readonly Func<string, object>[] _convertTable;
        readonly Func<string, bool>[] _constraintsTable;

        string[] _currentLineValues;
        string _currentLine;

        // Конструктор ридера CSV-файла.
        // Передаем полный абсолютный путь к файлу, таблицы функций ограничений и преобразований.
        // (о функциях ограничения и преобразования - читайте ниже, в пояснении).

        public CsvReader(string filepath, Func<string, bool>[] constraintsTable, Func<string, object>[] convertTable)
        {
            _constraintsTable = constraintsTable;
            _convertTable = convertTable;
            _streamReader = new StreamReader(filepath);

            _currentLine = null;
            _currentLineValues = null;
        }

        // Возвращаем значение, используя одну из функций преобразования и обработку исключения.
        // Это обезопасит нас от прерывания загрузки данных.

        public object GetValue(int i)
        {
            try
            {
                return _convertTable[i](_currentLineValues[i]);
            }
            catch (Exception)
            {
                return null;
            }
        }


        // Чтение очередной строки.
        // Используем функции ограничения для того, чтобы еще на этапе чтения понять, что строка
        // вызовет исключения при передаче ее в SqlBulkCopy, поэтому мы пропускаем некорректные строки.


        public bool Read()
        {
            if (_streamReader.EndOfStream) return false;

            _currentLine = _streamReader.ReadLine();

            // В случае, если значения будут содержать символ ";" это работать не будет,
            // и придется использовать более сложный алгоритм разбора.
            _currentLineValues = _currentLine.Split(';');

            var invalidRow = false;
            for (int i = 0; i < _currentLineValues.Length; i++)
            {
                if (!_constraintsTable[i](_currentLineValues[i]))
                {
                    invalidRow = true;
                    break;
                }
            }

            return !invalidRow || Read();
        }

        public int Depth { get; set; }
        public bool IsClosed { get; set; }
        public int RecordsAffected { get; set; }

        // Возвращем число столбцов в csv файле.
        // Нам заранее известно, что 4, поэтому не будем усложнять код.

        public int FieldCount
        {
            get { return 4; }
        }

        // Освобождаем ресурсы. Закрываем поток.

        public void Dispose()
        {
            _streamReader.Close();
        }

        // ... множестве нереализованных методов IDataReader, которые здесь не нужны.

        #region MyRegion

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public object this[int i]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public object this[string name]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }
        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}