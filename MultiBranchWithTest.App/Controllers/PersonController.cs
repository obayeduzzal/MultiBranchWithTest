using Microsoft.AspNetCore.Mvc;
using MultiBranchWithTest.App.Models;
using MultiBranchWithTest.App.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiBranchWithTest.App.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        public IActionResult Get()
        {
            var items = _personService.GetAllItems();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _personService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Person value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _personService.Add(value);
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var existingItem = _personService.GetById(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _personService.Remove(id);
            return NoContent();
        }
    }
}
