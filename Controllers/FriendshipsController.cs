using ChatManager.Models;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {
        // GET: Friendships
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AreFriends(int userId1, int userId2)
        {
            return Json(DB.Friendships.AreFriends(userId1, userId2), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult SendFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.SendFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }        

        [OnlineUsers.UserAccess]
        public JsonResult AcceptFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.AcceptFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult DeclineFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.DeclineFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult RemoveFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.RemoveFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult RemoveFriendship(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.RemoveFriendship(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public PartialViewResult GetFriendShipsStatus()
        {
            return PartialView(DB.Users.SortedUsers());
        }
    }
}