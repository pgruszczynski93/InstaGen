using System;
using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class PanelFader : MonoBehaviour
    {
        [SerializeField] private AlphaTweenParameters _parameters;

        private void OnEnable()
        {
            EventsHelper.OnInputPanelScrollFinished += ChangePanel;
        }

        private void OnDisable()
        {
            EventsHelper.OnInputPanelScrollFinished -= ChangePanel;
        }

        private void ChangePanel()
        {
            StartCoroutine(ChangePanelRoutine());
        }

        private IEnumerator ChangePanelRoutine()
        {
            SetupInteractablePanel();
            yield return StartCoroutine(TweenHelper.AlphaTweenAction((inAlpha, outAlpha) =>
                {
                    _parameters.inGroup.alpha = inAlpha;
                    _parameters.outGroup.alpha = outAlpha;
                }, _parameters));

            StopCoroutine(TweenHelper.AlphaTweenAction());
        }

        private void SetupInteractablePanel()
        {
            _parameters.inGroup.interactable = false;
            _parameters.inGroup.blocksRaycasts = false;
            _parameters.outGroup.interactable = true;
            _parameters.outGroup.blocksRaycasts = true;
        }
    }
}
