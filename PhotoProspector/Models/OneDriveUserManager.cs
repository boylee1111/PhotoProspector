using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoProspector.Models
{
    public class OneDriveUserManager
    {
        private static Dictionary<string, OneDriveUser> KnownUsers = new Dictionary<string, OneDriveUser>();

        public static OneDriveUser LookupUserById(string userGuid)
        {
            OneDriveUser user;
            if (KnownUsers.TryGetValue(userGuid, out user))
                return user;

            throw new InvalidOperationException("Unknown user.");
        }

        public static void RegisterUser(string userGuid, OneDriveUser user)
        {
            KnownUsers[userGuid] = user;
        }

        public static OneDriveUser LookupUserForSubscriptionId(string subscriptionId)
        {
            var query = from u in KnownUsers.Values
                        where u.SubscriptionId == subscriptionId
                        select u;

            return query.FirstOrDefault();
        }
    }
}
