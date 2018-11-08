using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer)), ExecuteInEditMode]
public class Hint : MonoBehaviour
{
	[Header("Set by yourself")]
	public string HintText = "No hints applied";
	public Text TextField;
	[Header("Don't touch if you're not a developer")]
	public Material material;
	public Transform target;
	private LineRenderer _lineRenderer;
	
	public Vector3 _offset;
	
	void Start ()
	{
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.positionCount = 2;
		_lineRenderer.SetWidth(0.001f, 0.001f);
		_lineRenderer.material = material;
		////////////////////////////
		TextField.text = HintText;
	}
	
	void Update () 
	{
		if (target == null)
		{
			return;
		}

        TextField.text = HintText;
        _lineRenderer.SetPosition(0, transform.position);
		_lineRenderer.SetPosition(1, target.position + _offset * 0.01f);
	}
}
