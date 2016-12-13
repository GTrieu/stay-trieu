using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Sabio.Data;
using Sabio.Web.Models.Requests;
using Sabio.Web.Domain;

namespace Sabio.Web.Services
{
    public class ArticlesService : BaseService, IArticlesService
    {
        public int Insert(ArticlesAddRequest model)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Articles_Insert",
            inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                string UserId = UserService.GetCurrentUserId();
                paramCollection.AddWithValue("@UserId", UserId);
                paramCollection.AddWithValue("@Title", model.Title);
                paramCollection.AddWithValue("@Author", model.Author);
                paramCollection.AddWithValue("@Content", model.Content);

                SqlParameter p = new SqlParameter("@id", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
                paramCollection.Add(p);
            }, returnParameters: delegate (SqlParameterCollection param)
            {
                int.TryParse(param["@id"].Value.ToString(), out id);
            }
            );

            return id;
        }

        public void Put(ArticlesUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Articles_Update"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    string UserId = UserService.GetCurrentUserId();
                    paramCollection.AddWithValue("@id", model.Id);
                    paramCollection.AddWithValue("@UserId", UserId);
                    paramCollection.AddWithValue("@Title", model.Title);
                    paramCollection.AddWithValue("@Author", model.Author);
                    paramCollection.AddWithValue("@Content", model.Content);
                    paramCollection.AddWithValue("@IsDeleted", model.IsDeleted);
                }
                );
        }

        public List<Article> GetAll()
        {
            List<Article> list = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Articles_SelectAll"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)

            {
                Article a = new Article();
                int startingIndex = 0;
                a.Id = reader.GetSafeInt32(startingIndex++);
                a.Title = reader.GetSafeString(startingIndex++);
                a.Author = reader.GetSafeString(startingIndex++);
                a.Content = reader.GetSafeString(startingIndex++);
                a.DateAdded = reader.GetDateTime(startingIndex++);
                a.DateModified = reader.GetDateTime(startingIndex++);
                a.IsDeleted = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<Article>();
                }
                list.Add(a);
            }
            );
            return list; 
        }

        public Article GetById(int id)
        {
            Article a = new Article();
            DataProvider.ExecuteCmd(GetConnection, "dbo.Articles_SelectById"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                 {
                     paramCollection.AddWithValue("@id", id);
                 }
                , map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    a.Id = reader.GetInt32(startingIndex++);
                    a.Title = reader.GetSafeString(startingIndex++);
                    a.Author = reader.GetSafeString(startingIndex++);
                    a.Content = reader.GetSafeString(startingIndex++);
                    a.DateAdded = reader.GetSafeDateTime(startingIndex++);
                    a.DateModified = reader.GetSafeDateTime(startingIndex++);
                    a.IsDeleted = reader.GetSafeInt32(startingIndex++);
                }
                );
            return a; 
        }

        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Articles_Delete"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                 {
                     paramCollection.AddWithValue("@id", id);
                 }
                );
        }

        public void SoftDelete(ArticlesUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Articles_SoftDelete"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    string UserId = UserService.GetCurrentUserId();
                    paramCollection.AddWithValue("@id", model.Id);
                    paramCollection.AddWithValue("@UserId", UserId);
                    //paramCollection.AddWithValue("@Title", model.Title);
                    //paramCollection.AddWithValue("@Author", model.Author);
                    //paramCollection.AddWithValue("@Content", model.Content);
                    paramCollection.AddWithValue("@IsDeleted", model.IsDeleted);
                }
                );
        }



    }

}