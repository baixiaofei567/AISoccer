using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=11")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeObject : Conditional
    {
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
        [Tooltip("The object that we are searching for")]
        public SharedGameObject targetObject;
        [Tooltip("The objects that we are searching for")]
        public SharedGameObjectList targetObjects;
        [Tooltip("The tag of the object that we are searching for")]
        public SharedString targetTag;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 1000;
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [Tooltip("The offset to apply to 2D angles")]
        public SharedFloat angleOffset2D;
        [Tooltip("Should the target bone be used?")]
        public SharedBool useTargetBone;
        [Tooltip("The target's bone if the target is a humanoid")]
        public HumanBodyBones targetBone;
        [Tooltip("The object that is within sight")]
        public SharedGameObject returnedObject;

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            if (usePhysics2D) {
                if (targetObjects.Value != null && targetObjects.Value.Count > 0) { // If there are objects in the group list then search for the object within that list
                    GameObject objectFound = null;
                    float minAngle = Mathf.Infinity;
                    for (int i = 0; i < targetObjects.Value.Count; ++i) {
                        float angle;
                        GameObject obj;
                        if ((obj = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObjects.Value[i], targetOffset.Value, true, angleOffset2D.Value, out angle, ignoreLayerMask, useTargetBone.Value, targetBone)) != null) {
                            // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                            if (angle < minAngle) {
                                minAngle = angle;
                                objectFound = obj;
                            }
                        }
                    }
                    returnedObject.Value = objectFound;
                } else if (targetObject.Value == null) { // If the target object is null then determine if there are any objects within sight based on the layer mask
                    returnedObject.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, targetOffset.Value, angleOffset2D.Value, ignoreLayerMask);
                } else if (!string.IsNullOrEmpty(targetTag.Value)) { // If the target tag is not null then determine if there are any objects within sight based on the tag
                    returnedObject.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, GameObject.FindGameObjectWithTag(targetTag.Value), targetOffset.Value, angleOffset2D.Value, ignoreLayerMask, useTargetBone.Value, targetBone);
                } else { // If the target is not null then determine if that object is within sight
                    returnedObject.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value, targetOffset.Value, angleOffset2D.Value, ignoreLayerMask, useTargetBone.Value, targetBone);
                }
            } else {
                if (targetObjects.Value != null && targetObjects.Value.Count > 0) { // If there are objects in the group list then search for the object within that list
                    GameObject objectFound = null;
                    float minAngle = Mathf.Infinity;
                    for (int i = 0; i < targetObjects.Value.Count; ++i) {
                        float angle;
                        GameObject obj;
                        if ((obj = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObjects.Value[i], targetOffset.Value, false, angleOffset2D.Value, out angle, ignoreLayerMask, useTargetBone.Value, targetBone)) != null) {
                            // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                            if (angle < minAngle) {
                                minAngle = angle;
                                objectFound = obj;
                            }
                        }
                    }
                    returnedObject.Value = objectFound;
                } else if (targetObject.Value == null) { // If the target object is null then determine if there are any objects within sight based on the layer mask
                    returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone);
                } else if (!string.IsNullOrEmpty(targetTag.Value)) { // If the target tag is not null then determine if there are any objects within sight based on the tag
                    returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, GameObject.FindGameObjectWithTag(targetTag.Value), targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone);
                } else { // If the target is not null then determine if that object is within sight
                    returnedObject.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value, targetOffset.Value, ignoreLayerMask, useTargetBone.Value, targetBone);
                }
            }
            if (returnedObject.Value != null) {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            fieldOfViewAngle = 90;
            viewDistance = 1000;
            offset = Vector3.zero;
            targetOffset = Vector3.zero;
            angleOffset2D = 0;
            targetTag = "";
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, angleOffset2D.Value, viewDistance.Value, usePhysics2D);
        }

        public override void OnBehaviorComplete()
        {
            MovementUtility.ClearCache();
        }
    }
}