using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PhotoProspector.Models;

namespace PhotoProspector.Services
{
    public interface IUserService
    {
        bool InsertUserToSQL(string imagepath, Person rperson, string toPath);
        bool DeleteUserFromSQL(string alias, string fromPath);

        bool InsertInvitationCodeSQL(string alias, string code);

        InvitationCodeStatus CheckInvitationCode(string alias, string code);
        List<Person> GetPersonListByDataSet(DataSet myDs);
        DataSet[] GetDataSetsByAlias(string[] str);
        string[] GetDisplayNameByDsarray(DataSet[] dsarray);
    }


    class UserService : IUserService
    {
        private readonly IImageService imageService;

        public UserService(IImageService imageService)
        {
            this.imageService = imageService;
        }

        public bool InsertUserToSQL(string imagepath, Person rperson, string toPath)
        {
#if DEBUG
            return (new Random().Next(100) % 2 == 0);
#endif
            bool result = false;

            string[] temp = imagepath.Split('.');
            string extend = temp[temp.Length - 1];

            string cutname = rperson.alias + "." + extend;
            string cutpath = toPath + cutname;

            rperson.photoPath = ".\\ImageSource\\" + cutname;

            if (rperson.alias == null)
            {
                rperson.alias = "";
            }
            if (rperson.displayname == null)
            {
                rperson.displayname = "";
            }
            if (rperson.title == null)
            {
                rperson.title = "";
            }
            if (rperson.team == null)
            {
                rperson.team = "";
            }
            if (rperson.specialty == null)
            {
                rperson.specialty = "";
            }
            if (rperson.favoritesport == null)
            {
                rperson.favoritesport = "";
            }

            this.imageService.CutImg(imagepath, cutpath, 800, 800, "HW");

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            string query = "select * from FaceWebsiteTable WHERE alias='" + rperson.alias + "'";

            DataSet myDs = new DataSet();
            SqlDataAdapter myDa = new SqlDataAdapter(query, myConn);

            myDa.Fill(myDs);

            if (myDs.Tables[0].Rows.Count > 0)
            {

                result = false;
                return result;
            }
            else
            {
                SqlCommand insert = new SqlCommand("insert into FaceWebsiteTable(displayname, alias, team,title,specialty,favoritesport,photoPath) values(@displayname, @alias, @team, @title, @specialty, @favoritesport, @photoPath)", myConn);
                insert.Parameters.AddWithValue("@displayname", rperson.displayname);
                insert.Parameters.AddWithValue("@alias", rperson.alias);
                insert.Parameters.AddWithValue("@team", rperson.team);
                insert.Parameters.AddWithValue("@title", rperson.title);
                insert.Parameters.AddWithValue("@specialty", rperson.specialty);
                insert.Parameters.AddWithValue("@favoritesport", rperson.favoritesport);
                insert.Parameters.AddWithValue("@photoPath", rperson.photoPath);

                try
                {

                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                finally
                {
                    myConn.Close();
                }
            }
            result = true;
            return result;
        }
        public bool DeleteUserFromSQL(string alias, string fromPath)
        {
#if DEBUG
            return (new Random().Next(100) % 2 == 0);
#endif
            bool result = false;

            string trainimgpath1 = fromPath + alias + ".jpg";
            string trainimgpath2 = fromPath + alias + ".png";

            if (System.IO.File.Exists(trainimgpath1))
            {
                System.IO.File.Delete(trainimgpath1);
            }

            if (System.IO.File.Exists(trainimgpath2))
            {
                System.IO.File.Delete(trainimgpath2);
            }

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            SqlCommand delete = new SqlCommand("Delete from FaceWebsiteTable Where Alias = '" + alias + "'", myConn);

            try
            {
                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConn.Close();
            }

            result = true;
            return result;
        }

        public bool InsertInvitationCodeSQL(string alias, string code)
        {
            bool result = false;

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            string query = "select * from FaceWebsiteTable WHERE alias='" + alias + "'";

            DataSet myDs = new DataSet();
            SqlDataAdapter myDa = new SqlDataAdapter(query, myConn);

            myDa.Fill(myDs);

            if (myDs.Tables[0].Rows.Count > 0)
            {

                SqlCommand delete = new SqlCommand("Delete from InvitationCode Where Alias = '" + alias + "'", myConn);

                try
                {
                    delete.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    myConn.Close();
                    throw ex;
                }
            }

            SqlCommand insert = new SqlCommand("insert into InvitationCode(alias, code) values(@alias, @code)", myConn);
            insert.Parameters.AddWithValue("@alias", alias);
            insert.Parameters.AddWithValue("@code", code);

            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConn.Close();
            }

            result = true;

            return result;
        }

