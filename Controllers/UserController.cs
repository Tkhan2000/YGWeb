using Microsoft.AspNetCore.Mvc;
using YGWeb.Data;
using YGWeb.Models;

namespace YGWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private User _currentUser;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, string pass)
        {
            IEnumerable<User> userList = _db.Users;
            if (userList != null)
            {
                foreach (User user in userList)
                {
                    if (user.username == name && user.password == pass)
                    {
                        ViewBag.currentUser = _currentUser = user;
                        return View(user);
                    }
                }
            }
            return View();
        }
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
