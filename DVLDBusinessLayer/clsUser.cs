using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsUser
    {
        public int UserId { get; set; }

        public int PersonId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive {  get; set; }


        clsPerson.enMode Mode = clsPerson.enMode.AddNew;


        public static DataTable GetAllUsers()
        {
            return clsUserDataAccess.GetAllUsers();
        }

        public static DataTable GetAllUsersInfo()
        {
            return clsUserDataAccess.GetAllUsersInfo();
        }


        public clsUser()
        {
            this.UserId = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
            this.PersonId = -1;
            
            Mode = clsPerson.enMode.AddNew;
        }


        private clsUser(int UserId,string UserName, int PersonId, string Password
             , bool IsActive)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.PersonId = PersonId;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = clsPerson.enMode.Update;

        }

        private bool _AddNewUser()
        {
            this.UserId = clsUserDataAccess.AddNewUser(this.UserName,this.PersonId
                ,this.Password,this.IsActive);


            return (this.UserId != -1);

        }

        public static clsUser Find(int UserId)
        {
            string UserName = "", Password = "";
            int PersonId = -1;
            bool IsActive = false;

            if (clsUserDataAccess.GetUserInfoById(UserId,ref UserName,ref PersonId,ref Password,ref IsActive))
            {
                return new clsUser(UserId,UserName,PersonId,Password,IsActive);
            }

            else
                return null;


        }


        
        private bool _UpdateUserInfo()
        {

            return clsUserDataAccess.UpdateUser(this.UserId,this.UserName
                    ,this.PersonId,this.Password,this.IsActive);

        }

        public static bool DeleteUser(int UserId)
        {
            return clsUserDataAccess.DeleteUser(UserId);
        }

        public static bool IsUserExist(int UserId)
        {
            return clsUserDataAccess.IsUserExist(UserId);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case clsPerson.enMode.AddNew:
                    {
                        if (_AddNewUser())
                        {
                            Mode = clsPerson.enMode.Update;
                            return true;
                        }
                        else
                            return false;

                    }
                case clsPerson.enMode.Update:
                    {
                        return this._UpdateUserInfo();
                    }
                default:
                    return false;
            }

        }


        //static public bool IsUserExist(string NationalNo)
        //{
        //    bool isFound = false;

        //    DataTable dtPeople = GetAllUsers();

        //    DataView dvPeople = dtPeople.DefaultView;

        //    string dataColumn = "NationalNo";
        //    string searchValue = NationalNo;

        //    dvPeople.RowFilter = string.Format("[{0}] = '{1}'", dataColumn, searchValue);

        //    return (dvPeople.Count > 0);
        //}


    }
}
