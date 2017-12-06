﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PMS.Persistence;
using AutoMapper;
using PMS.Resources;
using PMS.Models;
using PMS.Persistence.IRepository;

namespace PMS.Controllers
{
    [Route("/api/announcementuser")]
    public class AnnouncementUserController : Controller
    {
        private IAnnouncementUserRepository repository;
        private IMapper mapper;
        private IUnitOfWork unitOfWork;

        public AnnouncementUserController(IMapper mapper, IUnitOfWork unitOfWork, IAnnouncementUserRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateAnnouncementUser([FromBody]AnnouncementUserResource announcementUserResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var announcementUser = mapper.Map<AnnouncementUserResource, AnnouncementUser>(announcementUserResource);

            repository.AddAnnouncementUser(announcementUser);
            await unitOfWork.Complete();

            announcementUser = await repository.GetAnnouncementUser(announcementUser.AnnouncementUserId);

            var result = mapper.Map<AnnouncementUser, AnnouncementUserResource>(announcementUser);

            return Ok(result);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateAnnouncementUser(int id, [FromBody]AnnouncementUserResource announcementUserResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var announcementUser = await repository.GetAnnouncementUser(id);

            if (announcementUser == null)
                return NotFound();

            mapper.Map<AnnouncementUserResource, AnnouncementUser>(announcementUserResource, announcementUser);
            await unitOfWork.Complete();

            var result = mapper.Map<AnnouncementUser, AnnouncementUserResource>(announcementUser);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteAnnouncementUser(int id)
        {
            var announcementUser = await repository.GetAnnouncementUser(id, includeRelated: false);

            if (announcementUser == null)
            {
                return NotFound();
            }

            repository.RemoveAnnouncementUser(announcementUser);
            await unitOfWork.Complete();

            return Ok(id);
        }

        [HttpGet]
        [Route("getannouncementuser/{id}")]
        public async Task<IActionResult> GetAnnouncementUser(int id)
        {
            var announcementUser = await repository.GetAnnouncementUser(id);

            if (announcementUser == null)
            {
                return NotFound();
            }

            var announcementUserResource = mapper.Map<AnnouncementUser, AnnouncementUserResource>(announcementUser);

            return Ok(announcementUserResource);
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAnnouncementUserResources()
        {
            var announcementUsers = await repository.GetAnnouncementUsers();
            var announcementUserResource = mapper.Map<IEnumerable<AnnouncementUser>, IEnumerable<AnnouncementUserResource>>(announcementUsers);
            return Ok(announcementUserResource);
        }
    }
}