using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OracleEFCore.UI.Site.Models;
using OracleEFCore.UI.Site.Repositories;

namespace OracleEFCore.UI.Site.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IDepartamentoRepository _depRepository;

        public DepartamentosController(IDepartamentoRepository dept)
        {
            _depRepository = dept;
        }
        public async Task<IActionResult> Index()
        {           
            return View(await _depRepository.GetAllAsync());
        }
        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Departamento dept)
        {
            if (ModelState.IsValid)
            {
               var result =  _depRepository.AddAsync(dept);
                
                //if (result == null)
                //{
                //    return BadRequest();
                //}
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }


        // GET: Blogs/Create
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _depRepository.FindAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Departamento dept)
        {
            if (ModelState.IsValid)
            {
               await  _depRepository.UpdateAsync(dept);
                //if (result == null)
                //{
                //    return BadRequest();
                //}
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }

        // GET: Blogs/Create
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _depRepository.FindAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Departamento dept)
        {
            if (ModelState.IsValid)
            {
                await _depRepository.DeleteAsync(dept);
                //if (result == null)
                //{
                //    return BadRequest();
                //}
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }
    }
}