namespace VideoStreaming.Core.Models
{
    // Base interface-like contract for all signaling messages
    public class PeerSignal
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        // SignalR connectionId of sender
        public PeerMessage Message { get; set; }    // Nested message
    }

    public class PeerMessage
    {
        public string Type { get; set; }            // "offer", "answer", "ice"

        // For offer/answer
        public RtcSessionDescription Sdp { get; set; }

        // For ice
        public RtcIceCandidate Candidate { get; set; }
    }

    // Equivalent to RTCSessionDescriptionInit
    public class RtcSessionDescription
    {
        public string Type { get; set; }            // "offer" | "answer"
        public string Sdp { get; set; }             // Full SDP string
    }

    // Equivalent to RTCIceCandidateInit
    public class RtcIceCandidate
    {
        public string Candidate { get; set; }       // candidate line
        public string SdpMid { get; set; }
        public int? SdpMLineIndex { get; set; }
        public string UsernameFragment { get; set; }
    }
}
