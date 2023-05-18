using ChatManager.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {
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

        private bool PassesFriendFilter(User targetUser)
        {
            int friendFilters = (int)Session["FriendFilters"];
            
            if (targetUser.Blocked)
                return (friendFilters & (int)FriendFilters.BLOCKED) > 0;
            
            User currentUser = OnlineUsers.GetSessionUser();
            Friendship f = DB.Friendships.GetFriendship(currentUser.Id, targetUser.Id);
            FriendshipStatus status = f is null ? FriendshipStatus.None : f.Status;
            
            switch (status)
            {
                case FriendshipStatus.None:
                    return (friendFilters & (int)FriendFilters.NOT_FRIEND) > 0;
                case FriendshipStatus.Friends:
                    return (friendFilters & (int)FriendFilters.FRIEND) > 0;
                case FriendshipStatus.FriendRequestSent:
                    if (currentUser.Id == f.UserId)
                        return (friendFilters & (int)FriendFilters.PENDING) > 0;
                    else
                        return (friendFilters & (int)FriendFilters.REQUEST) > 0;
                case FriendshipStatus.FriendRequestDeclined:
                    return (friendFilters & (int)FriendFilters.REFUSED) > 0;
            }

            return false;
        }
        [OnlineUsers.UserAccess]
        public PartialViewResult GetFriendShipsStatus()
        {
            return PartialView(DB.Users.SortedUsers().Where(PassesFriendFilter));
        }

        private void SetFriendFilterBit(FriendFilters filter, bool check)
        {
            if (check)
            {
                Session["FriendFilters"] = (int)Session["FriendFilters"] | (int)filter;
            }
            else
            {
                Session["FriendFilters"] = (int)Session["FriendFilters"] & ~(int)filter;
            }
        }
        [OnlineUsers.UserAccess]
        public JsonResult SetFilterNotFriend(bool check)
        {
            SetFriendFilterBit(FriendFilters.NOT_FRIEND, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterRequest(bool check)
        {
            SetFriendFilterBit(FriendFilters.REQUEST, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterPending(bool check)
        {
            SetFriendFilterBit(FriendFilters.PENDING, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterFriend(bool check)
        {
            SetFriendFilterBit(FriendFilters.FRIEND, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterRefused(bool check)
        {
            SetFriendFilterBit(FriendFilters.REFUSED, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess]
        public JsonResult SetFilterBlocked(bool check)
        {
            SetFriendFilterBit(FriendFilters.BLOCKED, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}