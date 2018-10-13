using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Decision
{
	public abstract bool Decide(StateController c);
}

/// <summary>
/// AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
/// yeah so this is fucked
/// Basically, I want to be able to serialize children of the decide class and have their fields baked into the state asset
/// Since unity doesn't handle serializing polymorphism (or actually it kinda does in scriptableObject with some fuckery????)
/// i'm instead serializing a factory class that'll just pump out which ever child class that you want.
/// only downside of this is we can't serialize child fields so everything needs to use the fields from the factory which is
/// H O T  G A R B A G E
/// so yeah I've already spent like 2 hours on this one thing and don't really wanna waste anymore time on this I want to die
/// you might think this this comment is to help you understand what's happening here but really it's to help me sleep better at night
/// since this code is so garbage thanks for listening 
/// </summary>

[Serializable]
public class DecisionFactory
{
	public enum DecisionType
	{
		RANDOM_TIMED,
		TIMED,
		TARGET_NEAR,
		TARGET_FAR,
		ON_HIT,
	}

	public float float1;
	public float float2;
	public float float3;
	
	public DecisionType eDecision;
	
	public Decision GetDecision()
	{
		switch (eDecision)
		{
			case DecisionType.RANDOM_TIMED:
				return TimedDecisionMaker(true);
				break;
			case DecisionType.TIMED:
				return TimedDecisionMaker(false);
				break;
			case DecisionType.TARGET_NEAR:
				return DistanceDecision(false);
				break;
			case DecisionType.TARGET_FAR:
				return DistanceDecision(true);
				break;
			case DecisionType.ON_HIT:
				return new HitDecision();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private Decision TimedDecisionMaker(bool random)
	{
		return new TimedDecision {randomize = random, TimeInState = float1, VariationAmount = float2};		
	}

	private TargetDistanceDecision DistanceDecision(bool flip)
	{
		return new TargetDistanceDecision {Distance = float1, Flip = flip};		
	}
}
