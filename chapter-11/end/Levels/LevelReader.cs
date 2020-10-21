using chapter_11.Engine.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace chapter_11.Levels
{
    public class LevelReader
    {
        private int _viewportWidth;

        private const int NB_ROWS = 11;
        private const int NB_TILE_ROWS = 10;

        public LevelReader(int viewportWidth)
        {
            _viewportWidth = viewportWidth;
        }

        public List<List<BaseGameStateEvent>> LoadLevel(int nb)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.FullName.Split(',')[0].Replace('-', '_');
            var fileName = $"{assemblyName}.Levels.LevelData.Level{nb}.txt";

            var stream = assembly.GetManifestResourceStream(fileName);

            string levelString;
            using (var reader = new StreamReader(stream))
            {
                levelString = reader.ReadToEnd();
            }

            if (levelString != null && levelString.Length > 0)
            {
                var rows = levelString.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var convertedRows = from r in rows
                                    select ToEventRow(r);

                return convertedRows.Reverse().ToList();
            }
            else
            {
                return new List<List<BaseGameStateEvent>>();
            }
        }

        private List<BaseGameStateEvent> ToEventRow(string rowString)
        {
            var elements = rowString.Split(',');

            var newRow = new List<BaseGameStateEvent>();
            for (int i = 0; i < NB_ROWS; i++)
            {
                newRow.Add(ToEvent(i, elements[i]));
            }

            return newRow;
        }

        private BaseGameStateEvent ToEvent(int elementNumber, string input)
        {
            switch (input) 
            {
                case "0":
                    return new BaseGameStateEvent.Nothing();
                    
                case "_":
                    return new LevelEvents.NoRowEvent();

                case "1":
                    var xPosition = elementNumber * _viewportWidth / NB_TILE_ROWS;
                    return new LevelEvents.GenerateTurret(xPosition);

                case "s":
                    return new LevelEvents.StartLevel();

                case "e":
                    return new LevelEvents.EndLevel();

                case string g when g.StartsWith("g"):
                    var nb = int.Parse(g.Substring(1));
                    return new LevelEvents.GenerateEnemies(nb);

                default:
                    return new BaseGameStateEvent.Nothing();
            }
        }
    }
}
