
using UnityEngine;

public class ClimberTrajectory : MonoBehaviour {
    [SerializeField] GameObject dotsParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] GameObject cancelSlingGraphich;
    [SerializeField] int dotsNumber;
    [SerializeField] float dotSpacing;

    [SerializeField][Range(0.01f, 0.3f)] float dotMinScale;
    [SerializeField][Range(0.3f, 1f)] float dotMaxScale;

    Transform[] dotsList;

    float timeStamp;
    Vector2 pos;

    GameObject placedCancelSlingGraphich;

    void Start() {
        PrepareDots();
    }

    void PrepareDots() {
        dotsList = new Transform[dotsNumber];
        dotPrefab.transform.localScale = Vector2.one * dotMaxScale;

        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;

        for (int i = 0; i < dotsNumber; i++) {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;

            dotsList[i].localScale = Vector2.one * scale;
            if (scale > dotMinScale)
                scale -= scaleFactor;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied) {
        timeStamp = dotSpacing;
        for (int i = 0; i < dotsNumber; i++) {
            pos.x = (ballPos.x + forceApplied.x * timeStamp);
            pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude*2 * timeStamp * timeStamp) / 2f;
            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }
    }

    public void Show ()
	{
		dotsParent.SetActive (true);
	}
    public void Show (Vector2 pos)
	{
		dotsParent.SetActive (true);
        if (!placedCancelSlingGraphich) {
            placedCancelSlingGraphich = Instantiate(cancelSlingGraphich, pos, Quaternion.identity);
        }
    }

	public void Hide ()
	{
        if(placedCancelSlingGraphich) Destroy(placedCancelSlingGraphich);
		dotsParent.SetActive (false);
	}
}