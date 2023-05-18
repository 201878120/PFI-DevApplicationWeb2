using ChatManager.Models;
using System;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {
        public enum FriendFilters
        {
            NOT_FRIEND=1,
            REQUEST=2,
            PENDING=4,
            FRIEND=8,
            REFUSED=16,
            BLOCKED=32
        }
        // GET: Friendships
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            if (Session["FriendFilters"] == null)
            {
                int newFilter = 0;
                foreach (var val in Enum.GetValues(typeof(FriendFilters)))
                {
                    newFilter |= (int)val;
                }
                Session["FriendFilters"] = newFilter;
            }
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

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterNotFriend(bool check)
        {

        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterRequest(bool check)
        {

        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterPending(bool check)
        {

        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterFriend(bool check)
        {

        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterRefused(bool check)
        {

        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterBlocked(bool check)
        {

        }
    }
}