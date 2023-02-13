using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mockbster.Data;
using Mockbster.Models;

namespace Mockbster.Controllers
{
    public class UsersController : Controller
    {
        private readonly MockbsterContext _context;

        public UsersController(MockbsterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies["loggedUser"] != null)
            {
                string uname = HttpContext.Request.Cookies["loggedUser"]!;
                if (uname == null || _context.User == null)
                {
                    return NotFound();
                }

                var user = await _context.User
                    .FirstOrDefaultAsync(m => m.Username == uname);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            } else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Firstname,Lastname,Email,Username,Password")] UserModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
