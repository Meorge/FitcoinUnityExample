using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPageManager : MonoBehaviour
{
    public List<RectTransform> pages = new List<RectTransform>();
    public int currentIndex = 0;

    public float animationDuration = 0.5f;

    public void Start() {
        for (int i = 0; i < pages.Count; i++) {
            pages[i].gameObject.SetActive(currentIndex == i); 
        }
    }

    public void SegueForward(int newIndex) => SegueToPage(newIndex);
    public void SegueBackward(int newIndex) => SegueToPage(newIndex, true);

    public void SegueToPage(int newIndex, bool moveBack = false) {
        /*
        - current page should move to the right
        - set next page's position to out of view
        - move next page into center
        */
        int previousIndex = currentIndex;
        currentIndex = newIndex;

        DOTween.Sequence()
            // Set new current page to just out of view
            .AppendCallback(() => { pages[currentIndex].gameObject.SetActive(true); })
            .Append(pages[currentIndex].DOAnchorPosX(moveBack ? -2360 : 2360, 0))

            // Swipe both of them left
            .Append(pages[currentIndex].DOAnchorPosX(0, animationDuration))
            .Join(pages[previousIndex].DOAnchorPosX(moveBack ? 2360 : -2360, animationDuration))
            .AppendCallback(() => { pages[previousIndex].gameObject.SetActive(true); });
    }


}
