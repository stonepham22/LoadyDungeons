﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class Loading : MonoBehaviour
{
    // private AsyncOperation m_SceneOperation;
    private static AsyncOperationHandle<SceneInstance> m_SceneLoadOpHandle;

    [SerializeField]
    private Slider m_LoadingSlider;

    [SerializeField]
    private GameObject m_PlayButton, m_LoadingText;

    private void Awake()
    {
        StartCoroutine(loadNextLevel("Level_0" + GameManager.s_CurrentLevel));
    }

    private IEnumerator loadNextLevel(string level)
    {
        // m_SceneOperation = SceneManager.LoadSceneAsync(level);
        // m_SceneOperation.allowSceneActivation = false;
        m_SceneLoadOpHandle = Addressables.LoadSceneAsync(level, activateOnLoad: true);

        while (!m_SceneLoadOpHandle.IsDone) //!m_SceneOperation.isDone
        {
            m_LoadingSlider.value = m_SceneLoadOpHandle.PercentComplete;

            if (m_SceneLoadOpHandle.PercentComplete >= 0.9f && !m_PlayButton.activeInHierarchy)
                m_PlayButton.SetActive(true);

            yield return null;
        }

        Debug.Log($"Loaded Level {level}");
    }

    // Function to handle which level is loaded next
    // public void GoToNextLevel()
    // {
    //     // m_SceneOperation.allowSceneActivation = true;
    // }
}
