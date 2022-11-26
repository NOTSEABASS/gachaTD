using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClickFallback : MonoSingleton<ClickFallback>, IOnLeftMouseDown, IOnRightMouseDown {
  public enum Result {
    None,
    StopListen
  }
  public interface IListener {
    public Result OnClickFallback(MouseInputArgument arg);
  }

  private HashSet<IListener> listeners = new HashSet<IListener>();

  public void AddListener<T>(T listener) where T : MonoBehaviour, IListener {
    if (!listeners.Contains(listener)) {
      listeners.Add(listener);
    }
  }

  public MouseResult OnLeftMouseDown(MouseInputArgument arg) {
    CallListeners(arg);
    return MouseResult.BreakBehind;
  }

  public MouseResult OnRightMouseDown(MouseInputArgument arg) {
    CallListeners(arg);
    return MouseResult.BreakBehind;
  }

  private void CallListeners(MouseInputArgument arg) {
    var tempList = new List<IListener>(listeners);
    foreach (var listener in tempList) {
      if (listener.IsNullOrUObjectNull()) {
        continue;
      }
      var res = listener.OnClickFallback(arg);
      if(res == Result.StopListen) {
        listeners.Remove(listener);
      }
    }
  }

}
