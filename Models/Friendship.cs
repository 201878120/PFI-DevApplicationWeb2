using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public enum FriendshipStatus
    {
        // Shouldn't be used in a friend request object, used when there is no friendship and a status is needed
        None,
        Friends,
        FriendRequestSent,
        FriendRequestDeclined
    }
    public class Friendship : IEquatable<Friendship>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TargetUserId { get; set; }
        public FriendshipStatus Status { get; set; }

        public Friendship(){ } // Required for repository

        public Friendship(int userId, int targetUserId, FriendshipStatus status)
        {
            UserId = userId;
            TargetUserId = targetUserId;
            Status = status;
        }
        public bool AreTheUsers(int userId1, int userId2)
        {
            return (UserId == userId1 && TargetUserId == userId2) || (UserId == userId2 && TargetUserId == userId1);
        }

        #region Equals
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Friendship)) return false;
            return Equals(obj as Friendship);
        }
        public bool Equals(Friendship other)
        {
            if (other is null) return this is null;
            return AreTheUsers(other.UserId, other.TargetUserId) && Status == other.Status; 
        }
        public static bool operator==(Friendship f1, Friendship f2)
        {
            return f1.Equals(f2);
        }
        public static bool operator!=(Friendship f1, Friendship f2)
        {
            return !(f1 == f2);
        }
        #endregion
    }

}