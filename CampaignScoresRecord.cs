using System;

namespace BigBang1112.TmXmlRpc
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
                return Time.ToStringTM();
            return $"{Time.ToStringTM()} by {Nickname}";
        }
    }
}