using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Models;
using PMS.Persistence.IRepository;
using PMS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Persistence.Repository
{
    public class TagRepository : ITagRepository
    {
        private ApplicationDbContext context;

        public TagRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Tag> GetTag(int? id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await context.Tags.FindAsync(id);
            }
            return await context.Tags
                .Include(g => g.TagProjects)
                    .ThenInclude(tp => tp.Project)
                .SingleOrDefaultAsync(g => g.TagId == id);
        }

        public void AddTag(Tag tag)
        {
            context.Tags.Add(tag);
        }

        public void RemoveTag(Tag tag)
        {
            tag.IsDeleted = true;
            //context.Remove(Grade);
        }

        public async Task<IEnumerable<Tag>> GetTags()
        {
            return await context.Tags
                .Include(g => g.TagProjects)
                    .ThenInclude(tp => tp.Project)
                .ToListAsync();
        }

        public void UpdateTagProjects(Tag tag, TagResource tagResource)
        {
            if (tagResource.TagProjects != null && tagResource.TagProjects.Count >= 0)
            {
                //remove old tagprojects
                tag.TagProjects.Clear();

                //add new tagprojects
                var newTagProjects = context.TagProjects.Where(e => tagResource.TagProjects.Any(id => id == e.TagProjectId)).ToList();
                foreach (var a in newTagProjects)
                {
                    tag.TagProjects.Add(a);
                }
            }
        }
    }
}
