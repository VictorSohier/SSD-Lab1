using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment_1.Context;
using Assignment_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Assignment_1.Controllers
{
    //[AllowAnonymous]
    [Authorize(Roles = "Player,Manager")]
    public class TeamsController : Controller
    {
        private Assignment1DBContext _Context;

        public TeamsController(Assignment1DBContext context)
        {
            _Context = context;
        }

        // GET: TeamsController
        public ActionResult Index()
        {
            List<Team> teams = _Context.Set<Team>().ToList();
            return View(teams);
        }

        // GET: TeamsController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            return View(await _Context.Set<Team>().FindAsync(id));
        }

        [Authorize(Roles="Manager")]
        // GET: TeamsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamsController/Create
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Team model)
        {
            if (ModelState.IsValid)
            {
                await _Context.Set<Team>().AddAsync(model);
                await _Context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: TeamsController/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Edit(string id)
        {
            return View(await _Context.Set<Team>().FindAsync(id));
        }

        // POST: TeamsController/Edit/5
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Team model)
        {
            if (ModelState.IsValid)
            {
                _Context.Set<Team>().Update(model);
                await _Context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: TeamsController/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(string id)
        {
            return View(await _Context.Set<Team>().FindAsync(id));
        }

        // POST: TeamsController/Delete/5
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Team model)
        {
            _Context.Set<Team>().Remove(model);
            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
