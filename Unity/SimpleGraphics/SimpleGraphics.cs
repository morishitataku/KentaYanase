using UnityEngine;
using System.Collections;

static public class SimpleGraphics{

	public enum RenderModeLine3DEnum {LOOKCAMERA = 0, VERTICAL, HORIZONTAL};
	
	static private Shader _shaderAlphaBlended;
	static private Shader _shaderAdditive;
	static private Shader _shaderMultiply;

	static private Material _materialAlphaBlended;
	static private Material _materialAdditive;
	static private Material _materialMultiply;
	
	static public Shader ShaderAlphaBlended { get { return _shaderAlphaBlended; } }
	static public Shader ShaderAdditive { get { return _shaderAdditive; } }
	static public Shader ShaderMultiply { get { return _shaderMultiply; } }

	static public Material MaterialAlphaBlended { get { return _materialAlphaBlended; } }
	static public Material MaterialAdditive { get { return _materialAdditive; } }
	static public Material MaterialMultiply { get { return _materialMultiply; } }
	
	/// <summary>
	/// コンストラクタ
	/// シェーダーの読み込みと基本マテリアルの作成
	/// </summary>
	static SimpleGraphics () {
		_shaderAlphaBlended = Shader.Find ("Particles/Alpha Blended");
		_shaderAdditive 	= Shader.Find ("Particles/Additive");
		_shaderMultiply 	= Shader.Find ("Particles/Multiply");
		
		_materialAlphaBlended	 = new Material(_shaderAlphaBlended);
		_materialAdditive = new Material(_shaderAdditive);
		_materialMultiply = new Material(_shaderMultiply);
	}

