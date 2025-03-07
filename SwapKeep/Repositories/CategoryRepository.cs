﻿using Microsoft.Extensions.Configuration;
using SwapKeep.Models;
using SwapKeep.Utils;
using System.Collections.Generic;

namespace SwapKeep.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }


        public string getCategoryById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select Id, Name FROM Category where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    string catName = null;

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            catName = DbUtils.GetString(reader, "Name");
                        }

                    }

                    return catName;
                }

            }
        }

        public List<Category> GetAllCategories()
        {

            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name FROM Category";

                    var categories = new List<Category>();

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            categories.Add(new Category() { 
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name =DbUtils.GetString(reader, "Name")
                            });
                        }

                    }

                    return categories;
                }
            }

        }


    }
}
