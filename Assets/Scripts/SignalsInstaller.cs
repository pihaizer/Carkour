using Signals;
using Zenject;

public class SignalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<LevelFinishedSignal>();
        Container.DeclareSignal<RestartLevelSignal>();
        Container.DeclareSignal<ExitLevelSignal>();
    }
}