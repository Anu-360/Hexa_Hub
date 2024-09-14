﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexa_Hub.DTO;
using Hexa_Hub.Exceptions;
using Hexa_Hub.Interface;
using Hexa_Hub.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ICategory _category;

        public CategoriesController(DataContext context, ICategory category)
        {
            _context = context;
            _category = category;
        }

        // GET: api/Categories
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {

            return await _category.GetAllCategories();
        }

      
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CategoriesDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _category.UpdateCategoryAsync(id, categoryDto);
                if (!result)
                {
                    return NotFound("Category not found.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound("No Category Found");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Category Updation Success");
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> PostCategory(CategoriesDto categoryDto)
        {
            var category = await _category.AddCategory(categoryDto);
            await _category.Save();
            return CreatedAtAction("GetCategories", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _category.DeleteCategory(id);
                await _category.Save();
                return NoContent();
            }
            catch (CategoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
