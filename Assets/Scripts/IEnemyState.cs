public interface IEnemyState
{
    void UpdateState();

    void ToInitialState();

    void ToNormalState();

    void ToChaseState();
}
