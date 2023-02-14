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
        public UsersController(MockbsterContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies["loggedUser"] == null) return NotFound();
            
            var uname = HttpContext.Request.Cookies["loggedUser"];
            if (uname == null) { return NotFound(); }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Username == uname);
            if (user == null) { return NotFound(); }

            var history = from m in _context.Order select m;
            history = history.Where(m => m.UserId == user.Id);

            var movies = from m in _context.Movie select m;

            var completeOrder = new List<Tuple<string, OrderModel>?>();
            foreach (var item in history)
            {
                completeOrder.Add( new Tuple<string, OrderModel>(
                    movies.FirstOrDefault(m => m.Id == item.MovieId)!.Title, 
                    item
                    ));
            }
            
            var userPageVm = new UserPageModel
            {
                UserData = user,
                UserHistory = completeOrder
            };
            return View(userPageVm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }

            var user = await _context.User.FindAsync(id);
            if (user == null) { return NotFound(); }
            
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Firstname,Lastname,Email,Username,Password")] UserModel user)
        {
            if (id != user.Id) { return NotFound(); }
            if (!ModelState.IsValid) return View(user);
            
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id)) { return NotFound(); }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
