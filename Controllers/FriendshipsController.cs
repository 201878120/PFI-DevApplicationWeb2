using ChatManager.Models;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {
        // GET: Friendships
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AreFriends(int userId1, int userId2)
        {
            return Json(DB.Friendships.AreFriends(userId1, userId2));
        }

        public JsonResult SendFriendRequest(int other)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            FriendshipStatus status = DB.Friendships.GetUsersStatus(currentUser.Id, other);
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}