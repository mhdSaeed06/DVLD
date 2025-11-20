using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDBusinessLayer;
using DVLDDataAccessLayer;


namespace DVLDBusinessLayer
{
    public class clsPerson
    {

            public enum enMode { AddNew = 0, Update = 1 };
            public enMode Mode = enMode.AddNew;


        public int PersonId { get; set; }

        public string NationalNumber { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public enum enGender { Male = 0, Female = 1 };

        public enGender gender { get; set; }

        public string Address { get; set; }

        public int NationalityCountryId { get; set; }

        public string ImagePath { get; set; }


        public static DataTable GetAllPeopleForList()
        {
            return clsPersonDataAccess.GetAllPeopleForList();
        }

        public static DataTable GetAllPeopleInfo()
        {
            return clsPersonDataAccess.GetAllPeopleInfo();
        }

        public clsPerson()
        {
            this.PersonId = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.Email = "";
            this.NationalNumber = "";
            this.Phone = "";
            this.Address = "";
            this.gender = enGender.Male;
            this.NationalityCountryId = 0;
            this.DateOfBirth = DateTime.Now;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }


        private clsPerson(int PersonId, string FirstName, string SecondName, 
            string ThirdName, string LastName,string NationalNo,
            string Email, string Phone, string Address, 
            DateTime DateOfBirth, int CountryID, string ImagePath, byte Gender)
        {
            this.PersonId = PersonId;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.NationalNumber = NationalNo;
            this.LastName = LastName;
            this.Email = Email;
            this.Address = Address;
            this.Phone = Phone;
            this.DateOfBirth = DateOfBirth;
            this.NationalityCountryId = CountryID;
            this.ImagePath = ImagePath;
            this.gender = (enGender)Gender;

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonId = clsPersonDataAccess.AddNewPerson(this.FirstName, this.SecondName,this.ThirdName,
                this.LastName,this.NationalNumber, this.Email, this.Phone, this.Address, this.DateOfBirth, this.NationalityCountryId, this.ImagePath, (byte)this.gender);


            return (this.PersonId != -1);

        }

        public static clsPerson Find(int PersonId)
        {
            string FirstName = "", NationalNo = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;
            byte Gendor = 0;

            if (clsPersonDataAccess.GetPersonInfoById(PersonId, ref FirstName,ref NationalNo, ref SecondName
                , ref ThirdName, ref LastName, ref Email, ref Phone, ref Address
                , ref DateOfBirth, ref CountryID, ref ImagePath, ref Gendor))
            {
                return new clsPerson(PersonId, FirstName, SecondName, ThirdName, LastName,NationalNo, Email, Phone
                    , Address, DateOfBirth, CountryID, ImagePath, Gendor);
            }

            else
                return null;


        }

        private bool _UpdatePersonInfo()
        {

            return clsPersonDataAccess.UpdatePerson(this.PersonId, this.FirstName, this.SecondName,
                this.ThirdName,this.LastName, this.NationalNumber, this.Email, this.Phone, this.Address,
                this.DateOfBirth, this.NationalityCountryId, this.ImagePath,(byte)this.gender);

        }

        public static bool DeletePerson(int PersonId)
        {
            return clsPersonDataAccess.DeletePerson(PersonId);
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonDataAccess.IsPersonExist(PersonID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewPerson())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;

                    }
                case enMode.Update:
                    {
                        return this._UpdatePersonInfo();
                    }
                default: 
                    return false;
            }

        }
        

        static public bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            DataTable dtPeople = GetAllPeopleForList();
             
            DataView dvPeople = dtPeople.DefaultView;

            string dataColumn = "NationalNo";
            string searchValue = NationalNo;

            dvPeople.RowFilter = string.Format("[{0}] = '{1}'",dataColumn,searchValue);

            return (dvPeople.Count > 0);
        }

        public static clsPerson Find(DataTable dtPeople, string columnName, object value)
        {
            string filterExpression;

            if (value is string)
                filterExpression = $"{columnName} = '{value}'";
            else
                filterExpression = $"{columnName} = {value}";

            try
            {
                DataRow[] rows = dtPeople.Select(filterExpression);
                if (rows.Length > 0)
                    return new clsPerson(rows[0]);
                //return CreateFromDataRow(rows[0]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            

        }

        private clsPerson(DataRow row)
        {

            this.PersonId = GetValueOrDefault(row, "PersonID", -1);
            this.FirstName = GetValueOrDefault(row, "FirstName", "");
            this.SecondName = GetValueOrDefault(row, "SecondName", "");
            this.ThirdName = GetValueOrDefault(row, "ThirdName", "");
            this.LastName = GetValueOrDefault(row, "LastName", "");
            this.NationalNumber = GetValueOrDefault(row, "NationalNo", "");
            this.Email = GetValueOrDefault(row, "Email", "");
            this.Phone = GetValueOrDefault(row, "Phone", "");
            this.Address = GetValueOrDefault(row, "Address", "");
            this.DateOfBirth = GetValueOrDefault(row, "DateOfBirth", DateTime.Now);
            this.NationalityCountryId = GetValueOrDefault(row, "NationalityCountryId", 0);
            this.ImagePath = GetValueOrDefault(row, "ImagePath", "");
            this.gender = (enGender)GetValueOrDefault(row, "Gender", (byte)0);

            this.Mode = enMode.Update;
        }

        private T GetValueOrDefault<T>(DataRow row,string columnName,T defaultValue = default)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                return (T)Convert.ChangeType(row[columnName], typeof(T));
            else
                return defaultValue;

        }

        //private static clsPerson CreateFromDataRow(DataRow row)
        //{
        //    if (row == null)
        //        return null;

        //    return new clsPerson
        //    {
        //        PersonId = row["PersonId"] != DBNull.Value ? (int)row["PersonId"] : -1,
        //        FirstName = row["FirstName"]?.ToString() ?? "",
        //        SecondName = row["SecondName"]?.ToString() ?? "",
        //        ThirdName = row["ThirdName"]?.ToString() ?? "",
        //        LastName = row["LastName"]?.ToString() ?? "",
        //        NationalNumber = row["NationalNo"]?.ToString() ?? "",
        //        Email = row["Email"]?.ToString() ?? "",
        //        Phone = row["Phone"]?.ToString() ?? "",
        //        Address = row["Address"]?.ToString() ?? "",
        //        DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime)row["DateOfBirth"] : DateTime.Now,
        //        NationalityCountryId = row["NationalityCountryId"] != DBNull.Value ? (int)row["NationalityCountryId"] : 0,
        //        ImagePath = row["ImagePath"]?.ToString() ?? "",
        //        gender = row["Gendor"] != DBNull.Value ? (enGender)(byte)row["Gendor"] : enGender.Male,

        //        Mode = enMode.Update
        //    };
        //}


    }

}
