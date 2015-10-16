using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

[RequireComponent(typeof(Animator))]
public class AvatarController : MonoBehaviour
{	
	// Bool that has the characters (facing the player) actions become mirrored. Default false.
	public bool mirroredMovement = false;

	// Slerp smooth factor
	public float smoothFactor = 5f;

	// The body root node
	protected Transform bodyRoot;

	// Avatar's offset/parent object that may be used to rotate and position the model in space.
	protected GameObject offsetNode;

	// Variable to hold all them bones. It will initialize the same size as initialRotations.
	protected Transform[] bones;
	
	// Rotations of the bones when the Kinect tracking starts.
	protected Quaternion[] initialRotations;
	protected Quaternion[] initialLocalRotations;

	// Initial position and rotation of the transform
	protected Vector3 initialPosition;
	protected Quaternion initialRotation;

	// transform caching gives performance boost since Unity calls GetComponent<Transform>() each time you call transform 
	private Transform _transformCache;
	public new Transform transform
	{
		get
		{
			if (!_transformCache) 
				_transformCache = base.transform;

			return _transformCache;
		}
	}

	static MotionMagixSimulator.Model.Joints joints = new MotionMagixSimulator.Model.Joints();
	
	private Quaternion[] rotations;

	public void Awake()
    {	
		rotations = new Quaternion[15];
		MMData.SkeletonDataReceived += HandleSkeletonDataReceived;
		// check for double start
		if(bones != null)
			return;

		// inits the bones array
		bones = new Transform[27];
		
		// Initial rotations and directions of the bones.
		initialRotations = new Quaternion[bones.Length];
		initialLocalRotations = new Quaternion[bones.Length];

		// Map bones to the points the Kinect tracks
		MapBones();

		// Get initial bone rotations
		GetInitialRotations();
	}

