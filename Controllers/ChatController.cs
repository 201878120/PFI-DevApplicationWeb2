using ChatManager.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            return View();
        }

        [OnlineUsers.PowerUserAccess]
        public ActionResult ModerateMessages()
        {
            return View();
        }
        [OnlineUsers.UserAccess]
        public PartialViewResult GetFriendsList()
        {
            // Sorting here instead of calling SortedUsers() to minimize overhead
            int currentUserId = OnlineUsers.GetSessionUser().Id;
            return PartialView(DB.Users.ToList().Where(u => DB.Friendships.AreFriends(currentUserId, u.Id)).OrderBy(u => u.FirstName).ThenBy(u => u.LastName));
        }

        [OnlineUsers.UserAccess]
        public PartialViewResult GetMessages()
        {
            User currentUser = OnlineUsers.GetSessionUser();
            if (Session["currentChattedId"] != null)
            {
                User otherUser = DB.Users.FindUser((int)Session["currentChattedId"]);
                return PartialView(DB.Messages.GetConversation(currentUser.Id, (int)Session["currentChattedId"]));
            } else return null;
        }

        [OnlineUsers.AdminAccess(false)] // RefreshTimout = false otherwise periodical refresh with lead to never timed out session
        public ActionResult GetMessagesList(/*bool forceRefresh = false*/)
        {
            /*if (forceRefresh || DB.Messages.)
            {*/
                return PartialView(DB.Messages.SortedMessages());
            /*}
            return null;*/
        }

        [HttpGet]
        [OnlineUsers.UserAccess]
        public void SetCurrentTarget(int id)
        {
            Session["currentChattedId"] = id;
        }

        [HttpGet]
        [OnlineUsers.UserAccess]
        public void Send(string message)
        {
            if (Session["currentChattedId"] == null) return;
            Message m = new Message();
            User currentUser = OnlineUsers.GetSessionUser();
            m.FromUserId = currentUser.Id;
            m.ToUserId = (int)Session["currentChattedId"];
            m.Content = message;
            DB.Messages.Create(m);
        }

        [HttpGet]
        [OnlineUsers.UserAccess]
        public void Update(int id, string message)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            Message m = DB.Messages.FindMessage(id);
            if (currentUser.Id != m.FromUserId) return;
            m.Content = message;
            DB.Messages.Update(m);
        }

        [HttpGet]
        [OnlineUsers.UserAccess]
        public void Delete(int id)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            Message m = DB.Messages.Get(id);
            if (currentUser.Id != m.FromUserId && !currentUser.IsPowerUser) return;
            DB.Messages.Delete(id);
        }
    }
}