using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

// Used for the Hat selection logic
public class PlayerConfigurator : MonoBehaviour
{
    [SerializeField]
    private Transform m_HatAnchor;

    // private ResourceRequest m_HatLoadingRequest;
    // private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;
    // private AsyncOperationHandle<IList<GameObject>> m_HatsLoadOpHandle;
    private AsyncOperationHandle<IList<IResourceLocation>> m_HatsLocationsOpHandle;
    private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;

    // [SerializeField] private string m_Address; // Addressable asset address
    [SerializeField]
    // private AssetReferenceGameObject m_HatAssetReference; // Addressable asset reference
    private GameObject m_HatInstance;
    // private List<string> m_Keys = new List<string>() { "Hats" };
    // private List<string> m_Keys = new List<string>() {"Hats", "Seasonal"};
    private List<string> m_Keys = new List<string>() { "Hats", "Fancy" };

    void Start()
    {
        // SetHat(string.Format("Hat{0:00}", GameManager.s_ActiveHat));
        // LoadInRandomHat();
        // m_HatsLoadOpHandle = Addressables.LoadAssetsAsync<GameObject>(m_Keys, null, Addressables.MergeMode.Union);
        // m_HatsLoadOpHandle = Addressables.LoadAssetsAsync<GameObject>(m_Keys, null, Addressables.MergeMode.Intersection);
        // m_HatsLoadOpHandle.Completed += OnHatsLoadComplete;
        m_HatsLocationsOpHandle = Addressables.LoadResourceLocationsAsync(m_Keys, Addressables.MergeMode.Intersection);
        m_HatsLocationsOpHandle.Completed += OnHatLocationsLoadComplete;
    }
    // private void LoadInRandomHat()
    // {
    //     int randomIndex = Random.Range(0, 6);
    //     string hatAddress = string.Format("Hat{0:00}", randomIndex);

    //     m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(hatAddress);
    //     m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    // }
    // private void LoadInRandomHat(IList<GameObject> prefabs)
    // {
    //     int randomIndex = Random.Range(0, prefabs.Count);
    //     GameObject randomHatPrefab = prefabs[randomIndex];
    //     m_HatInstance = Instantiate(randomHatPrefab, m_HatAnchor);
    // }
    private void LoadInRandomHat(IList<IResourceLocation> resourceLocations)
    {
        int randomIndex = Random.Range(0, resourceLocations.Count);
        IResourceLocation randomHatPrefab = resourceLocations[randomIndex];

        m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(randomHatPrefab);
        m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            // Destroy(m_HatInstance);
            // Addressables.ReleaseInstance(m_HatLoadOpHandle);

            // LoadInRandomHat();
            Destroy(m_HatInstance);

            // LoadInRandomHat(m_HatsLoadOpHandle.Result);
            Addressables.Release(m_HatLoadOpHandle);

            LoadInRandomHat(m_HatsLocationsOpHandle.Result);
        }
    }

    // public void SetHat(string hatKey)
    // {
    //     if (!m_HatAssetReference.RuntimeKeyIsValid())
    //     {
    //         return;
    //     }

    //     m_HatLoadOpHandle = m_HatAssetReference.LoadAssetAsync<GameObject>();
    //     m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    // }

    // private void OnHatLoaded(AsyncOperation asyncOperation)
    // {
    //     Instantiate(m_HatLoadingRequest.asset as GameObject, m_HatAnchor, false);
    // }
    // private void OnHatLoadComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    // {
    //     // if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
    //     // {
    //     //     Instantiate(asyncOperationHandle.Result, m_HatAnchor);
    //     // }
    //     m_HatInstance = Instantiate(asyncOperationHandle.Result, m_HatAnchor);
    // }
    private void OnHatLoadComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            m_HatInstance = Instantiate(asyncOperationHandle.Result, m_HatAnchor);
        }
    }
    // private void OnHatsLoadComplete(AsyncOperationHandle<IList<GameObject>> asyncOperationHandle)
    private void OnHatLocationsLoadComplete(AsyncOperationHandle<IList<IResourceLocation>> asyncOperationHandle)
    {
        Debug.Log("AsyncOperationHandle Status: " + asyncOperationHandle.Status);

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> results = asyncOperationHandle.Result;
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log("Hat: " + results[i].PrimaryKey);
            }

            LoadInRandomHat(results);
        }
    }

    private void OnDisable()
    {
        // if (m_HatLoadingRequest != null)
        //     m_HatLoadingRequest.completed -= OnHatLoaded;
        // m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
        // m_HatsLoadOpHandle.Completed -= OnHatsLoadComplete;
        m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
        m_HatsLocationsOpHandle.Completed -= OnHatLocationsLoadComplete;
    }
}
