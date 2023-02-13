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
        List<Tuple<int, MovieModel>> cartLiat = new();

        private readonly MockbsterContext _context;

        public OrdersController(MockbsterContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            CartModel cartVM = new();
            cartVM.Movies = new();
            if (HttpContext.Session.GetString("carList") != null)
            {
                cartVM = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("carList")!
                    )!;
            }
            return View(cartVM);

        }
        public IActionResult Edit(int id, int day)
        {
            CartModel cartVM = new();
            cartVM.Movies = new();
            if (HttpContext.Session.GetString("carList") != null)
            {
                cartVM = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("carList")!
                    )!;
                if (cartVM.Movies[id].Item2 + day > 0 &&
                    cartVM.Movies[id].Item2 + day < 31)
                {
                    cartVM.Movies[id] = new Tuple<int, int, MovieModel>(
                        cartVM.Movies[id].Item1,
                        cartVM.Movies[id].Item2 + day,
                        cartVM.Movies[id].Item3
                        );
                    HttpContext.Session.SetString("carList", JsonConvert.SerializeObject(cartVM));
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            CartModel cartVM = new();
            cartVM.Movies = new();
            if (HttpContext.Session.GetString("carList") != null)
            {
                cartVM = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("carList")!
                    )!;
                cartVM.Movies.RemoveAt((int)id);
                HttpContext.Session.SetString("carList", JsonConvert.SerializeObject(cartVM));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> FinalizeAsync()
        {
            CartModel cartVM = new();
            cartVM.Movies = new();
            if (HttpContext.Session.GetString("carList") != null)
            {
                cartVM = JsonConvert.DeserializeObject<CartModel>(
                    HttpContext.Session.GetString("carList")!
                    )!;

                var users = from m in _context.User
                            where m.Username == HttpContext.Request.Cookies["loggedUser"]
                            select m.Id;
                int uid = await users.FirstOrDefaultAsync();
                DateTime dayNow = DateTime.Now;
                foreach (var movie in cartVM.Movies!)
                {
                    DateTime dayRent = DateTime.Now.AddDays(movie.Item2);
                    OrderModel order = new OrderModel
                    {
                        UserId = uid,
                        MovieId = movie.Item1,
                        OrderBegin = dayNow,
                        OrderEnd = dayRent,
                    };
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
