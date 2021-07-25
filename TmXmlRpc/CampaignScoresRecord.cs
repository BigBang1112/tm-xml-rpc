using System;
using TmEssentials;

namespace TmXmlRpc
{
    public struct CampaignScoresRecord
    {
        public int Rank { get; set; }
        public TimeSpan? Time { get; set; }
        public string Login { get; set; }
        public string Nickname { get; set; }

        public override string ToString()
        {
            if (Nickname == null)
                return Time.ToStringTm();
            return $"{Time.ToStringTm()} by {Formatter.Deformat(Nickname)}";
        }
    }
}