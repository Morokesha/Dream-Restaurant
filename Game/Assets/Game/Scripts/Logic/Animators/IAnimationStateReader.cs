namespace Game.Scripts.Logic.Animators
{
    public interface IAnimationStateReader
    {
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);
    }
}