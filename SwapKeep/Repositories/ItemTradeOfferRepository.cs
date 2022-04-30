﻿using Microsoft.Extensions.Configuration;
using SwapKeep.Models;
using SwapKeep.Repositories;
using SwapKeep.Utils;
using System;
using System.Collections.Generic;

namespace SwapKeep.Repositories
{
    public class ItemTradeOfferRepository : BaseRepository, IItemTradeOfferRepository
    {
        public ItemTradeOfferRepository(IConfiguration config) : base(config) { }

        public List<ItemTradeOffer> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Party1ItemId, Party2ItemId, StatusId FROM ItemTradeOffer";

                    var offers = new List<ItemTradeOffer>();

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            offers.Add(new ItemTradeOffer()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Party1ItemId = DbUtils.GetInt(reader, "Party1ItemId"),
                                Party2ItemId = DbUtils.GetInt(reader, "Party2ItemId"),
                                StatusId = DbUtils.GetInt(reader, "StatusId")

                            });
                        }

                        return offers;

                    }
                }
            }
        }

        public ItemTradeOffer GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"SELECT Id, Party1ItemId, Party2ItemId, StatusId FROM ItemTradeOffer
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    var offer = new ItemTradeOffer();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            offer = new ItemTradeOffer()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Party1ItemId = DbUtils.GetInt(reader, "Party1ItemId"),
                                Party2ItemId = DbUtils.GetInt(reader, "Party2ItemId"),
                                StatusId = DbUtils.GetInt(reader, "StatusId")
                            };
                        }

                        return offer;
                    }

                }
            }
        }

        public void AddOffer(ItemTradeOffer offer)
        {

            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO ItemTradeOffer (Party1ItemId, Party2ItemId, StatusId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@party1ItemId, @party2ItemId, 1)";
                    DbUtils.AddParameter(cmd, "@party1ItemId", offer.Party1ItemId);
                    DbUtils.AddParameter(cmd, "@party2ItemId", offer.Party2ItemId);



                    offer.Id = (int)cmd.ExecuteScalar();
                }
            }

        }

        public void DeleteOfferById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = @"DELETE FROM ItemTradeOffer WHERE Id = @id";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            }
        }
    }
}
