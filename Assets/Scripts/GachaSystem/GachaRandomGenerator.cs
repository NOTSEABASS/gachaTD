using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaRandomGenerator
{
    private Dictionary<string, int> _weightMap;
    private int _sumWeight;

    //sum to random with weight
    private void InitGacha() {
        foreach (var meta in _weightMap) {
            _sumWeight += meta.Value;
        }
    }

    public void ConsturctDataFromDict(Dictionary<string, int> dict) {
        _weightMap = dict;
        InitGacha();
    }

    public string GetRandomMetaData() {
        var indicator = Random.Range(0,_sumWeight);
        foreach (var meta in _weightMap) {
            if (indicator >= meta.Value) {
                indicator -= meta.Value;
            }
            else {
                return meta.Key;
            }
        }
        return null;
    }
}
