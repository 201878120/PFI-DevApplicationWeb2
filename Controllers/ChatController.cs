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

        [HttpGet]
        public ContentResult GetFriendsList()
        {
            string s = "";
            List<Dictionary<string, string>> amis = new List<Dictionary<string, string>>();
            Dictionary<string, string> c = new Dictionary<string, string>
            {
                { "Name", "Saliha Yacoub" },
                { "UserId", "2" },
                { "Image", "4491ed49-2848-4618-b131-7036cb07126f" }
            };
            amis.Add(c);
            Dictionary<string, string> c2 = new Dictionary<string, string>
            {
                { "Name", "Stéphane Chassé" },
                { "UserId", "3" },
                { "Image", "89ca9b58-9e3d-4c21-bb08-369f8f04b2fb" }
            };
            amis.Add(c2);

            foreach(Dictionary<string, string> ami in amis)
            {
                string selected = "unselectedTarget";
                if (Session["currentChattedId"] != null) if (ami["UserId"] == Session["currentChattedId"].ToString()) selected = "selectedTarget";
                s += 
                "<div class='" + selected + "' userid='" + ami["UserId"] + "'>\r\n" +
                    "<div class='UserSmallAvatar' title='" + ami["Name"] + "' \r\n" +
                        "style='background: url(/Images_Data/User_Avatars/" + ami["Image"] + ".Jpeg)' >\r\n" +
                    "</div>\r\n" +
                "</div>\r\n";
            }
            return Content(s, "text/html");
        }

        [HttpGet]
        public ContentResult GetMessages()
        {
            User currentUser = DB.Users.FindUser((int)Session["currentLoginId"]);
            string s = "";
            if (Session["currentChattedId"] != null)
            {
                User otherUser = DB.Users.FindUser((int)Session["currentChattedId"]);
                s =
                "<div class='messagesHeader'>" +
                    "<h4>Conversation avec</h4>" +
                    "<div class='userItem'>" +
                        "<div class='UserMediumAvatar'" +
                            "style='background: url(/Images_Data/User_Avatars/" + otherUser.Avatar + ".Jpeg)'>" +
                        "</div>" +
                        "<div class='ellipsis'>" + otherUser.FirstName + otherUser.LastName + "</div>" +
                    "</div>" +
                "</div>";
            }
            //Session["currentLoginId"]
            //Session["currentChattedId"]
            return Content(s, "text/html");
        }

        [HttpGet]
        public void SetCurrentTarget(int id)
        {
            Session["currentChattedId"] = id;
        }

        [HttpGet]
        public JsonResult Send(string message)
        {
            return Json("fake", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Update(int id, string message)
        {
            return Json("fake", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            return Json("fake", JsonRequestBehavior.AllowGet);
        }
    }
}