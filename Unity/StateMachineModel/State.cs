using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class State
{
	public delegate void ExecuteMethod ();
	public delegate State DecideMethod ();
	public delegate void EnterMethod ();
	public delegate void ExitMethod ();

	//Stateが受け持つ処理
	ExecuteMethod executeMethod;
	//Stateの遷移判断処理
	DecideMethod decideMethod;
	//Stateが選ばれたときの処理
	EnterMethod enterMethod;
	//Stateが離れたときの処理
	ExitMethod exitMethod;

	public State (ExecuteMethod executeMethod, DecideMethod decideMethod, EnterMethod enterMethod, ExitMethod exitMethod)
	{
		this.executeMethod	= executeMethod;
		this.decideMethod	= decideMethod;
		this.enterMethod	= enterMethod;
		this.exitMethod = exitMethod;
	}

	public void Execute ()
	{
		if (executeMethod != null)
			executeMethod ();
	}

	public State Decide ()
	{
		if (decideMethod != null) {
			return decideMethod ();
		} else {
			return null;
		}
	}

	public void Enter ()
	{
		if (enterMethod != null)
			enterMethod ();
	}

	public void Exit ()
	{
		if (exitMethod != null)
			exitMethod ();
	}
}