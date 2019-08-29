
using UnityEngine;

public class Gnome : MonoBehaviour
{
	public enum DamageType
	{
		Slicing,
		Burning
	}



	/// <summary>
	/// Объект, под фокусом камеры
	/// </summary>
	public Transform cameraFollowTarget;

	public Rigidbody2D ropeBody;

	public Sprite armHoldingEmpty;

	public Sprite armHoldingTreasure;

	public SpriteRenderer holdingArm;

	public GameObject deathPrefab;

	public GameObject flameDeathPrefab;

	public GameObject ghostPrefab;

	public float delayBeforeRemoving = 3.0f;

	public float delayBeforeReleasingGhost = 0.25f;

	public GameObject bloodFountainPrefab;

	bool dead = false;

	bool _holdingTreasure = false;

	public bool holdingTreasure
	{
		get
		{
			return _holdingTreasure;
		}
		set
		{
			if (dead) return;

			_holdingTreasure = value;

			if (holdingArm == null) return;

			holdingArm.sprite = _holdingTreasure
				? armHoldingTreasure
				: armHoldingEmpty;
		}
	}

	public void ShowDamageEffect(DamageType type)
	{
		if (type == DamageType.Slicing)
		{
			if (deathPrefab != null)
				Instantiate(deathPrefab,
					cameraFollowTarget.position, cameraFollowTarget.rotation);
			return;
		}

		if (flameDeathPrefab != null)
			Instantiate(flameDeathPrefab,
				cameraFollowTarget.position, cameraFollowTarget.rotation);
	}

	public void DestroyGnome(DamageType type)
	{
		holdingTreasure = false;

		dead = true;

		foreach (var part in GetComponentsInChildren<BodyPart>())
		{
			if (type == DamageType.Burning)
			{
				//Один шанс из трех загореться
				if (Random.Range(0, 2) == 0)
				{
					part.ApplyDamageSprite(type);
				}
			}
			else
			{
				part.ApplyDamageSprite(type);
			}
			//Один шанс из трех отделиться от тела
			if (Random.Range(0, 2) == 0)
			{
				//Обеспечивание удаления твердого тела и коллайдера из части при падении
				part.Detach();

				//При отрубании конечности добавить фонтан крови
				if (type == DamageType.Slicing)
				{
					if (part.bloodFountainOrigin != null && bloodFountainPrefab != null)
					{
						//Присоединение фонтана в место отрубания
						var fountain = Instantiate(bloodFountainPrefab,
							part.bloodFountainOrigin.position, part.bloodFountainOrigin.rotation)
							as GameObject;

						fountain.transform.SetParent(cameraFollowTarget, false);
					}
				}

				foreach (var joint in part.GetComponentsInChildren<Joint2D>())
					Destroy(joint);
			}
		}

		var remove = gameObject.AddComponent<RemoveAfterDelay>();
		remove.delay = delayBeforeRemoving;

		StartCoroutine(ReleaseGhost());
	}

	System.Collections.IEnumerator ReleaseGhost()
	{
		//Шаблон духа не определен = выход
		if (ghostPrefab == null) yield break;

		yield return new WaitForSeconds(delayBeforeReleasingGhost);

		Instantiate(ghostPrefab, transform.position, Quaternion.identity);
	}
}