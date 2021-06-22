using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBang1112.TmXmlRpc
{
    public class MapLeaderBoard : IReadOnlyCollection<MapLeaderBoardPlayer>
    {
        public int R { get; init; }
        public string Context { get; init; }
        public string MapUid { get; init; }
        public string Zone { get; init; }
        public string S { get; init; }
        public int D { get; init; }
        public (int time, int count)[] Times { get; init; }
        public int TotalCount { get; init; }
        public ReadOnlyCollection<MapLeaderBoardPlayer> Records { get; }

        public int Count => Records.Count;

        public MapLeaderBoard(IList<MapLeaderBoardPlayer> records)
        {
            Records = new ReadOnlyCollection<MapLeaderBoardPlayer>(records);
        }

        public MapLeaderBoard() : this(new List<MapLeaderBoardPlayer>())
        {

        }

        public override string ToString()
        {
            if (MapUid == null)
                return "Leaderboard";
            return $"Leaderboard [{MapUid}]";
        }

        public IEnumerator<MapLeaderBoardPlayer> GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Records.GetEnumerator();
        }
    }
}
