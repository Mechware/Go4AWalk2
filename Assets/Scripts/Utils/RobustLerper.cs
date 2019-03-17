using CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobustLerper : MonoBehaviour {

    public enum LoopType {
        CallOnEnd,
        Clamp,
        Restart,
        Reverse
    }

    public UnityEvent OnStart;
    public UnityEvent OnUpdate;
    public UnityEvent OnEnd;

    public LoopType EndBehaviour;

    public bool AutoStart = false;
    public float LerpDuration = 1f;

    public List<LerpObject> Lerpers;

    private bool playing = false;
    private float duration;

    [ContextMenu("Play")]
    public void StartLerping() {
        OnStart.Invoke();
        duration = 0;
        playing = true;
    }

    [ContextMenu("Pause")]
    public void PauseLerp() {
        playing = true;
    }

    [ContextMenu("Stop")]
    public void EndLerping() {
        playing = false;
        OnEnd.Invoke();
    }

    private void Start() {
        if(AutoStart) {
            StartLerping();
        }
    }

    // Update is called once per frame
    void Update () {
        if(!playing)
            return;

        duration += Time.deltaTime;
        float tempDuration = duration;

        if(EndBehaviour == LoopType.Clamp) {
            tempDuration = Mathf.Min(tempDuration, LerpDuration);
        } else if(EndBehaviour == LoopType.Restart) {
            tempDuration = tempDuration % LerpDuration;
        } else if(EndBehaviour == LoopType.Reverse) {
            tempDuration = tempDuration % (LerpDuration * 2);
            if(tempDuration > LerpDuration) {
                tempDuration = 2 * LerpDuration - tempDuration;
            }
        } else if (EndBehaviour == LoopType.CallOnEnd) {
            if(duration > LerpDuration) {
                EndLerping();
                return;
            }
        }

        foreach(LerpObject lerper in Lerpers) {
            lerper.Update(tempDuration);
        }
    }

    [System.Serializable]
    public class LerpObject {
        public AnimationCurve Values;
        public UnityEventFloat OnUpdate;

        public float Update(float duration) {
            OnUpdate.Invoke(Values.Evaluate(duration));
            return Values.Evaluate(duration);
        }
    }
}
