using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StateMachine<T>
{
	Dictionary<T, State> stateMachineTable = new Dictionary<T, State> ();
	State currentState;
	//================================================================================
	//コンストラクタ
	public StateMachine (Dictionary<T, State> stateMachineTable, int defaultStateIndex) : this (stateMachineTable)
	{
		if (defaultStateIndex < stateMachineTable.Keys.Count) {
			currentState = stateMachineTable.Values.ElementAt (defaultStateIndex);
		} else {
			Debug.LogError ("stateMachineTableに存在しないデフォルトStateインデックスです");
		}
	}

	public StateMachine (Dictionary<T, State> stateMachineTable, T defaultStateID) : this (stateMachineTable)
	{
		if (this.stateMachineTable.Keys.Contains (defaultStateID)) {
			currentState = this.stateMachineTable [defaultStateID];
		} else {
			Debug.LogError ("stateMachineTableに存在しないデフォルトStateです");
		}
	}

	public StateMachine (Dictionary<T, State> stateMachineTable)
	{
		foreach (KeyValuePair<T, State> pair in stateMachineTable) {
			AddState (pair);
		}
	}
	//================================================================================
	//現在のStateを取得します
	public State CurrentState {
		get {
			if (currentState == null) {
				Debug.LogError ("現在のStateがありません");
			}
			return currentState;
		}
	}
	//================================================================================
	//Stateに登録した処理を実行します
	public void Execute ()
	{
		if (currentState == null) {
			Debug.LogError ("現在のStateがありません");
		} else {
			currentState.Execute ();
		}
	}
	//Stateを変遷処理を行います
	public State UpdateState ()
	{
		State tempState = null;
		if (currentState == null) {
			Debug.LogError ("現在のStateがありません");
		} else {
			tempState = currentState.Decide ();
		}
		if (tempState != currentState) {
			if (currentState != null)
				currentState.Exit ();
			if (tempState != null)
				tempState.Enter ();
			currentState = tempState;
		}
		return currentState;
	}
	//================================================================================
	//別のStateに切り替えます
	public void ChangeState (int index)
	{
		if (index < stateMachineTable.Keys.Count) {
			currentState = stateMachineTable.Values.ElementAt (index);
		} else {
			Debug.LogError ("stateMachineTableに存在しないStateインデックスです");
		}
	}

	public void ChangeState (T stateID)
	{
		if (!stateMachineTable.Keys.Contains (stateID)) {
			currentState = stateMachineTable [stateID];
		} else {
			Debug.LogError ("stateMachineTableに存在しないStateです");
		}
	}
	//Stateを取得します
	public State GetState (int index)
	{
		if (index < stateMachineTable.Keys.Count) {
			return stateMachineTable.Values.ElementAt (index);
		}
		return null;
	}

	public State GetState (T stateID)
	{
		if (stateMachineTable.Keys.Contains (stateID)) {
			return stateMachineTable [stateID];
		}
		return null;
	}

	public void AddState (KeyValuePair<T, State> pair)
	{
		if (!stateMachineTable.Keys.Contains (pair.Key)) {
			stateMachineTable.Add (pair.Key, pair.Value);
		}
	}

	public void RemoveState (int removeIndex)
	{
		if (stateMachineTable.Keys.Count > removeIndex) {
			stateMachineTable.Remove (stateMachineTable.Keys.ElementAt (removeIndex));
		}
	}

	public void RemoveState (T stateID)
	{
		if (stateMachineTable.Keys.Contains (stateID)) {
			stateMachineTable.Remove (stateID);
		}
	}

	public int CountState ()
	{

		return 0;
	}

	public bool IsExistState (string name)
	{

		return false;
	}
}
