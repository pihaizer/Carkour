namespace Signals
{
    public class RestartLevelSignal
    {
        public bool IsFromBottomCollider = false;

        public RestartLevelSignal()
        {
        }

        public RestartLevelSignal(bool isFromBottomCollider)
        {
            IsFromBottomCollider = isFromBottomCollider;
        }
    }
}