using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public Transform guideParent;

    public GameObject[] guides;

    private AudioSource paperSFX;

    public int currentPage;

    public GameObject guideUI;

    void Start()
    {
        paperSFX = GetComponent<AudioSource>();

        guides = new GameObject[guideParent.transform.childCount];

        currentPage = 0;

        if (PlayerPrefs.GetInt("levelReached") == 0)
        {
            guideUI.SetActive(true);
            PlayerPrefs.SetInt("levelReached", 1);
        }
    }

    void Update()
    {
        for (int i = 0; i < guides.Length; i++)
        {
            guides[i] = guideParent.transform.GetChild(i).gameObject;
        }

        guides[currentPage].SetActive(true);
    }

    public void NextPage()
    {
        if (currentPage < guides.Length - 1)
        {
            guides[currentPage].SetActive(false);
            currentPage++;
            paperSFX.Play();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            guides[currentPage].SetActive(false);
            currentPage--;
            paperSFX.Play();
        }
    }

    public void ResetPage()
    {
        guides[currentPage].SetActive(false);
        currentPage = 0;
    }
}
