using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TmXmlRpc
{
    public class CampaignScoresMapZone
    {
        public short U01;
        public byte U02;
        public byte U03;

        [YamlIgnore]
        public string Zone { get; set; }
        [YamlIgnore]
        public int[] Times { get; set; }
        [YamlIgnore]
        public int[] TimesCount { get; set; }
        public CampaignScoresRecord[] Records { get; set; }
        public int TotalCount { get; set; }

        public override string ToString()
        {
            if (Records.Length > 0)
                return $"WR: {Records[0]} ({TotalCount} records)";
            return base.ToString();
        }

        public IEnumerable<TimeSpan?> GetTimes()
        {
            for (var i = 0; i < TimesCount.Length; i++)
            {
                for (var j = 0; j < TimesCount[i]; j++)
                {
                    var time = Times[i];

                    yield return time >= 0 ? TimeSpan.FromMilliseconds(time) : null;
                }
            }
        }

        public IEnumerable<(int count, TimeSpan? time)> GetGroupedTimes()
        {
            for (var i = 0; i < TimesCount.Length; i++)
            {
                var time = Times[i] >= 0 ? TimeSpan.FromMilliseconds(Times[i]) : default(TimeSpan?);
                var count = TimesCount[i];

                yield return (count, time);
            }
        }

        public IEnumerable<CampaignScoresRecord> GetRecords()
        {
            var counter = 0;
            foreach (var time in GetTimes())
            {
                if (counter < 10)
                {
                    yield return Records[counter];
                }
                else
                {
                    yield return new CampaignScoresRecord() { Time = time };
                }

                counter++;
            }
        }
    }
}