using System;
using System.Collections.Generic;
using TmEssentials;

namespace TmXmlRpc;

public class CampaignScoresMapZone
{
    public short U01;
    public byte U02;
    public byte U03;

    public string Zone { get; set; }
    public int[] Times { get; set; }
    public int[] TimesCount { get; set; }
    public CampaignScoresRecord[] Records { get; set; }
    public int TotalCount { get; set; }

    public override string ToString()
    {
        if (Records.Length > 0)
            return $"WR: {Records[0]} ({TotalCount} records)";
        return base.ToString() ?? "";
    }

    public IEnumerable<TimeInt32?> GetTimes()
    {
        for (var i = 0; i < TimesCount.Length; i++)
        {
            for (var j = 0; j < TimesCount[i]; j++)
            {
                var time = Times[i];

                yield return time >= 0 ? new TimeInt32(time) : null;
            }
        }
    }

    public IEnumerable<(int count, TimeInt32? time)> GetGroupedTimes()
    {
        for (var i = 0; i < TimesCount.Length; i++)
        {
            var time = Times[i] >= 0 ? new TimeInt32(Times[i]) : default(TimeInt32?);
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
