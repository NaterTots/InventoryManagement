using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace InventoryManagement.Data.Web
{
    public class DataObject
    {

    }

    [DataContract]
    public class Commodity : DataObject
    {
        [Key]
        [DataMember]
        public virtual int InventoryID { get; set; }
        [DataMember]
        public virtual string PartNumber { get; set; }
        [DataMember]
        public virtual int InventoryType { get; set; }
        [DataMember]
        public virtual string PartDescription { get; set; }
        [DataMember]
        public virtual int UnitOfMeasureID { get; set; }
        [DataMember]
        public virtual int ReorderLevel { get; set; }
        [DataMember]
        public virtual int Vendor { get; set; }
        [DataMember]
        public virtual bool Discontinued { get; set; }
    }

    [DataContract]
    public class CommodityType : DataObject
    {
        #region Properties

        [Key]
        [DataMember]
        public virtual int TypeID { get; set; }
        [DataMember]
        public virtual string TypeCode { get; set; }
        [DataMember]
        public virtual string Description { get; set; }

        #endregion Properties
    }

    [DataContract]
    public class Vendor : DataObject
    {
        #region Properties
        [Key]
        [DataMember]
        public virtual int VendorID { get; set; }
        [DataMember]
        public virtual string VendorName { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual string City { get; set; }
        [DataMember]
        public virtual string State { get; set; }
        [DataMember]
        public virtual string Zip { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string PhoneNumber { get; set; }
        [DataMember]
        public virtual string ContactPerson { get; set; }
        [DataMember]
        public virtual int MeansOfPayment { get; set; }

        #endregion Properties
    }

    [DataContract]
    public class UnitOfMeasure : DataObject
    {
        #region Properties

        [Key]
        [DataMember]
        public virtual int UnitOfMeasureID { get; set; }

        [DataMember]
        public virtual string UnitOfMeasureTag { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        #endregion Properties
    }

    [DataContract]
    public class MeansOfPayment : DataObject
    {
        #region Properties
        [Key]
        [DataMember]
        public virtual int PaymentMethodID { get; set; }
        [DataMember]
        public virtual string PaymentType { get; set; }

        #endregion Properties
    }

    [DataContract]
    public class User : DataObject
    {
        #region Properties
        [Key]
        [DataMember]
        public virtual int UserID { get; set; }
        [DataMember]
        public virtual string UserName { get; set; }
        [DataMember]
        public virtual string FirstName { get; set; }
        [DataMember]
        public virtual string LastName { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string PasswordHash { get; set; }
        [DataMember]
        public virtual string PasswordSalt { get; set; }
        [DataMember]
        public virtual string PasswordQuestion { get; set; }
        [DataMember]
        public virtual string PasswordAnswerHash { get; set; }
        [DataMember]
        public virtual string PasswordAnswerSalt { get; set; }

        public enum UserTypeEnum
        {
            Admin = 1,
            User = 0
        }

        [DataMember]
        public virtual UserTypeEnum UserTypeAsEnum
        {
            get
            {
                return (UserTypeEnum)UserType;
            }
        }

        [DataMember]
        public virtual int UserType { get; set; }
        [DataMember]
        public virtual bool ProfileReset { get; set; }

        #endregion Properties

        #region Extras

        /// <summary>
        /// Returns each user in the format of "user (FirstName LastName)"
        /// </summary>
        [DataMember]
        public virtual string DisplayName
        {
            get { return UserName == null ? " " : UserName + " (" + FirstName + " " + LastName + ")"; }
        }

        #endregion Extras
    }

    [DataContract]
    public class WorkOrder : DataObject
    {
        #region Properties
        [Key]
        [DataMember]
        public virtual int WorkOrderID { get; set; }
        [DataMember]
        public virtual string JobID { get; set; }
        [DataMember]
        public virtual string CustomerName { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual int JobStatus { get; set; }

        private List<Commodity> commodityList = new List<Commodity>();
        public virtual List<Commodity> Commodities
        {
            get { return commodityList; }
            set { commodityList = value; }
        }

        #endregion Properties

        #region Extras

        public enum WorkOrderStatus
        {
            Open = 0,
            Closed = 1
        }

        #endregion Extras
    }

    [DataContract]
    public class Inventory : DataObject
    {
        #region Properties
        [Key]
        [DataMember]
        public virtual int InventoryID { get; set; }
        [Key]
        [DataMember]
        public virtual int CribLocation { get; set; }
        [DataMember]
        public virtual int Quantity { get; set; }

        #endregion Properties
    }

    [DataContract]
    public class PurchaseHistory : DataObject
    {
        #region Properties
        [Key]
        [DataMember]
        public virtual int HistoryID { get; set; }
        [DataMember]
        public virtual int InventoryID { get; set; }
        [DataMember]
        public virtual DateTime PurchaseDate { get; set; }
        [DataMember]
        public virtual float PurchaseAmount { get; set; }
        [DataMember]
        public virtual int VendorID { get; set; }

        #endregion Properties
    }

    [DataContract]
    public class WorkOrderXInventory : DataObject
    {
        #region Properties
        [DataMember]
        public virtual int WorkOrderID { get; set; }
        [DataMember]
        public virtual int InventoryID { get; set; }
        [DataMember]
        public virtual int Quantity { get; set; }

        #endregion Properties
    }
}