	void HandleSkeletonDataReceived (object sender, SkeletonEventArgs e)
	{
		try
		{
			joints = e.Skeleton.UserObject.CurrentUser.SketelonJoints;
			for (int i = 0; i < 15; i++) 
			{
				switch (i) 
				{
				case 0:rotations[i] = new Quaternion((float)joints.Head.OrientX,(float)joints.Head.OrientY,(float)joints.Head.OrientZ,(float)joints.Head.OrientW);
					break;
				case 1:rotations[i] = new Quaternion((float)joints.Neck.OrientX,(float)joints.Neck.OrientY,(float)joints.Neck.OrientZ,(float)joints.Neck.OrientW);
					break;
				case 2:rotations[i] = new Quaternion((float)joints.LeftShoulder.OrientX,(float)joints.LeftShoulder.OrientY,(float)joints.LeftShoulder.OrientZ,(float)joints.LeftShoulder.OrientW);
					break;
                case 3:rotations[i] = new Quaternion((float)joints.RightShoulder.OrientX,(float)joints.RightShoulder.OrientY,(float)joints.RightShoulder.OrientZ,(float)joints.RightShoulder.OrientW);
					break;
				case 4:rotations[i] = new Quaternion((float)joints.LeftElbow.OrientX,(float)joints.LeftElbow.OrientY,(float)joints.LeftElbow.OrientZ,(float)joints.LeftElbow.OrientW);
					break;
				case 5:rotations[i] = new Quaternion((float)joints.RightElbow.OrientX,(float)joints.RightElbow.OrientY,(float)joints.RightElbow.OrientZ,(float)joints.RightElbow.OrientW);
					break;
				case 6:rotations[i] = new Quaternion((float)joints.LeftHand.OrientX,(float)joints.LeftHand.OrientY,(float)joints.LeftHand.OrientZ,(float)joints.LeftHand.OrientW);
					break;
				case 7:rotations[i] = new Quaternion((float)joints.RightHand.OrientX,(float)joints.RightHand.OrientY,(float)joints.RightHand.OrientZ,(float)joints.RightHand.OrientW);
					break;
				case 8:rotations[i] = new Quaternion((float)joints.Torso.OrientX,(float)joints.Torso.OrientY,(float)joints.Torso.OrientZ,(float)joints.Torso.OrientW);
					break;
				case 9:rotations[i] = new Quaternion((float)joints.LeftHip.OrientX,(float)joints.LeftHip.OrientY,(float)joints.LeftHip.OrientZ,(float)joints.LeftHip.OrientW);
					break;
				case 10:rotations[i] = new Quaternion((float)joints.RightHip.OrientX,(float)joints.RightHip.OrientY,(float)joints.RightHip.OrientZ,(float)joints.RightHip.OrientW);
					break;
				case 11:rotations[i] = new Quaternion((float)joints.LeftKnee.OrientX,(float)joints.LeftKnee.OrientY,(float)joints.LeftKnee.OrientZ,(float)joints.LeftKnee.OrientW);
					break;
				case 12:rotations[i] = new Quaternion((float)joints.RightKnee.OrientX,(float)joints.RightKnee.OrientY,(float)joints.RightKnee.OrientZ,(float)joints.RightKnee.OrientW);
					break;
				case 13:rotations[i] = new Quaternion((float)joints.LeftFoot.OrientX,(float)joints.LeftFoot.OrientY,(float)joints.LeftFoot.OrientZ,(float)joints.LeftFoot.OrientW);
					break;
				case 14:rotations[i] = new Quaternion((float)joints.RightFoot.OrientX,(float)joints.RightFoot.OrientY,(float)joints.RightFoot.OrientZ,(float)joints.RightFoot.OrientW);
					break;
				default:
					break;
				}
			}
		}
		catch(Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	void Update()
	{
		UpdateAvatar (0);
	}

	// Update the avatar each frame.
    public void UpdateAvatar(Int64 UserID)
    {	
		if(!transform.gameObject.activeInHierarchy || joints == null) 
			return;


		for (var boneIndex = 0; boneIndex < bones.Length; boneIndex++)
		{
			if (!bones[boneIndex]) 
				continue;

			if(boneIndexToJointMap.ContainsKey(boneIndex))
			{
				Quaternion joint = !mirroredMovement ? rotations[boneIndexToJointMap[boneIndex]] : rotations[boneIndexToJointMapMirrored[boneIndex]];

				TransformBone(UserID, joint, boneIndex);
			}
		}
	}

	// Apply the rotations tracked by kinect to the joints.
	protected void TransformBone(Int64 userId, Quaternion joint, int boneIndex)
    {
		Transform boneTransform = bones[boneIndex];
		if(boneTransform == null)
			return;

		// Get Kinect joint orientation
		Quaternion jointRotation = joint;
		if(jointRotation == Quaternion.identity)
			return;

		Vector3 mirroredAngles = jointRotation.eulerAngles;
		mirroredAngles.x = -mirroredAngles.x;

		if (!mirroredMovement) 
		{
			mirroredAngles.y = -mirroredAngles.y;
		}
		else 
		{
			mirroredAngles.z = -mirroredAngles.z;
		}

		jointRotation = Quaternion.Euler (mirroredAngles);
		// Smoothly transition to the new rotation
		Quaternion newRotation = Kinect2AvatarRot(jointRotation, boneIndex);
		
		if(smoothFactor != 0f)
        	boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, smoothFactor * Time.deltaTime);
		else
			boneTransform.rotation = newRotation;

	}

	// If the bones to be mapped have been declared, map that bone to the model.
	protected virtual void MapBones()
	{
		// make OffsetNode as a parent of model transform.
		offsetNode = new GameObject(name + "Ctrl") { layer = transform.gameObject.layer, tag = transform.gameObject.tag };
		offsetNode.transform.position = transform.position;
		offsetNode.transform.rotation = transform.rotation;
		offsetNode.transform.parent = transform.parent;
		
		// take model transform as body root
		transform.parent = offsetNode.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		
		bodyRoot = transform;

		// get bone transforms from the animator component
		var animatorComponent = GetComponent<Animator>();
		
		for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
		{
			if (!boneIndex2MecanimMap.ContainsKey(boneIndex)) 
				continue;
			
			bones[boneIndex] = animatorComponent.GetBoneTransform(boneIndex2MecanimMap[boneIndex]);
		}
	}
	
	// Capture the initial rotations of the bones
	protected void GetInitialRotations()
	{
		// save the initial rotation
		if(offsetNode != null)
		{
			initialPosition = offsetNode.transform.position;
			initialRotation = offsetNode.transform.rotation;

			offsetNode.transform.rotation = Quaternion.identity;
		}
		else
		{
			initialPosition = transform.position;
			initialRotation = transform.rotation;

			transform.rotation = Quaternion.identity;
		}

		for (int i = 0; i < bones.Length; i++)
		{
			if (bones[i] != null)
			{
				initialRotations[i] = bones[i].rotation; // * Quaternion.Inverse(initialRotation);
				initialLocalRotations[i] = bones[i].localRotation;
			}
		}

		// Restore the initial rotation
		if(offsetNode != null)
		{
			offsetNode.transform.rotation = initialRotation;
		}
		else
		{
			transform.rotation = initialRotation;
		}
	}
	
	// Converts kinect joint rotation to avatar joint rotation, depending on joint initial rotation and offset rotation
	protected Quaternion Kinect2AvatarRot(Quaternion jointRotation, int boneIndex)
	{
		Quaternion newRotation = jointRotation * initialRotations[boneIndex];

		if (offsetNode != null)
		{
			Vector3 totalRotation = newRotation.eulerAngles + offsetNode.transform.rotation.eulerAngles;
			newRotation = Quaternion.Euler(totalRotation);
		}
		
		return newRotation;
	}

	// dictionaries to speed up bones' processing
	private readonly Dictionary<int, HumanBodyBones> boneIndex2MecanimMap = new Dictionary<int, HumanBodyBones>
	{
		{0, HumanBodyBones.Hips},
		{1, HumanBodyBones.Spine},
//      {2, HumanBodyBones.Chest},
		{3, HumanBodyBones.Neck},
		{4, HumanBodyBones.Head},
		
		{5, HumanBodyBones.LeftUpperArm},
		{6, HumanBodyBones.LeftLowerArm},
		{7, HumanBodyBones.LeftHand},
		{8, HumanBodyBones.LeftIndexProximal},
		{9, HumanBodyBones.LeftIndexIntermediate},
		{10, HumanBodyBones.LeftThumbProximal},
		
		{11, HumanBodyBones.RightUpperArm},
		{12, HumanBodyBones.RightLowerArm},
		{13, HumanBodyBones.RightHand},
		{14, HumanBodyBones.RightIndexProximal},
		{15, HumanBodyBones.RightIndexIntermediate},
		{16, HumanBodyBones.RightThumbProximal},
		
		{17, HumanBodyBones.LeftUpperLeg},
		{18, HumanBodyBones.LeftLowerLeg},
		{19, HumanBodyBones.LeftFoot},
		{20, HumanBodyBones.LeftToes},
		
		{21, HumanBodyBones.RightUpperLeg},
		{22, HumanBodyBones.RightLowerLeg},
		{23, HumanBodyBones.RightFoot},
		{24, HumanBodyBones.RightToes},
		
		{25, HumanBodyBones.LeftShoulder},
		{26, HumanBodyBones.RightShoulder},
	};

	protected readonly Dictionary<int, int> boneIndexToJointMap = new Dictionary<int, int>
	{
		{1, 8},

		{3, 1},
		{4, 0},
		
		{5, 2},
		{6, 4},
//		{8, 6},
				
		{11, 3},
		{12, 5},
//		{14, 7},

//		{17, 9},
//		{18, 11},
//		{20, 13},
		
//		{21, 10},
//		{22, 12},
//		{24, 14}
	};

	protected readonly Dictionary<int, int> boneIndexToJointMapMirrored = new Dictionary<int, int>
	{
		{1, 8},
		
		{3, 1},
		{4, 0},
		
		{5, 3},
		{6, 5},
//		{8, 7},
		
		{11, 2},
		{12, 4},
//		{14, 6},
		
//		{17, 10},
//		{18, 12},
//		{20, 14},
		
//		{21, 9},
//		{22, 11},
//		{24, 13}
	};

}

