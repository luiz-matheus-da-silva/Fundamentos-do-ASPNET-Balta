﻿using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext _context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context.Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(
                    new
                    {
                        total = count,
                        page,
                        pageSize,
                        posts
                    }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Post>(
                     "05X04 - Falha interna no servidor." ));
            }
        }
        
        [HttpGet("v1/posts/categories/{category}")]
        public async Task<IActionResult> GetByCategoryAsync([FromServices] BlogDataContext _context,
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
            )
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context.Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Where(x => x.Category.Slug == category)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(
                    new
                    {
                        total = count,
                        page,
                        pageSize,
                        posts
                    }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Post>(
                     "05X04 - Falha interna no servidor." ));
            }
        }

        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> GetByCategoryAsync([FromServices] BlogDataContext _context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25,
            [FromQuery] int id = 0)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context.Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Where(x => x.Id == id)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(
                    new
                    {
                        total = count,
                        page,
                        pageSize,
                        posts
                    }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Post>(
                     "05X04 - Falha interna no servidor."));
            }
        }
    }
}
