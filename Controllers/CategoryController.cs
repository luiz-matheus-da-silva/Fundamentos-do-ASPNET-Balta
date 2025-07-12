using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05XE1 - Não foi possível buscar a categoria."));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("05XE2 - Não foi possível buscar a categoria."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05XE3 - Falha interna no servidor."));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromServices] BlogDataContext context,
            EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try
            {
                Category category = new Category
                {
                    Name = model.Name,
                    Slug = model.Slug,
                    Posts = [],
                    Id = 0
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("05XE3 - Não foi possível incluir a categoria."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05XE4 - Falha interna no servidor."));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id,
            EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("05XE6 - Não foi possível atualizar a categoria."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05XE7 - Falha interna no servidor."));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("05XE8 - Não foi possível excluir a categoria."));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<string>("05XE9 - Falha interna no servidor."));
            }
        }
    }
}
