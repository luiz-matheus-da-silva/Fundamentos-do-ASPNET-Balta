using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
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
            var categories = await context.Categories.ToListAsync();
            return Ok(new { categories = categories });
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                return Ok(new { category = category });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "05XE1 - Não foi possível buscar a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "05XE2 - Falha interna no servidor.");
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> Post(
            [FromServices] BlogDataContext context,
            EditorCategoryViewModel model)
        {
            if (model == null)
                return NotFound();

            try
            {
                Category category = new Category
                {
                    Name = model.Name,
                    Slug = GenerateSlug(model.Name),
                    Posts = [],
                    Id = 0
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", model);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "05XE3 - Não foi possível incluir a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "05XE4 - Falha interna no servidor.");
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> Put(
            [FromServices] BlogDataContext context,
            [FromRoute] int id,
            EditorCategoryViewModel model)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                category = new Category
                {
                    Name = model.Name,
                    Slug = GenerateSlug(model.Name),
                    Posts = [],
                    Id = id
                };

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "05XE5 - Não foi possível atualizar a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "05XE6 - Falha interna no servidor.");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> Delete(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "05XE7 - Não foi possível excluir a categoria.");
            }
            catch (Exception)
            {
                return StatusCode(500, "05XE8 - Falha interna no servidor.");
            }
        }

        public string GenerateSlug(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return string.Empty;

            // Converte para minúsculas
            string slug = phrase.ToLowerInvariant();

            // Remove acentos
            slug = RemoveDiacritics(slug);

            // Remove caracteres inválidos
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Substitui espaços por hífens
            slug = Regex.Replace(slug, @"\s+", "-").Trim();

            // Remove múltiplos hífens
            slug = Regex.Replace(slug, @"-+", "-");

            return slug;
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
