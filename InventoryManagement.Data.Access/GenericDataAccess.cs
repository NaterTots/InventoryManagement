using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

using InventoryManagement.Data.Web;

namespace InventoryManagement.Data.Access
{
    public class GenericDataAccess : IDisposable
    {
        #region Members

        // Track whether Dispose has been called.
        private bool disposed = false;

        OleDbConnection _accessConnection = null;

        #endregion

        #region Constructor

        public GenericDataAccess()
        {
            try
            {
                //TODO: put this into the web.config file
                _accessConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; " +
                        "Data Source=" + @"C:\Documents and Settings\Nate\My Documents\Visual Studio 2010\Projects\InventoryManagement\Database\ImperialInventoryManagement.accdb");
                _accessConnection.Open();
            }
            catch (Exception e)
            {
            }
        }

        #endregion

        #region Commodities

        public List<Commodity> LoadCommodities(string whereClause)
        {
            List<Commodity> commodityList = new List<Commodity>();

            string sqlQuery = "SELECT * FROM Commodities";
            sqlQuery += ((whereClause != null && whereClause.Length > 0) ? " WHERE " + whereClause : "");

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                OleDbDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Commodity newComm = new Commodity();
                    newComm.Load(reader);
                    commodityList.Add(newComm);
                }
            }
            catch (Exception e)
            {
            }

            return commodityList;
        }

        public bool InsertCommodity(ref Commodity comm)
        {
            bool retVal = false;

            string sqlQuery = "INSERT INTO Commodities (";
            StringBuilder sbColDefs = new StringBuilder();
            StringBuilder sbColValues = new StringBuilder();
            foreach (ColumnDefinition columnDef in comm.ColumnDefs())
            {
                if (!columnDef.ColumnName.Equals("InventoryID"))
                {
                    if (sbColDefs.Length > 0)
                    {
                        sbColDefs.Append(", ");
                        sbColValues.Append(", ");
                    }
                    sbColDefs.Append(columnDef.ColumnName);
                    sbColValues.Append(comm.GetInsertableValue(columnDef));
                }
            }
            sqlQuery += sbColDefs.ToString() + ") VALUES (" + sbColValues.ToString() + ")";

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                int result = sqlCommand.ExecuteNonQuery();
                if (result == 1)
                {
                    OleDbCommand identityCommand = new OleDbCommand("SELECT @@IDENTITY", _accessConnection);
                    int newMaxNumKey = (int)identityCommand.ExecuteScalar();

                    comm.InventoryID = newMaxNumKey;
                    retVal = true;
                }
            }
            catch (Exception e)
            {
                //TODO: how to handle this?
            }

            return retVal;
        }

        public int UpdateCommodity(Commodity comm)
        {
            int result = 0;

            string sqlQuery = "UPDATE Commodities SET ";
            StringBuilder sbColValues = new StringBuilder();
            StringBuilder sbWhereClause = new StringBuilder();
            foreach (KeyValuePair<int, object> columnValue in comm.GetColumnValues())
            {
                ColumnDefinition columnDef = null;
                foreach (ColumnDefinition colDef in comm.ColumnDefs())
                {
                    if (colDef.ColumnIndex == columnValue.Key)
                    {
                        columnDef = colDef;
                        break;
                    }
                }
                if (columnDef == null)
                {
                    //TODO: LOG THIS!
                    return result;
                }

                if (!columnDef.ColumnName.Equals("InventoryID"))
                {
                    if (sbColValues.Length > 0)
                    {
                        sbColValues.Append(", ");
                    }
                    sbColValues.Append(columnDef.ColumnName + " = " + comm.GetInsertableValue(columnDef));
                }
                else
                {
                    sbWhereClause.Append(" WHERE " + columnDef.ColumnName + " = " + comm.GetInsertableValue(columnDef));
                }
            }

            if (sbColValues.Length > 0 && sbWhereClause.Length > 0)
            {
                try
                {
                    sqlQuery += sbColValues.ToString() + sbWhereClause.ToString();
                    OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                    result = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //TODO: how to handle this?
                }
            }

            return result;
        }

        public int DeleteCommodity(Commodity comm)
        {
            int result = 0;

            string sqlQuery = "DELETE FROM Commodities WHERE InventoryID = " + comm.InventoryID;
            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                result = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //TODO: how to handle this?
            }

            return result;
        }

        #endregion Commodities

        #region Commodity Types

        public List<CommodityType> LoadCommodityTypes(string whereClause)
        {
            List<CommodityType> commodityTypeList = new List<CommodityType>();

            string sqlQuery = "SELECT * FROM " + CommodityType._tableName;
            sqlQuery += ((whereClause != null && whereClause.Length > 0) ? " WHERE " + whereClause : "");

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                OleDbDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    CommodityType newComm = new CommodityType();
                    newComm.Load(reader);
                    commodityTypeList.Add(newComm);
                }
            }
            catch (Exception e)
            {
            }

            return commodityTypeList;
        }

        public bool InsertCommodityType(ref CommodityType comm)
        {
            bool retVal = false;

            /* TODO:THIS
            string sqlQuery = "INSERT INTO Commodities (";
            StringBuilder sbColDefs = new StringBuilder();
            StringBuilder sbColValues = new StringBuilder();
            foreach (ColumnDefinition columnDef in comm.ColumnDefs())
            {
                if (!columnDef.ColumnName.Equals("InventoryID"))
                {
                    if (sbColDefs.Length > 0)
                    {
                        sbColDefs.Append(", ");
                        sbColValues.Append(", ");
                    }
                    sbColDefs.Append(columnDef.ColumnName);
                    sbColValues.Append(comm.GetInsertableValue(columnDef));
                }
            }
            sqlQuery += sbColDefs.ToString() + ") VALUES (" + sbColValues.ToString() + ")";

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                int result = sqlCommand.ExecuteNonQuery();
                if (result == 1)
                {
                    OleDbCommand identityCommand = new OleDbCommand("SELECT @@IDENTITY", _accessConnection);
                    int newMaxNumKey = (int)identityCommand.ExecuteScalar();

                    comm.InventoryID = newMaxNumKey;
                    retVal = true;
                }
            }
            catch (Exception e)
            {
                //TODO: how to handle this?
            }
            */
            return retVal;
        }

        public int UpdateCommodityType(CommodityType comm)
        {
            int result = 0;
            /* TODO:THIS
            string sqlQuery = "UPDATE Commodities SET ";
            StringBuilder sbColValues = new StringBuilder();
            StringBuilder sbWhereClause = new StringBuilder();
            foreach (KeyValuePair<int, object> columnValue in comm.GetColumnValues())
            {
                ColumnDefinition columnDef = null;
                foreach (ColumnDefinition colDef in comm.ColumnDefs())
                {
                    if (colDef.ColumnIndex == columnValue.Key)
                    {
                        columnDef = colDef;
                        break;
                    }
                }
                if (columnDef == null)
                {
                    //TODO: LOG THIS!
                    return result;
                }

                if (!columnDef.ColumnName.Equals("InventoryID"))
                {
                    if (sbColValues.Length > 0)
                    {
                        sbColValues.Append(", ");
                    }
                    sbColValues.Append(columnDef.ColumnName + " = " + comm.GetInsertableValue(columnDef));
                }
                else
                {
                    sbWhereClause.Append(" WHERE " + columnDef.ColumnName + " = " + comm.GetInsertableValue(columnDef));
                }
            }

            if (sbColValues.Length > 0 && sbWhereClause.Length > 0)
            {
                try
                {
                    sqlQuery += sbColValues.ToString() + sbWhereClause.ToString();
                    OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                    result = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    //TODO: how to handle this?
                }
            }
            */
            return result;
        }

        public int DeleteCommodityType(CommodityType comm)
        {
            int result = 0;

            /* TODO:this
            string sqlQuery = "DELETE FROM Commodities WHERE InventoryID = " + comm.InventoryID;
            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                result = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //TODO: how to handle this?
            }
            */

            return result;
        }

        #endregion Commodity Types

        #region Work Orders

        public bool TryLoadWorkOrderByJobID(string jobID, out WorkOrder workOrder)
        {
            bool bSuccess = false;
            workOrder = null;

            //TODO: prone to SQL injection
            string sqlQuery = string.Format("SELECT * FROM WorkOrders WHERE JobID = '{0}'", jobID);

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                OleDbDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    workOrder = new WorkOrder();
                    workOrder.Load(reader);
                    bSuccess = true;
                }
            }
            catch (Exception e)
            {
            }

            return bSuccess;
        }

        public bool TryGetDataReaderForCommoditiesForWorkOrder(int workOrderNum, out IDataReader dataReader)
        {
            const string sqlQuery = "SELECT Commodities.PartNumber as PartNumber, Commodities.PartDescription as PartDescription, " +
                "WorkOrderXInventory.Quantity as Quantity, UnitOfMeasure.UnitOfMeasureTag as UM " +
                "FROM Commodities, WorkOrderXInventory, UnitOfMeasure " +
                "WHERE WorkOrderXInventory.InventoryID = Commodities.InventoryID " +
                "AND Commodities.UnitOfMeasureID = UnitOfMeasure.UnitOfMeasureID " +
                "AND WorkOrderID = ";

            string query = sqlQuery + workOrderNum.ToString();

            return TryGetDataReader(query, out dataReader);
        }

        #endregion Work Orders

        #region Means Of Payment

        public List<MeansOfPayment> LoadMeansOfPayments(string whereClause)
        {
            List<MeansOfPayment> mopList = new List<MeansOfPayment>();

            string sqlQuery = "SELECT * FROM " + MeansOfPayment._tableName;
            sqlQuery += ((whereClause != null && whereClause.Length > 0) ? " WHERE " + whereClause : "");

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                OleDbDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MeansOfPayment newMop = new MeansOfPayment();
                    newMop.Load(reader);
                    mopList.Add(newMop);
                }
            }
            catch (Exception e)
            {
            }

            return mopList;
        }

        #endregion MOP

        #region UM

        public List<UnitOfMeasure> LoadUnitOfMeasures(string whereClause)
        {
            List<UnitOfMeasure> umList = new List<UnitOfMeasure>();

            string sqlQuery = "SELECT * FROM " + UnitOfMeasure._tableName;
            sqlQuery += ((whereClause != null && whereClause.Length > 0) ? " WHERE " + whereClause : "");

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                OleDbDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    UnitOfMeasure newUm = new UnitOfMeasure();
                    newUm.Load(reader);
                    umList.Add(newUm);
                }
            }
            catch (Exception e)
            {
            }

            return umList;
        }

        #endregion UM

        #region General Methods

        public bool TryGetDataReader(string sqlQuery, out IDataReader dataReader)
        {
            dataReader = null;

            try
            {
                OleDbCommand sqlCommand = new OleDbCommand(sqlQuery, _accessConnection);
                dataReader = sqlCommand.ExecuteReader();
            }
            catch (Exception e)
            {
            }

            return (dataReader != null);
        }

        #endregion General Methods

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_accessConnection != null)
                    {
                        _accessConnection.Close();
                    }
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion IDisposable
    }
}