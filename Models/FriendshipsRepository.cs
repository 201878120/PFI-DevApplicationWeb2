using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public enum FriendFilters
    {
        NOT_FRIEND = 1,
        REQUEST = 2,
        PENDING = 4,
        FRIEND = 8,
        REFUSED = 16,
        BLOCKED = 32
    }

    public class FriendshipsRepository : Repository<Friendship>
    {
        public Friendship GetFriendship(int userId1, int userId2)
        {
            return ToList().Where(f => f.AreTheUsers(userId1, userId2)).FirstOrDefault();
        }
        public FriendshipStatus GetUsersStatus(int userId1, int userId2)
        {
            Friendship friendship = GetFriendship(userId1, userId2);

            if (friendship is null)
                return FriendshipStatus.None;

            return friendship.Status;
        }
        public bool AreFriends(int userId1, int userId2)
        {
            return GetUsersStatus(userId1, userId2) == FriendshipStatus.Friends;
        }
        public bool SendFriendshipRequest(int userId, int targetUserId)
        {
            // in case someone's being a funny man
            if (userId == targetUserId)
                return false;

            Friendship friendship = GetFriendship(userId, targetUserId);

            // Friendship doesn't exist
            if (friendship is null)
            {
                Add(new Friendship(userId, targetUserId, FriendshipStatus.FriendRequestSent));
                return true;
            }
            
            // If this friendship was ever declined
            if (friendship.Status == FriendshipStatus.FriendRequestDeclined)
            {
                // If the invoking user declined the last friend request, he can change his mind
                if (userId == friendship.TargetUserId)
                {
                    friendship.UserId = userId;
                    friendship.TargetUserId = targetUserId;
                    friendship.Status = FriendshipStatus.FriendRequestSent;
                    Update(friendship);
                    return true;
                }
            }

            return false;
        }
        public bool AcceptFriendshipRequest(int userId, int other)
        {
            Friendship friendship = GetFriendship(userId, other);
            
            // If there was an incoming friendship, accept and become friends
            if (friendship.Status == FriendshipStatus.FriendRequestSent && friendship.TargetUserId == userId)
            {
                friendship.Status = FriendshipStatus.Friends;
                Update(friendship);
                return true;
            }
            return false;
        }

        public bool DeclineFriendshipRequest(int userId, int targetUserId)
        {
            Friendship friendship = GetFriendship(userId, targetUserId);
            
            // Declining that which never was
            if (friendship is null) return false;

            if (friendship.Status == FriendshipStatus.FriendRequestSent)
            {
                if (userId == friendship.TargetUserId)
                {
                    // if you were the target of a friend request, decline it
                    friendship.Status = FriendshipStatus.FriendRequestDeclined;
                    Update(friendship);
                    return true;
                }
            }

            return false;
        }

        public bool RemoveFriendshipRequest(int userId, int targetUserId)
        {
            Friendship friendship = GetFriendship(userId, targetUserId);
            if (friendship is null) return false;
            if (friendship.Status == FriendshipStatus.FriendRequestSent && userId == friendship.UserId)
            {
                // if you were the invoker, cancel the FR
                Delete(friendship.Id);
                return true;
            }

            return false;
        }
        public bool RemoveFriendship(int userId, int targetUserId)
        {
            Friendship friendship = GetFriendship(userId, targetUserId);
            if (friendship is null) return false;
            if (friendship.Status == FriendshipStatus.Friends)
            {
                Delete(friendship.Id);
                return true;
            }

            return false;
        }
    }
}