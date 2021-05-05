using TheArchitect.Core.Constants;
using TheArchitect.Core.Data;
using UnityEngine;
using System.Text.RegularExpressions;

namespace TheArchitect.Core
{
    public class ResourceString
    {
        public const string TEXTS = "Texts";
        public const string FLAGS = "Flags";
        public const string ITEMS = "Items";
        public const string RANDOM = "Random";
        private static Regex regexVariable = new Regex(@"\${([^\${}]+)}*");
        private static Regex regexEmphasis = new Regex(@"\*([^\*]+)\**");
        private static Regex regexSmall = new Regex(@"\|([^\*]+)\|*");
        private static Regex regexLineSpaceDiscard = new Regex(@"^[\s]*|\n\s*|[\s]+$");

        public static int ParseToInt(string s)
        {
            string sp = Parse(s);
            int o = 0;
            return int.TryParse(sp, out o)
                ? o : 0;
        }
        public static string Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string finalString = s;

            foreach (Match match in regexVariable.Matches(s))
            {
                if (match.Groups.Count < 2)
                {
                    Debug.LogWarning($"Can't find Regex Group on {match.Value}'");
                    continue;
                }

                string[] path = match.Groups[1].Value.Split(new char[] {'.'});

                if (path[0]==RANDOM)
                {
                    finalString = finalString.Replace(
                        match.Value,
                        $"{(UnityEngine.Random.value * 1000000):0000000}"
                    );
                }
                else if (path[0]==FLAGS)
                {
                    Game game = Resources.Load<Game>(ResourcePaths.SO_GAME);
                    finalString = finalString.Replace(
                        match.Value,
                        game.GetFlagState(path[1]).ToString()
                    );
                }
                else if (path[0]==TEXTS)
                {
                    Game game = Resources.Load<Game>(ResourcePaths.SO_GAME);
                    finalString = finalString.Replace(
                        match.Value,
                        game.GetTextState(path[1])
                    );
                }
                else if (path[0]==ITEMS && path.Length == 2)
                {
                    int value = 0;
                    Game game = Resources.Load<Game>(ResourcePaths.SO_GAME);
                    Item item = Resources.Load<Item>($"{ResourcePaths.SO_ITEMS}/{path[1]}");
                    if (item==null) 
                    {
                        Debug.LogWarning($"Item not found {path[1]}");
                        break;
                    }

                    value = game.GetItemCount(item);
                    finalString = finalString.Replace(match.Value, value.ToString());
                }
                else
                {
                    ScriptableObject o = Resources.Load<ScriptableObject>($"ScriptableObjects/{path[0]}/{path[1]}");
                    if (o == null)
                    {
                        Debug.LogWarning($"Can't load 'ScriptableObjects/{path[0]}/{path[1]}'");
                        continue;
                    }

                    var property = o.GetType().GetProperty(path[2]);
                    if (property == null)
                    {
                        Debug.LogWarning($"Can't find property '{path[2]}' in '{o.GetType().FullName}'");
                        continue;
                    }

                    finalString = finalString.Replace(
                        match.Value,
                        o.GetType().GetProperty(path[2]).GetValue(o).ToString()
                    );
                }

            }

            foreach (Match match in regexEmphasis.Matches(finalString))
            {
                if (match.Groups.Count > 1)
                {
                    finalString = finalString.Replace(
                        match.Value,
                        $"<color=cyan>{match.Groups[1].Value}</color>"
                    );
                }
            }
            
            foreach (Match match in regexSmall.Matches(finalString))
            {
                if (match.Groups.Count > 1)
                {
                    finalString = finalString.Replace(
                        match.Value,
                        $"<i>{match.Groups[1].Value}</i>"
                    );
                }
            }

            return regexLineSpaceDiscard.Replace(finalString, "")
                .Replace("\\n", "\n");
        }
    }
}