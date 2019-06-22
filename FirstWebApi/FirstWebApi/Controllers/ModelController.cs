using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstWebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {

        public ModelContext modelContext;

        public ModelController(ModelContext _modelContext)
        {
            modelContext = _modelContext;

           if (modelContext.ModelEntity.Count() == 0)
           {
               modelContext.ModelEntity.Add(new ModelEntity { Name="Shubham",IsComplete=true});
              modelContext.SaveChanges();
            }
        }
        /// <summary>
        /// This will get all Items 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelClass>>> GetItems()
        {
            return await modelContext.ModelEntity.ToListAsync();
        }

        /// <summary>
        /// This will get single item
        /// </summary>
        /// <param name="Id"> ID Is mandatory</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelClass>> GetItem(long Id)
        {
            var modelClass = await modelContext.ModelEntity.FindAsync(Id);

            if(modelClass==null)
            {
                return NotFound();
            }
            return modelClass;

        }


        [HttpPost]
        public async Task<ActionResult<ModelClass>> PostModel(ModelClass modelClass)
        {
            modelContext.ModelEntity.Add(modelClass);
            await modelContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem),new { Id=modelClass.Id},modelClass);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutModel(long Id, ModelClass modelClass)
        {
            if (Id != modelClass.Id)
            {
                return BadRequest();
            }

            modelContext.Entry(modelClass).State = EntityState.Modified;
            await modelContext.SaveChangesAsync();

            return NoContent();


        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteModel(long Id)
        {
            var modelClass = await modelContext.ModelEntity.FindAsync(Id);
            if (modelClass == null)
            {
                return NotFound();
            }

            modelContext.ModelEntity.Remove(modelClass);
            await modelContext.SaveChangesAsync();

            return NoContent();

        }  





    }
}
