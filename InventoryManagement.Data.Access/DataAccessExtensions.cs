using System;
using System.Data.OleDb;
using InventoryManagement.Data.Web;

namespace InventoryManagement.Data.Access
{
    internal static class DataAccessExtensions
    {
        public static void Load(this DataObject dataObj, OleDbDataReader reader)
        {
            try
            {
                foreach (ColumnDefinition columnDef in dataObj.ColumnDefs())
                {
                    if (reader.GetValue(columnDef.ColumnIndex) != DBNull.Value)
                    {
                        switch (columnDef.ColumnFieldType)
                        {
                            case ColumnDefinition.FieldType.String:
                                dataObj.GetColumnValues()[columnDef.ColumnIndex] = reader.GetString(columnDef.ColumnIndex);
                                break;
                            case ColumnDefinition.FieldType.Int32:
                                dataObj.GetColumnValues()[columnDef.ColumnIndex] = reader.GetInt32(columnDef.ColumnIndex);
                                break;
                            case ColumnDefinition.FieldType.Boolean:
                                dataObj.GetColumnValues()[columnDef.ColumnIndex] = reader.GetBoolean(columnDef.ColumnIndex);
                                break;
                        }
                    }
                    else //default values for nulls
                    {
                        switch (columnDef.ColumnFieldType)
                        {
                            case ColumnDefinition.FieldType.String:
                                dataObj.GetColumnValues()[columnDef.ColumnIndex] = string.Empty;
                                break;
                            case ColumnDefinition.FieldType.Int32:
                                dataObj.GetColumnValues()[columnDef.ColumnIndex] = 0;
                                break;
                            case ColumnDefinition.FieldType.Boolean:
                                dataObj.GetColumnValues()[columnDef.ColumnIndex] = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //TODO: what to do here?
            }
        }
    }
}