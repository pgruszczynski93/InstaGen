using System;
using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class PanelFader : MonoBehaviour
    {
        [SerializeField] private AlphaTweenParameters _parameters;

        public void ChangePanel()
        {
            StartCoroutine(ChangePanelRoutine());
        }

        private IEnumerator ChangePanelRoutine()
        {
            yield return StartCoroutine(TweenHelper.AlphaTweenAction((inAlpha, outAlpha) =>
                {
                    _parameters.inGroup.alpha = inAlpha;
                    _parameters.outGroup.alpha = outAlpha;
                }, _parameters));

            StopCoroutine(TweenHelper.AlphaTweenAction());
            SetupInteracablePanel();
        }

        private void SetupInteracablePanel()
        {
            _parameters.inGroup.interactable = false;
            _parameters.inGroup.blocksRaycasts = false;
            _parameters.outGroup.interactable = true;
            _parameters.outGroup.blocksRaycasts = true;
        }
    }
}
