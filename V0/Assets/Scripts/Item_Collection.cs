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
//        Debug.Log($"�����ռ�: {collision.gameObject.name}", collision.gameObject);

//        // ��ӡ�㼶�ṹ
//        Transform parent = collision.transform.parent;
//        while (parent != null)
//        {
//            Debug.Log("������: " + parent.name);
//            parent = parent.parent;
//        }

//        Destroy(collision.gameObject); // ����� DestroyImmediate ����
//        orange++;
//        orangeText.text = "���ռ�����Orange����Ϊ:" + orange;
//        Debug.Log("��ǰ������: " + orange);
//    }
//}