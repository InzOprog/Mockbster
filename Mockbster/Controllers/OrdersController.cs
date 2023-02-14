using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mockbster.Data;
using Mockbster.Models;
using Newtonsoft.Json;

namespace Mockbster.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MockbsterContext _context;
        private CartModel? _cartVm;
        public OrdersController(MockbsterContext context) { _context = context; }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("cartList") != null)
            {
                _cartVm = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("cartList")!
                    )!;
            }
            return View(_cartVm);
        }
        public IActionResult Edit(int id, int day)
        {
            if (HttpContext.Session.GetString("cartList") == null) 
                return RedirectToAction(nameof(Index));
            
            _cartVm = JsonConvert.DeserializeObject<CartModel>(
                HttpContext.Session.GetString("cartList")!
            )!;
            if (_cartVm.Movies![id].Item2 + day <= 0 || _cartVm.Movies[id].Item2 + day >= 31) 
                return RedirectToAction(nameof(Index));
            
            _cartVm.Movies[id] = new Tuple<int, int, MovieModel>(
                _cartVm.Movies[id].Item1,
                _cartVm.Movies[id].Item2 + day,
                _cartVm.Movies[id].Item3
            );
            HttpContext.Session.SetString("cartList", JsonConvert.SerializeObject(_cartVm));
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if(id == null) { return NotFound(); }
            if (HttpContext.Session.GetString("cartList") == null) return RedirectToAction(nameof(Index));
            
            _cartVm = JsonConvert.DeserializeObject<CartModel>(
                HttpContext.Session.GetString("cartList")!
            )!;
            _cartVm.Movies.RemoveAt((int)id);
            HttpContext.Session.SetString("cartList", JsonConvert.SerializeObject(_cartVm));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> FinalizeAsync()
        {
            if (HttpContext.Session.GetString("cartList") == null) return RedirectToAction("Index", "Home");
            
            _cartVm = JsonConvert.DeserializeObject<CartModel>(
                HttpContext.Session.GetString("cartList")!
            )!;

            var users = from m in _context.User
                where m.Username == HttpContext.Request.Cookies["loggedUser"]
                select m.Id;
            
            var uid = await users.FirstOrDefaultAsync();
            var dayNow = DateTime.Now;
            foreach (var order in from movie in _cartVm.Movies! 
                     let dayRent = DateTime.Now.AddDays(movie.Item2) 
                     select new OrderModel
                     {
                         UserId = uid,
                         MovieId = movie.Item1,
                         OrderBegin = dayNow,
                         OrderEnd = dayRent,
                     })
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
