﻿using System;
using UnityEngine;


public class LevelFrame : MonoBehaviour
{
	BoxCollider2D frameCollider;

	void Awake()
	{
		frameCollider = GetComponent<BoxCollider2D>();
	}

	public float GetMinY()
	{
		return transform.position.y + frameCollider.offset.y - frameCollider.size.y * 0.5f;
	}

	public float GetMaxY()
	{
		return transform.position.y + frameCollider.offset.y + frameCollider.size.y * 0.5f;
	}
}

