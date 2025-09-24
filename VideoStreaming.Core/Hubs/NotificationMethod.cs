using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreaming.Core.Hubs;

public static class NotificationMethod
{
    public const string UserJoinedTheStream = "UserJoinedTheStream";
    public const string UserLeftTheStream = "UserLeftTheStream";
    public const string BroadcasterCreatedOffer = "BroadcasterCreatedOffer";
    public const string ViewerRespondedToAnOffer = "ViewerRespondedToAnOffer";
    public const string IceCandidateReceived = "IceCandidateReceived";
}
