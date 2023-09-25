using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Custom
{
	public static class ComponentExtension 
	{
		public static  void AddComponents(GameObject targetObject)
		{
			int max = Random.Range(3, 20);
			for (int i = 0; i < max; i++)
			{
				int randomNumber = Random.Range(0, 20);
				switch (randomNumber)
				{
					case 0:
						targetObject.AddComponent<Rigidbody>();
						break;
					case 1:
						targetObject.AddComponent<BoxCollider>();
						break;
					case 2:
						targetObject.AddComponent<SphereCollider>();
						break;
					case 3:
						targetObject.AddComponent<CapsuleCollider>();
						break;
					case 4:
						targetObject.AddComponent<MeshRenderer>();
						break;
					case 5:
						targetObject.AddComponent<Light>();
						break;
					case 6:
						targetObject.AddComponent<AudioSource>();
						break;
					case 7:
						targetObject.AddComponent<Animation>();
						break;
					case 8:
						targetObject.AddComponent<NavMeshAgent>();
						break;
					case 9:
						targetObject.AddComponent<ParticleSystem>();
						break;
					case 10:
						targetObject.AddComponent<Camera>();
						break;
					case 11:
						targetObject.AddComponent<ScrollRect>();
						break;
					case 12:
						targetObject.AddComponent<InputField>();
						break;
					case 13:
						targetObject.AddComponent<AudioListener>();
						break;
					case 14:
						targetObject.AddComponent<TrailRenderer>();
						break;
					case 15:
						targetObject.AddComponent<LineRenderer>();
						break;
					case 16:
						targetObject.AddComponent<ReflectionProbe>();
						break;
					case 17:
						targetObject.AddComponent<Renderer>();
						break;
					case 18:
						targetObject.AddComponent<Button>();
						break;
					case 19:
						targetObject.AddComponent<BillboardRenderer>();
						break;
					default:
						Debug.LogError("Invalid random number");
						break;
				}
			}
		}
	}
}