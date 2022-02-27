using System.IO.Compression;
using TmEssentials;

namespace TmXmlRpc;

public class CampaignScores
{
    public int U01 = 7;
    public int U02 = 1;

    public string Zone { get; set; }
    public Dictionary<string, CampaignScoresMap> Maps { get; set; }

    public static CampaignScores Parse(Stream gzStream)
    {
        var scores = new CampaignScores();

        using (var gzip = new GZipStream(gzStream, CompressionMode.Decompress))
        using (var r = new ResponseReader(gzip))
        {
            scores.U01 = r.ReadByte();
            scores.U02 = r.ReadByte();
            scores.Zone = r.ReadString();

            r.ReadByte();
            r.ReadByte();
            r.ReadByte();
            var wtf = r.ReadArrayInt32(500);

            var numMaps = r.ReadInt32();
            scores.Maps = new Dictionary<string, CampaignScoresMap>(numMaps);

            for (var i = 0; i < numMaps; i++)
            {
                var map = new CampaignScoresMap
                {
                    MapUid = r.ReadString()
                };

                var zoneCount = r.ReadByte();
                map.Zones = new Dictionary<string, CampaignScoresMapZone>(zoneCount);

                for (var j = 0; j < zoneCount; j++)
                {
                    var zone = r.ReadString();
                    var u01 = r.ReadInt16();
                    var u02 = r.ReadByte();
                    var times = r.ReadArrayInt32();

                    var timesCount = r.ReadArrayInt32(times.Length);
                    var totalCount = timesCount.Sum();

                    var u03 = r.ReadByte();

                    var top10Ranks = r.ReadArrayInt32();
                    var top10Count = top10Ranks.Length;

                    var top10Times = r.ReadArrayInt32(top10Count);

                    var logins = r.ReadArrayString(top10Count);
                    var nicknames = r.ReadArrayString(top10Count);

                    var records = new CampaignScoresRecord[top10Count];

                    for (var k = 0; k < top10Count; k++)
                    {
                        var record = new CampaignScoresRecord
                        {
                            Rank = top10Ranks[k],
                            Time = new TimeInt32(top10Times[k]),
                            Login = logins[k],
                            Nickname = nicknames[k]
                        };

                        records[k] = record;
                    }

                    var mapZone = new CampaignScoresMapZone
                    {
                        Zone = zone,
                        U01 = u01,
                        U02 = u02,
                        Times = times,
                        TimesCount = timesCount,
                        U03 = u03,
                        Records = records,
                        TotalCount = totalCount
                    };

                    map.Zones[zone] = mapZone;
                }

                scores.Maps[map.MapUid] = map;
            }

            var zoneCountOther = r.ReadByte();
            for (var i = 0; i < zoneCountOther; i++)
            {
                var someZone = r.ReadString();
                var what = r.ReadInt16();
                var what2 = r.ReadInt16();

                var somecountdownarray = r.ReadArrayInt32();
                var someshortarray = r.ReadArrayInt32(somecountdownarray.Length);
            }
        }

        return scores;
    }

    public static CampaignScores Parse(byte[] gzData)
    {
        using var ms = new MemoryStream(gzData);
        return Parse(ms);
    }

    public static CampaignScores Parse(string gzFile)
    {
        using var fs = File.OpenRead(gzFile);
        return Parse(fs);
    }
}
