﻿using SwapKeep.Models;
using System.Collections.Generic;

namespace SwapKeep.Repositories
{
    public interface IItemRepository
    {
        void AddItem(Item item);
        void DeleteItem(int id);
        List<Item> GetAllItems();
        Item GetItemById(int id);
        List<Item> GetItemsByUserId(int id);
        List<Item> GetItemsByZipCode(int zip, int usrId);
        void Update(Item item);
        List<Item> GetItemsNotOfCurrentUserAndOfACategoryAndAZipCode(int id, int catId, int zipCode);
    }
}