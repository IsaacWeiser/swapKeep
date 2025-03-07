﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapKeep.Models;
using SwapKeep.Repositories;
using System;
using System.Security.Claims;

namespace SwapKeep.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public ItemController(IItemRepository itemRepo, IUserProfileRepository user)
        {
            _itemRepo = itemRepo;
            _userProfileRepo = user;
        }

        [HttpGet("listings")]
        public IActionResult GetByZip()
        {
            UserProfile user = GetCurrentUserProfile();
            int zip = user.ZipCode;
            int id = user.Id;
            return Ok(_itemRepo.GetItemsByZipCode(zip, id));
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(_itemRepo.GetAllItems());
        }

        [HttpGet("itemsofuser")]
        public IActionResult GetUserItems()
        {
            UserProfile user = GetCurrentUserProfile();
            int id = user.Id;
            return Ok(_itemRepo.GetItemsByUserId(id));
        }

        [HttpGet("item/{id}")]
        public IActionResult GetItemById(int id)
        {
            return Ok(_itemRepo.GetItemById(id));
        }

        [HttpGet("category/{catId}")]
        public IActionResult GetItemsByCategory(int catId)
        {
            UserProfile user = GetCurrentUserProfile();
            int usrId = user.Id;
            int zip = user.ZipCode;
            return Ok(_itemRepo.GetItemsNotOfCurrentUserAndOfACategoryAndAZipCode(usrId,catId,zip));
        }

        [HttpPost]
        public IActionResult Post(Item item)
        {
            UserProfile user = GetCurrentUserProfile();
            item.UserId = user.Id;
            _itemRepo.AddItem(item);
            return CreatedAtAction("post", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _itemRepo.DeleteItem(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _itemRepo.Update(item);
            return Ok(item);
        }

        private UserProfile GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userProfileRepo.GetByFirebaseUserId(firebaseUserId);
        }

    }
}