        public InvitationCodeStatus CheckInvitationCode(string alias, string code)
        {
#if DEBUG
            var value = new Random().Next(100) % 3;
            if (value == 0)
                return InvitationCodeStatus.Matched;
            else if (value == 1)
                return InvitationCodeStatus.NotMatch;
            else
                return InvitationCodeStatus.NotRegister;
#endif

            InvitationCodeStatus result = InvitationCodeStatus.Matched;
            string tempcode = "";

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            string query = "select * from InvitationCode WHERE alias='" + alias + "'";

            DataSet myDs = new DataSet();
            SqlDataAdapter myDa = new SqlDataAdapter(query, myConn);

            myDa.Fill(myDs);

            if (myDs.Tables[0].Rows.Count == 0)
            {
                result = InvitationCodeStatus.NotRegister;
                myConn.Close();
                return result;
            }
            else
            {
                foreach (DataRow mDr in myDs.Tables[0].Rows)
                {
                    foreach (DataColumn mDc in myDs.Tables[0].Columns)
                    {
                        if (mDc.ColumnName == "Code")
                        {
                            tempcode = mDr[mDc].ToString();
                        }
                    }
                }
            }

            if (tempcode != code)
            {
                result = InvitationCodeStatus.NotMatch;
                return result;
            }

            return result;
        }

        public List<Person> GetPersonListByDataSet(DataSet myDs)
        {
            List<Person> PersonList = new List<Person>();

            foreach (DataRow mDr in myDs.Tables[0].Rows)
            {
                Person person = new Person();

                foreach (DataColumn mDc in myDs.Tables[0].Columns)
                {
                    if (mDc.ColumnName == "DisplayName")
                    {
                        person.displayname = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Alias")
                    {
                        person.alias = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Title")
                    {
                        person.title = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Team")
                    {
                        person.team = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Specialty")
                    {
                        person.specialty = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "FavoriteSport")
                    {
                        person.favoritesport = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "photoPath")
                    {
                        person.photoPath = mDr[mDc].ToString();
                    }

                }
                PersonList.Add(person);

            }
            return PersonList;
        }

        public DataSet[] GetDataSetsByAlias(string[] str)
        {
            DataSet[] dsarray = new DataSet[str.Length];

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            string query = "select * from FaceWebsiteTable WHERE alias='";

            for (int i = 0; i < str.Length; i++)
            {
                query += str[i] + "'";
                DataSet myDs = new DataSet();
                SqlDataAdapter myDa = new SqlDataAdapter(query, myConn);

                myDa.Fill(myDs);

                dsarray[i] = myDs;
                myDa.Dispose();
                myDs.Dispose();
                query = "select * from FaceWebsiteTable WHERE alias='";
            }

            myConn.Close();
            return dsarray;
        }

        public string[] GetDisplayNameByDsarray(DataSet[] dsarray)
        {
            string[] displayname = new string[dsarray.Length];
            string tempname = "";

            for (int k = 0; k < dsarray.Length; k++)
            {
                if (dsarray[k].Tables[0].Rows.Count == 0)
                {
                    displayname[k] = "No Match";
                    continue;
                }

                foreach (DataRow mDr in dsarray[k].Tables[0].Rows)
                {
                    foreach (DataColumn mDc in dsarray[k].Tables[0].Columns)
                    {
                        if (mDc.ColumnName == "DisplayName")
                        {
                            tempname = mDr[mDc].ToString();
                            //Response.Write(tempname);                     
                            displayname[k] = tempname;
                        }
                    }
                }
            }
            //Response.Write(displayname.Length);

            return displayname;
        }
    }
}