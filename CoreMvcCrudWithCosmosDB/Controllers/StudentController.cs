using CoreMvcCrudWithCosmosDB.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMvcCrudWithCosmosDB.Models;

namespace CoreMvcCrudWithCosmosDB.Controllers
{
    public class StudentController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        public StudentController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetStudentsAsync("SELECT * FROM c"));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Id,Name,Age,Address")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddStudentAsync(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,Age,Address")] Student student)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateStudentAsync(student.Id, student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Student student = await _cosmosDbService.GetStudentAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Student student = await _cosmosDbService.GetStudentAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await _cosmosDbService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetStudentAsync(id));
        }
    }
}