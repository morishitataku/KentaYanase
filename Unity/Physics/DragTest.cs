using UnityEngine;
using System.Collections;

/// <summary>
/// 空気抵抗テスト
/// 投射するオブジェクトにアタッチしてください 
/// </summary>
[RequireComponent (typeof(Rigidbody))]
public class DragTest : MonoBehaviour {

	//初速度 
	public Vector3 initVelocity;
	//シンプルな抵抗 
	public float simpleDrag;
	//曲線の細かさ 
	public float sampleRate;
	//計算する時間 
	public float time;

	public float iconSize = 5f;

	//初期位置 
	[HideInInspector]
	public Vector3 initPos;

	//未来位置 
	[HideInInspector]
	public Vector3 futurePos;

	void Start () {
		//デフォルトのドラッグは信用できん 
		rigidbody.drag = 0f;
		//初速度を決める 
		rigidbody.velocity = initVelocity;
		//初期位置を保存 
		initPos = transform.position;

		//未来位置 
		futurePos = GetParabolaCurve(
			time,
			initPos,
			initVelocity,
			rigidbody.mass,
			simpleDrag
		);
	}

	void FixedUpdate () {
		rigidbody.AddForce( - rigidbody.velocity * simpleDrag, ForceMode.Force);
	}

	void OnDrawGizmos () {

		if(rigidbody) {
			if(!Application.isPlaying){
				//現在地 
				initPos = transform.position;

				//未来位置 
				futurePos = GetParabolaCurve(
					time,
					initPos,
					initVelocity,
					rigidbody.mass,
					simpleDrag
				);
			}

			Gizmos.color = Color.red;

			//未来位置をWireSphereで描画 
			if(futurePos != Vector3.zero || futurePos != null){
				Gizmos.DrawWireSphere(futurePos, iconSize);
			}

			//軌跡を描画 
			int count = 0;
			while(sampleRate > count){

				Vector3 vec1, vec2;

				vec1 = GetParabolaCurve(
					time/sampleRate * count,
					initPos,
					initVelocity,
					rigidbody.mass,
					simpleDrag
				);
				vec2 = GetParabolaCurve(
					time/sampleRate * (count+1),
					initPos,
					initVelocity,
					rigidbody.mass,
					simpleDrag
				);

				Gizmos.DrawLine(vec1, vec2);
				count ++;
			}
		}
	}


	/// <summary>
	/// t秒後の投射物の未来位置を計算します 
	/// </summary>
	/// <returns> 
	/// 未来位置 
	/// </returns>
	/// <param name="time">
	/// 求めたい時刻
	/// </param>
	/// <param name="initPos">
	/// 対象の初期位置
	/// </param>
	/// <param name="initVec">
	/// 対象の初速度
	/// </param>
	/// <param name="mass">
	/// 対象の質量
	/// </param>
	/// <param name="drag">
	/// 対象の空気抵抗の係数(rigidBody.drag)
	/// </param>
	static public Vector3 GetParabolaCurve (float time, Vector3 initPos, Vector3 initVec, float mass, float drag) {

		float gravity = Physics.gravity.magnitude;
		drag = drag * (0.5f + drag * 0.25f);	

		float horizontalMoveLength, verticalMoveLength, horizontalVecLength, verticalvecLength;
		Vector3 horizontalInitVec, verticalInitVec;

		horizontalInitVec 	= initVec;
		horizontalInitVec.y	= 0f;
		verticalInitVec		= initVec;
		verticalInitVec.x	= 0f;
		verticalInitVec.z	= 0f;

		horizontalVecLength	= Vector3.Magnitude(horizontalInitVec);
		verticalvecLength	= verticalInitVec.y;
		if(drag != 0){
			horizontalMoveLength 	= (mass/drag) * horizontalVecLength * (1f - Mathf.Exp(- (drag * time) / mass));
			verticalMoveLength 		= (mass/drag) * ( (verticalvecLength + ((mass * gravity) / drag) ) * (1f - Mathf.Exp(- (drag * time) / mass)) - gravity * time );
		}else{
			horizontalMoveLength 	= horizontalVecLength  * time;
			verticalMoveLength 		= verticalvecLength * time - 0.5f * gravity * time * time;
		}
		return (initPos + horizontalInitVec.normalized * horizontalMoveLength + Vector3.up * verticalMoveLength);
	}

	void OnValidate ()
	{
		if( rigidbody.drag != 0){
			rigidbody.drag = 0;
			Debug.LogWarning("DragTestコンポーネントのSimpleDragの値が空気抵抗の係数になります");
		}
	}
}
