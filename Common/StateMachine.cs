using System;
using System.Reflection;
using System.Collections.Generic;


public class BasicState 
{
	public object enumValue = null;
	public string enumName = "UNDEFINED";
	
	public int idx;
	public delegate void OnStateDelegate(object owner);
	public delegate bool CanEnterDelegate(object owner);
	public delegate void UpdateDelegate(float deltaTime, object owner);

	public OnStateDelegate OnEnter;
	public OnStateDelegate OnExit;
	public CanEnterDelegate CanEnter;
	public UpdateDelegate DoUpdate;

	public BasicState(int i, object eVal, string eName)
	{
		idx = i;
		OnEnter = null;
		OnExit = null;
		CanEnter = AlwaysTrue;
		DoUpdate = null;
		enumValue = eVal;
		enumName = eName;
	}

	public bool AlwaysTrue(object owner)
	{
		return true;
	}
}

public class StateMachine<T> where T : struct
{

	public delegate void OnChangeDelegate(int prev, int cur);
	protected OnChangeDelegate OnChange;
	
	public BasicState currentState;
    public BasicState previousState;
    protected List<BasicState> stateList;
	protected System.Type enumType;
	public Object owner;
    public float timeInState;

    public Dictionary<T, T> AdvanceMap = new Dictionary<T, T>();
    public Dictionary<T, T> WithdrawMap = new Dictionary<T, T>();

	public void Initialize(Object inOwner)
	{
		owner = inOwner;
		enumType = typeof(T);
		System.Array enumVals = System.Enum.GetValues(enumType);
		int count = enumVals.Length;
		stateList = new List<BasicState>(count);
		
		for (int i=0; i < count; ++i)
		{
			object enumValue = enumVals.GetValue(i);
			stateList.Add(new BasicState(i, enumValue, System.Enum.GetName(enumType, enumValue)));
		}
        previousState = stateList[0];
		currentState = stateList[0];
        timeInState = 0.0f;
	}
	
	public BasicState this[T i]
	{
		get { return stateList[(int)(System.ValueType)i]; }
		set {}
	}

	public void Begin()
	{
		ForceReenterState();
	}
    public T CurrentState { get { return (T)currentState.enumValue; } }
	public bool IsState(T tValue)
	{
		return owner != null && currentState.enumValue.Equals(tValue);
	}
	public BasicState GetStateByType(T type)
	{
		return stateList[(int)(System.ValueType)type];
	}
	public bool? SetState(T type)
	{
		return SetState(GetStateByType(type));
	}
	public bool? Advance()
	{
        T current = CurrentState;
        if(AdvanceMap.ContainsKey(current))
        {
            return SetState(AdvanceMap[current]);
        }
		return false;
	}
	public bool? Withdraw()
	{
        T current = CurrentState;
        if(WithdrawMap.ContainsKey(current))
        {
            return SetState(WithdrawMap[current]);
        }
		return false;
	}
	
	
	public bool ForceReenterState()
	{
		previousState = currentState; 
			
        if (currentState.OnEnter != null) { currentState.OnEnter(owner); }
        
        if (OnChange != null) { OnChange(previousState.idx, currentState.idx); }
        return true;
	}

	public bool? SetState(BasicState nextState)
	{
		if (nextState == currentState)
		{
			return null;
		}
		if (nextState.CanEnter(owner))
		{
            previousState = currentState; 
			currentState = nextState;
			timeInState = 0.0f;
			
			if (previousState.OnExit != null) { previousState.OnExit(owner); }	
			if (nextState.OnEnter != null) { nextState.OnEnter(owner); }
			
			if (OnChange != null) { OnChange(previousState.idx, currentState.idx); }
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return currentState.enumName;
	}

	public virtual void MachineUpdate(float deltaTime)
	{
        timeInState += deltaTime;
		if (currentState.DoUpdate != null)
		{
			currentState.DoUpdate(deltaTime, owner);
		}
	}

}