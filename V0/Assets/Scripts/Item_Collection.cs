using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AllControl;

public class Item_Collection : MonoBehaviour
{
    int orange = GameManager.Instance.score;
    [SerializeField] private Text orangeText;
    [SerializeField] private AudioSource collectSoundEffect;

    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orange"))
        {
            collectSoundEffect.Play();
            Destroy(collision.gameObject);
            orange++;
            orangeText.text = "You got:" + orange;

            GameManager.Instance.score = orange;

        }
    }

}

//private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (collision.CompareTag("Orange"))
//    {
//        Debug.Log($"触发收集: {collision.gameObject.name}", collision.gameObject);

//        // 打印层级结构
//        Transform parent = collision.transform.parent;
//        while (parent != null)
//        {
//            Debug.Log("父物体: " + parent.name);
//            parent = parent.parent;
//        }

//        Destroy(collision.gameObject); // 或改用 DestroyImmediate 测试
//        orange++;
//        orangeText.text = "你收集到的Orange数量为:" + orange;
//        Debug.Log("当前橙子数: " + orange);
//    }
//}