	/// <summary>
	/// スクリーン座標系で線を描画します
	/// Graphics.DrawMeshを使用しているためUpdate関数内で使用します
	/// </summary>
	/// <param name='start'>
	/// 線の開始点 画面左上が(0,0)
	/// </param>
	/// <param name='end'>
	/// 線の終点 画面左上が(0,0)
	/// </param>
	/// <param name='lineSize'>
	/// 線の太さ
	/// </param>
	/// <param name='color'>
	/// 線の色 指定しなければ Color(0f,1f,0f,0.2f)
	/// </param>
	/// <param name='mat'>
	/// 線の材質　指定しなければ[Particles/Alpha Blended]シェーダーを使用
	/// </param>
	/// <param name='cam'>
	/// 描画対象のカメラ　指定しなければCamera.mainを使用
	/// </param>
	/// <param name='layer = 0'>
	/// 描画対象のレイヤー 
	/// </param>
	/// <param name='distance = 1f'>
	/// カメラからの距離
	/// </param>
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, new Color(0f,1f,0f,0.2f), _materialAlphaBlended, Camera.main, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Color color, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, color, _materialAlphaBlended, Camera.main, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Material mat, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, new Color(0f,1f,0f,0.2f), mat, Camera.main, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Color color, Material mat, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, color, mat, Camera.main, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Camera cam, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, new Color(0f,1f,0f,0.2f), _materialAlphaBlended, cam, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Color color, Camera cam, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, color, _materialAlphaBlended, cam, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Material mat, Camera cam, int layer = 0, float distance = 1f) {
		DrawLine2D(start, end, lineSize, new Color(0f,1f,0f,0.2f), mat, cam, layer, distance);
	}
	static public void DrawLine2D (Vector2 start, Vector2 end, float lineSize, Color color, Material mat, Camera cam, int layer = 0, float distance = 1f) {
		Quaternion rot = Quaternion.identity;
		mat.SetColor("_TintColor", color);
		
		Vector3 startScreenPos = new Vector3(start.x, Screen.height-start.y, distance);
		Vector3 endScreenPos = new Vector3(end.x, Screen.height-end.y, distance);
		Vector3 crossLine = new Vector3((startScreenPos-endScreenPos).x*Mathf.Cos(Mathf.PI*0.5f) - (startScreenPos-endScreenPos).y*Mathf.Sin(Mathf.PI*0.5f)
									, (startScreenPos-endScreenPos).x*Mathf.Sin(Mathf.PI*0.5f) + (startScreenPos-endScreenPos).y*Mathf.Cos(Mathf.PI*0.5f)
									, 0f).normalized * 0.5f;
		Vector3 meshStartPos = Camera.main.ScreenToWorldPoint(startScreenPos);

		Mesh meshLine2D = new Mesh();
        meshLine2D.name = "Panel";
		
        meshLine2D.vertices = new Vector3[]{
			Camera.main.ScreenToWorldPoint(startScreenPos + crossLine * lineSize) - meshStartPos,
			Camera.main.ScreenToWorldPoint(startScreenPos - crossLine * lineSize) - meshStartPos,
			Camera.main.ScreenToWorldPoint(endScreenPos - crossLine * lineSize) - meshStartPos,
			Camera.main.ScreenToWorldPoint(endScreenPos + crossLine * lineSize) - meshStartPos,
        };
		meshLine2D.triangles = new int[]{
		    0, 1, 2, 2, 3, 0
		};
		meshLine2D.uv = new Vector2[]{
		    new Vector2(0f, 1f),
		    new Vector2(1f, 1f),
		    new Vector2(1f, 0f),
		    new Vector2(0f, 0f)
		};
		
		meshLine2D.RecalculateNormals();
		meshLine2D.RecalculateBounds();
		meshLine2D.Optimize();
		meshLine2D.MarkDynamic ();
		
		Graphics.DrawMesh(meshLine2D, meshStartPos, rot, mat, layer, cam);
		MonoBehaviour.Destroy(meshLine2D, 0.01f);
	}
	
	/// <summary>
	/// ワールド座標系で線を描画します
	/// Graphics.DrawMeshを使用しているためUpdate関数内で使用します
	/// </summary>
	/// <param name='start'>
	/// 線の開始点 
	/// </param>
	/// <param name='end'>
	/// 線の終点 
	/// </param>
	/// <param name='lineSize'>
	/// 線の太さ
	/// </param>
	/// <param name='color'>
	/// 線の色 指定しなければ Color(0f,1f,0f,0.2f)
	/// </param>
	/// <param name='mat'>
	/// 線の材質　指定しなければ[Particles/Alpha Blended]シェーダーを使用
	/// </param>
	/// <param name='cam'>
	/// 描画対象のカメラ　指定しなければCamera.mainを使用
	/// </param>
	/// <param name='layer = 0'>
	/// 描画対象のレイヤー 
	/// </param>
	/// <param name='mode'>
	/// Mode
	/// </param>
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, new Color(0f,1f,0f,0.2f), _materialAlphaBlended, Camera.main, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Color color, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, color, _materialAlphaBlended, Camera.main, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Material mat, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, new Color(0f,1f,0f,0.2f), mat, Camera.main, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Color color, Material mat, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, color, mat, Camera.main, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Camera cam, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, new Color(0f,1f,0f,0.2f), _materialAlphaBlended, cam, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Color color, Camera cam, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, color, _materialAlphaBlended, cam, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Material mat, Camera cam, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		DrawLine3D(start, end, lineSize, new Color(0f,1f,0f,0.2f), mat, cam, layer, mode);
	}
	static public void DrawLine3D (Vector3 start, Vector3 end, float lineSize, Color color, Material mat, Camera cam, int layer = 0, RenderModeLine3DEnum mode = RenderModeLine3DEnum.LOOKCAMERA) {
		Quaternion rot = Quaternion.identity;
		mat.SetColor("_TintColor", color);
		
		Vector3 crossLine = Vector3.zero;
		
		Mesh meshLine3D = new Mesh();
        meshLine3D.name = "Panel";
		
		if(mode == RenderModeLine3DEnum.LOOKCAMERA){
			crossLine = Vector3.Cross(end-start, (end+start)*0.5f-cam.transform.position).normalized * 0.5f;
		}
		if(mode == RenderModeLine3DEnum.HORIZONTAL){
			crossLine = new Vector3((start-end).x*Mathf.Cos(Mathf.PI*0.5f) - (start-end).z*Mathf.Sin(Mathf.PI*0.5f)
									, 0f
									, (start-end).x*Mathf.Sin(Mathf.PI*0.5f) + (start-end).z*Mathf.Cos(Mathf.PI*0.5f)
								).normalized * 0.5f;
		}
		if(mode == RenderModeLine3DEnum.VERTICAL){
			crossLine = Vector3.up * 0.5f;
		}
		
        meshLine3D.vertices = new Vector3[]{
			crossLine * lineSize,
			- crossLine * lineSize,
			(end - crossLine * lineSize) - start,
			(end + crossLine * lineSize) - start,
        };
		meshLine3D.triangles = new int[]{
		    0, 1, 2, 2, 3, 0
		};
		meshLine3D.uv = new Vector2[]{
		    new Vector2(0f, 1f),
		    new Vector2(1f, 1f),
		    new Vector2(1f, 0f),
		    new Vector2(0f, 0f)
		};
		
		meshLine3D.RecalculateNormals();
		meshLine3D.RecalculateBounds();
		meshLine3D.Optimize();
		meshLine3D.MarkDynamic ();
		
		Graphics.DrawMesh(meshLine3D, start, rot, mat, layer, cam);
		MonoBehaviour.Destroy(meshLine3D, 0.01f);
	}
}
