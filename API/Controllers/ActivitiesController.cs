using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private DataContext m_DataContext;

        public ActivitiesController(DataContext context)
        {
            m_DataContext = context;
        }

        [HttpGet]

        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await m_DataContext.Activities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            var activity = await m_DataContext.Activities.FindAsync(id);
            return activity ?? new Activity();
        }
    }
}