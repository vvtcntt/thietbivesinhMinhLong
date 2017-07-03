using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TOTO.Models;
namespace TOTO.Models
{
    public class Updatehistoty
    {
        public TOTOContext db = new TOTOContext();
        public static void UpdateHistory(string task,string FullName,string UserID)
        {

            TOTOContext db = new TOTOContext();
            tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
            tblhistorylogin.FullName = FullName;
            tblhistorylogin.Task = task;
            tblhistorylogin.idUser = int.Parse(UserID);
            tblhistorylogin.DateCreate = DateTime.Now;
            tblhistorylogin.Active = true;
            
            db.tblHistoryLogins.Add(tblhistorylogin);
            db.SaveChanges();
           
        }
    }
}