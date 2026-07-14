using UnityEngine;

public interface IState 
{
    void Enter();
    void OnUpdate();
    void OnFixedUpdate();
    void Exit();
}
