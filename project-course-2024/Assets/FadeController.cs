using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1.0f;
    [SerializeField] private GameObject endMenu;

    private bool isFading = false;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
    }
    public void StartFade()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeToBlack());
        }
    }

    private IEnumerator FadeToBlack()
    {
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(alpha));
            yield return null;
        }
        //Time.timeScale = 0f;
        endMenu.SetActive(true);
        isFading = false;
    }
}