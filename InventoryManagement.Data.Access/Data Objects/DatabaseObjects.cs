using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace InventoryManagement.Data.Web
{
    public class ColumnDefinition
    {
        public enum FieldType
        {
            Int32 = 0,
            String = 1,
            Boolean = 2,
            Currency = 3,
            DateTime = 4
        }

        public FieldType ColumnFieldType { get; private set; }
        public string ColumnName { get; private set; }
        public int ColumnIndex { get; private set; }

        public int ColumnLength { get; set; }
        public bool MaxNumKey { get; set; }

        public ColumnDefinition(int index, string columnName, FieldType ft)
        {
            ColumnIndex = index;
            ColumnName = columnName;
            ColumnFieldType = ft;
            MaxNumKey = false;
        }

        public ColumnDefinition(int index, string columnName, FieldType ft, int columnLength)
            : this(index, columnName, ft)
        {
            ColumnLength = columnLength;
        }

        public ColumnDefinition(int index, string columnName, FieldType ft, bool maxNumKey)
            : this(index, columnName, ft)
        {
            MaxNumKey = maxNumKey;
        }
    }

    public abstract class DataObject
    {
        public abstract IEnumerable<ColumnDefinition> ColumnDefs();
        public abstract string TableName();

        protected Dictionary<int, object> _columnValues = new Dictionary<int, object>();
        public bool TryGetValue<T>(int columnId, out T val)
        {
            object valAsObj;
            bool retVal = _columnValues.TryGetValue(columnId, out valAsObj);
            val = (T)valAsObj;
            return retVal;
        }
        public object GetValue(int columnId)
        {
            return _columnValues[columnId];
        }
        public Dictionary<int, object> GetColumnValues()
        {
            return _columnValues;
        }

        public string GetInsertableValue(ColumnDefinition columnDef)
        {
            string valueString = "";
            object columnValue = _columnValues[columnDef.ColumnIndex];
            switch (columnDef.ColumnFieldType)
            {
                case ColumnDefinition.FieldType.Boolean:
                    if ((bool)columnValue == true)
                    {
                        valueString = "TRUE";
                    }
                    else
                    {
                        valueString = "FALSE";
                    }
                    break;
                case ColumnDefinition.FieldType.String:
                    valueString = "\"" + columnValue.ToString() + "\"";
                    break;
                case ColumnDefinition.FieldType.DateTime:
                    DateTime dt = (DateTime)columnValue;
                    valueString = dt.ToShortDateString() + " " + dt.ToLongTimeString();
                    break;
                case ColumnDefinition.FieldType.Int32:
                case ColumnDefinition.FieldType.Currency:
                default:
                    valueString = columnValue.ToString();
                    break;
            }

            return valueString;
        }

        public void SetValue<T>(int columnId, T val)
        {
            _columnValues[columnId] = val;
        }

        /*
        public void Load(OleDbDataReader reader)
        {
            try
            {
                foreach (ColumnDefinition columnDef in ColumnDefs())
                {
                    if (reader.GetValue(columnDef.ColumnIndex) != DBNull.Value)
                    {
                        switch (columnDef.ColumnFieldType)
                        {
                            case ColumnDefinition.FieldType.String:
                                _columnValues[columnDef.ColumnIndex] = reader.GetString(columnDef.ColumnIndex);
                                break;
                            case ColumnDefinition.FieldType.Int32:
                                _columnValues[columnDef.ColumnIndex] = reader.GetInt32(columnDef.ColumnIndex);
                                break;
                            case ColumnDefinition.FieldType.Boolean:
                                _columnValues[columnDef.ColumnIndex] = reader.GetBoolean(columnDef.ColumnIndex);
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
         */
    }

    [DataContract]
    public class Commodity : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
        {
            new ColumnDefinition(0, "InventoryID", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(1, "PartNumber", ColumnDefinition.FieldType.String, 255),
            new ColumnDefinition(2, "InventoryType", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(3, "PartDescription", ColumnDefinition.FieldType.String, 255),
            new ColumnDefinition(4, "UnitOfMeasureID", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(5, "ReorderLevel", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(6, "Vendor", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(7, "Discontinued", ColumnDefinition.FieldType.Boolean)
        };

        static string _tableName = "Commodities";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties

        [Key]
        [DataMember]
        public int InventoryID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [DataMember]
        public string PartNumber
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }
        [DataMember]
        public int InventoryType
        {
            get
            {
                return (int)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }
        [DataMember]
        public string PartDescription
        {
            get
            {
                return (string)_columnValues[3];
            }
            set
            {
                _columnValues[3] = value;
            }
        }
        [DataMember]
        public int UnitOfMeasureID
        {
            get
            {
                return (int)_columnValues[4];
            }
            set
            {
                _columnValues[4] = value;
            }
        }
        [DataMember]
        public int ReorderLevel
        {
            get
            {
                return (int)_columnValues[5];
            }
            set
            {
                _columnValues[5] = value;
            }
        }
        [DataMember]
        public int Vendor
        {
            get
            {
                return (int)_columnValues[6];
            }
            set
            {
                _columnValues[6] = value;
            }
        }
        [DataMember]
        public bool Discontinued
        {
            get
            {
                return (bool)_columnValues[7];
            }
            set
            {
                _columnValues[7] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class CommodityType : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
        {
            new ColumnDefinition(0, "TypeID", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(1, "TypeCode", ColumnDefinition.FieldType.String, 255),
            new ColumnDefinition(2, "Description", ColumnDefinition.FieldType.String, 255)
        };

        internal static string _tableName = "CommodityType";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties

        [Key]
        [DataMember]
        public int TypeID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }

        [DataMember]
        public string TypeCode
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return (string)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class Vendor : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
        {
             new ColumnDefinition(0, "VendorID", ColumnDefinition.FieldType.Int32),
             new ColumnDefinition(1, "VendorName", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(2, "Address", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(3, "City", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(4, "State", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(5, "Zip", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(6, "Email", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(7, "PhoneNumber", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(7, "ContactPerson", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(7, "MeansOfPayment", ColumnDefinition.FieldType.Int32)
        };

        static string _tableName = "Vendors";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties
        [Key]
        [DataMember]
        public int VendorID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [DataMember]
        public string VendorName
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }
        [DataMember]
        public string Address
        {
            get
            {
                return (string)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }
        [DataMember]
        public string City
        {
            get
            {
                return (string)_columnValues[3];
            }
            set
            {
                _columnValues[3] = value;
            }
        }
        [DataMember]
        public string State
        {
            get
            {
                return (string)_columnValues[4];
            }
            set
            {
                _columnValues[4] = value;
            }
        }
        [DataMember]
        public string Zip
        {
            get
            {
                return (string)_columnValues[5];
            }
            set
            {
                _columnValues[5] = value;
            }
        }
        [DataMember]
        public string Email
        {
            get
            {
                return (string)_columnValues[6];
            }
            set
            {
                _columnValues[6] = value;
            }
        }
        [DataMember]
        public string PhoneNumber
        {
            get
            {
                return (string)_columnValues[7];
            }
            set
            {
                _columnValues[7] = value;
            }
        }
        [DataMember]
        public string ContactPerson
        {
            get
            {
                return (string)_columnValues[8];
            }
            set
            {
                _columnValues[8] = value;
            }
        }
        [DataMember]
        public int MeansOfPayment
        {
            get
            {
                return (int)_columnValues[9];
            }
            set
            {
                _columnValues[9] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class UnitOfMeasure : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
        {
            new ColumnDefinition(0, "UnitOfMeasureID", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(1, "UnitOfMeasureTag", ColumnDefinition.FieldType.String, 20),
            new ColumnDefinition(2, "Description", ColumnDefinition.FieldType.String, 255)
        };

        internal static string _tableName = "UnitOfMeasure";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties

        [Key]
        [DataMember]
        public int UnitOfMeasureID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }

        [DataMember]
        public string UnitOfMeasureTag
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return (string)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class MeansOfPayment : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
        {
            new ColumnDefinition(0, "PaymentMethodID", ColumnDefinition.FieldType.Int32),
            new ColumnDefinition(1, "PaymentType", ColumnDefinition.FieldType.String, 255),
        };

        internal static string _tableName = "MeansOfPayment";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties
        [Key]
        [DataMember]
        public int PaymentMethodID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [DataMember]
        public string PaymentType
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class User : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
        {
             new ColumnDefinition(0, "UserID", ColumnDefinition.FieldType.Int32),
             new ColumnDefinition(1, "UserName", ColumnDefinition.FieldType.String, 100),
             new ColumnDefinition(2, "FirstName", ColumnDefinition.FieldType.String, 100),
             new ColumnDefinition(3, "LastName", ColumnDefinition.FieldType.String, 100),
             new ColumnDefinition(4, "Email", ColumnDefinition.FieldType.String, 200),
             new ColumnDefinition(5, "PasswordHash", ColumnDefinition.FieldType.String, 200),
             new ColumnDefinition(6, "PasswordSalt", ColumnDefinition.FieldType.String, 20),
             new ColumnDefinition(7, "PasswordQuestion", ColumnDefinition.FieldType.String, 255),
             new ColumnDefinition(8, "PasswordAnswerHash", ColumnDefinition.FieldType.String, 200),
             new ColumnDefinition(9, "PasswordAnswerSalt", ColumnDefinition.FieldType.String, 20),
             new ColumnDefinition(10, "UserType", ColumnDefinition.FieldType.Int32),
             new ColumnDefinition(11, "ProfileReset", ColumnDefinition.FieldType.Boolean)
        };

        static string _tableName = "Users";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties
        [Key]
        [DataMember]
        public int UserID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [DataMember]
        public string UserName
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }
        [DataMember]
        public string FirstName
        {
            get
            {
                return (string)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }
        [DataMember]
        public string LastName
        {
            get
            {
                return (string)_columnValues[3];
            }
            set
            {
                _columnValues[3] = value;
            }
        }
        [DataMember]
        public string Email
        {
            get
            {
                return (string)_columnValues[4];
            }
            set
            {
                _columnValues[4] = value;
            }
        }
        [DataMember]
        public string PasswordHash
        {
            get
            {
                return (string)_columnValues[5];
            }
            set
            {
                _columnValues[5] = value;
            }
        }
        [DataMember]
        public string PasswordSalt
        {
            get
            {
                return (string)_columnValues[6];
            }
            set
            {
                _columnValues[6] = value;
            }
        }
        [DataMember]
        public string PasswordQuestion
        {
            get
            {
                return (string)_columnValues[7];
            }
            set
            {
                _columnValues[7] = value;
            }
        }
        [DataMember]
        public string PasswordAnswerHash
        {
            get
            {
                return (string)_columnValues[8];
            }
            set
            {
                _columnValues[8] = value;
            }
        }
        [DataMember]
        public string PasswordAnswerSalt
        {
            get
            {
                return (string)_columnValues[9];
            }
            set
            {
                _columnValues[9] = value;
            }
        }

        public enum UserTypeEnum
        {
            Admin = 1,
            User = 0
        }

        public UserTypeEnum UserTypeAsEnum
        {
            get
            {
                return (UserTypeEnum)UserType;
            }
        }

        public int UserType
        {
            get
            {
                return (int)_columnValues[10];
            }
            set
            {
                _columnValues[10] = value;
            }
        }

        public bool ProfileReset
        {
            get
            {
                return (bool)_columnValues[11];
            }
            set
            {
                _columnValues[11] = value;
            }
        }

        #endregion Properties

        #region Extras

        /// <summary>
        /// Returns each user in the format of "user (FirstName LastName)"
        /// </summary>
        public string DisplayName
        {
            get { return UserName == null ? " " : UserName + " (" + FirstName + " " + LastName + ")"; }
        }

        #endregion Extras
    }
    
    [DataContract]
    public class WorkOrder : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
     {
         new ColumnDefinition(0, "WorkOrderID", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(1, "JobID", ColumnDefinition.FieldType.String, 255),
         new ColumnDefinition(2, "CustomerName", ColumnDefinition.FieldType.String, 255),
         new ColumnDefinition(3, "Address", ColumnDefinition.FieldType.String, 255),
         new ColumnDefinition(4, "JobStatus", ColumnDefinition.FieldType.Int32)
     };

        static string _tableName = "WorkOrders";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties
        [Key]
        [DataMember]
        public int WorkOrderID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [DataMember]
        public string JobID
        {
            get
            {
                return (string)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }
        [DataMember]
        public string CustomerName
        {
            get
            {
                return (string)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }
        [DataMember]
        public string Address
        {
            get
            {
                return (string)_columnValues[3];
            }
            set
            {
                _columnValues[3] = value;
            }
        }
        [DataMember]
        public int JobStatus
        {
            get
            {
                return (int)_columnValues[4];
            }
            set
            {
                _columnValues[4] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class Inventory : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
     {
         new ColumnDefinition(0, "InventoryID", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(1, "CribLocation", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(2, "Quantity", ColumnDefinition.FieldType.Int32)
     };

        static string _tableName = "Inventory";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties
        [Key]
        [DataMember]
        public int InventoryID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [Key]
        [DataMember]
        public int CribLocation
        {
            get
            {
                return (int)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }
        [DataMember]
        public int Quantity
        {
            get
            {
                return (int)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }

        #endregion Properties
    }

    [DataContract]
    public class PurchaseHistory : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
     {
         new ColumnDefinition(0, "HistoryID", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(1, "InventoryID", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(2, "PurchaseDate", ColumnDefinition.FieldType.DateTime),
         new ColumnDefinition(3, "PurchaseAmount", ColumnDefinition.FieldType.Currency),
         new ColumnDefinition(3, "VendorID", ColumnDefinition.FieldType.Int32)
     };

        static string _tableName = "PurchaseHistory";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties
        [Key]
        [DataMember]
        public int HistoryID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }
        [DataMember]
        public int InventoryID
        {
            get
            {
                return (int)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }
        [DataMember]
        public DateTime PurchaseDate
        {
            get
            {
                return (DateTime)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }
        [DataMember]
        public float PurchaseAmount
        {
            get
            {
                return (float)_columnValues[3];
            }
            set
            {
                _columnValues[3] = value;
            }
        }
        [DataMember]
        public int VendorID
        {
            get
            {
                return (int)_columnValues[4];
            }
            set
            {
                _columnValues[4] = value;
            }
        }

        #endregion Properties
    }

    public class WorkOrderXInventory : DataObject
    {
        #region Table Definition

        static List<ColumnDefinition> _columnDefs = new List<ColumnDefinition>()
     {
         new ColumnDefinition(0, "WorkOrderID", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(1, "InventoryID", ColumnDefinition.FieldType.Int32),
         new ColumnDefinition(2, "Quantity", ColumnDefinition.FieldType.Int32)
     };

        static string _tableName = "WorkOrderXInventory";

        public override IEnumerable<ColumnDefinition> ColumnDefs()
        {
            return _columnDefs;
        }

        public override string TableName()
        {
            return _tableName;
        }

        #endregion Table Definition

        #region Properties

        public int WorkOrderID
        {
            get
            {
                return (int)_columnValues[0];
            }
            set
            {
                _columnValues[0] = value;
            }
        }

        public int InventoryID
        {
            get
            {
                return (int)_columnValues[1];
            }
            set
            {
                _columnValues[1] = value;
            }
        }

        public int Quantity
        {
            get
            {
                return (int)_columnValues[2];
            }
            set
            {
                _columnValues[2] = value;
            }
        }

        #endregion Properties
    }
}