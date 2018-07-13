﻿// logs Myo poses

using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class Logger : MonoBehaviour {
    
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    // Update is called once per frame.
    void Update()
    {
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Check if the pose has changed since last update.
        // The ThalmicMyo component of a Myo game object has a pose property that is set to the
        // currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
        // detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
        // is not on a user's arm, pose will be set to Pose.Unknown.
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.Fist)
            {
                Debug.Log("Fist");
            }
            else if (thalmicMyo.pose == Pose.WaveIn)
            {
                Debug.Log("Wave in");
            }
            else if (thalmicMyo.pose == Pose.WaveOut)
            {
                Debug.Log("Wave out");
            }
            else if (thalmicMyo.pose == Pose.DoubleTap)
            {
                Debug.Log("Double tap");
            }
            else if (thalmicMyo.pose == Pose.FingersSpread)
            {
                Debug.Log("Fingers spread");
            }
        }
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo1)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo1.Unlock(UnlockType.Timed);
        }

        myo1.NotifyUserAction();
    }
}
