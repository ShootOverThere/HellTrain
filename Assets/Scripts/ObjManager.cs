using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour{

    static ObjManager st;
    public static ObjManager Call() { return st; }
    void Awake() { st = this; }

    void OnDestroy(){
        MemoryDelete();
        st = null;
    }

    public GameObject[] Origin;
    public List<GameObject> Manager;

    void Start(){
        SetObject(Origin[0], 1, "missile");
    }

    public void SetObject(GameObject _Obj, int _Count, string _Name){
        for(int i = 0 ; i< _Count; i++){
            GameObject obj = Instantiate(_Obj) as GameObject;
            obj.transform.name = _Name;
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
            obj.transform.parent = transform;
            Manager.Add(obj);
        }
    }

    public void SetObject(string _Name, int _Count){

        GameObject obj = null;
        int Count = Origin.Length;
        for(int i = 0 ;i<Count;i++){
            if (Origin[i].name == _Name)
                obj = Origin[i];
        }
        SetObject(obj, _Count, _Name);
    }

    public GameObject GetObject(string _Name){

        if (Manager == null)
            return null;

        int Count = Manager.Count;
        for (int i = 0 ; i<Count ;i++){

            if (_Name != Manager[i].name) {Debug.Log("wow"); continue;}

            GameObject Obj = Manager[i];

            if (Obj.activeSelf == true){
                if (i == Count-1){
                    SetObject(Obj,1,"missile");
                    return Manager[i+1];
                }
                continue;
            }
            return Manager[i];
        }
        return null;
    }

    public void MemoryDelete(){
        if(Manager == null)
            return;

        int Count = Manager.Count;

        for(int i = 0 ; i<Count ; i++){
            GameObject obj = Manager[i];
            GameObject.Destroy(obj);
        }
        Manager = null;
    }



}