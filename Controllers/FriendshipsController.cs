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
            Session["searchQuery"] = "";
            int newFilter = 0;
            foreach (var val in Enum.GetValues(typeof(FriendFilters)))
            {
                newFilter |= (int)val;
            }
            Session["FriendFilters"] = newFilter;
            return View();
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult SendFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.SendFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult AcceptFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.AcceptFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult DeclineFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.DeclineFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult RemoveFriendshipRequest(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.RemoveFriendshipRequest(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult RemoveFriendship(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            return Json(DB.Friendships.RemoveFriendship(currentUser.Id, id), JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        private bool FilterSearchAndFilter(User targetUser)
        {
            bool matchesFilter = false;
            int friendFilters = (int)Session["FriendFilters"];

            if (targetUser.Blocked)
                matchesFilter = (friendFilters & (int)FriendFilters.BLOCKED) > 0;
            else
            {
                User currentUser = OnlineUsers.GetSessionUser();
                Friendship f = DB.Friendships.GetFriendship(currentUser.Id, targetUser.Id);
                FriendshipStatus status = f is null ? FriendshipStatus.None : f.Status;

                switch (status)
                {
                    case FriendshipStatus.None:
                        matchesFilter = (friendFilters & (int)FriendFilters.NOT_FRIEND) > 0;
                        break;
                    case FriendshipStatus.Friends:
                        matchesFilter = (friendFilters & (int)FriendFilters.FRIEND) > 0;
                        break;
                    case FriendshipStatus.FriendRequestSent:
                        if (currentUser.Id == f.UserId)
                            matchesFilter = (friendFilters & (int)FriendFilters.PENDING) > 0;
                        else
                            matchesFilter = (friendFilters & (int)FriendFilters.REQUEST) > 0;
                        break;
                    case FriendshipStatus.FriendRequestDeclined:
                        matchesFilter = (friendFilters & (int)FriendFilters.REFUSED) > 0;
                        break;
                }
            }
            
            return matchesFilter && targetUser.GetFullName().Contains((string)Session["searchQuery"]);
        }
        [OnlineUsers.UserAccess(false)]
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
        [OnlineUsers.UserAccess(false)]
        public JsonResult SetFilterNotFriend(bool check)
        {
            SetFriendFilterBit(FriendFilters.NOT_FRIEND, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult SetFilterRequest(bool check)
        {
            SetFriendFilterBit(FriendFilters.REQUEST, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult SetFilterPending(bool check)
        {
            SetFriendFilterBit(FriendFilters.PENDING, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult SetFilterFriend(bool check)
        {
            SetFriendFilterBit(FriendFilters.FRIEND, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult SetFilterRefused(bool check)
        {
            SetFriendFilterBit(FriendFilters.REFUSED, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public JsonResult SetFilterBlocked(bool check)
        {
            SetFriendFilterBit(FriendFilters.BLOCKED, check);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [OnlineUsers.UserAccess(false)]
        public void Search(string text)
        {
            Session["searchQuery"] = text;
        }

        [OnlineUsers.UserAccess(false)]
        public PartialViewResult GetFriendShipsStatus()
        {
            return PartialView(DB.Users.SortedUsers().Where(FilterSearchAndFilter));
        }
    }
}