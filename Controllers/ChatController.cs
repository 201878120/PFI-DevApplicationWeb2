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
        public ActionResult Index()
        {
            return View();
        }

        [OnlineUsers.PowerUserAccess]
        public ActionResult ModerateMessages()
        {
            return View();
        }

        public PartialViewResult GetFriendsList()
        {
            // Sorting here instead of calling SortedUsers() to minimize overhead
            int currentUserId = OnlineUsers.GetSessionUser().Id;
            return PartialView(DB.Users.ToList().Where(u => DB.Friendships.AreFriends(currentUserId, u.Id)).OrderBy(u => u.FirstName).ThenBy(u => u.LastName));
        }

        public PartialViewResult GetMessages()
        {
            User currentUser = OnlineUsers.GetSessionUser();
            if (Session["currentChattedId"] != null)
            {
                User otherUser = DB.Users.FindUser((int)Session["currentChattedId"]);
                return PartialView(DB.Messages.GetConversation(currentUser.Id, (int)Session["currentChattedId"]));
            } else return null;
        }

        [HttpGet]
        public void SetCurrentTarget(int id)
        {
            Session["currentChattedId"] = id;
        }

        [HttpGet]
        public void Send(string message)
        {
            Message m = new Message();
            User currentUser = OnlineUsers.GetSessionUser();
            m.FromUserId = currentUser.Id;
            m.ToUserId = (int)Session["currentChattedId"];
            m.Content = message;
            DB.Messages.Create(m);
        }

        [HttpGet]
        public void Update(int id, string message)
        {
            Message m = DB.Messages.FindMessage(id);
            m.Content = message;
            DB.Messages.Update(m);
        }

        [HttpGet]
        public void Delete(int id)
        {
            DB.Messages.Delete(id);
        }
    }
}