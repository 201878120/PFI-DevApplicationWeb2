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

        public PartialViewResult GetFriendsList()
        {
            List<(string, int, string)> amis = new List<(string, int, string)>
            {
                ("Saliha Yacoub", 2, "4491ed49-2848-4618-b131-7036cb07126f"),
                ("Stéphane Chassé", 3, "89ca9b58-9e3d-4c21-bb08-369f8f04b2fb")
            };
            return PartialView(amis);
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