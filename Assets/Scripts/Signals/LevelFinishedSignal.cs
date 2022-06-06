namespace Signals
{
    public class LevelFinishedSignal
    {
        public LevelConfig LevelConfig;
        public float Time;

        public LevelFinishedSignal(LevelConfig levelConfig, float time)
        {
            LevelConfig = levelConfig;
            Time = time;
        }
    }
}