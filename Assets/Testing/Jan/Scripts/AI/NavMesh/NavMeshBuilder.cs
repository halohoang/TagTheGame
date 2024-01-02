using Interactables;
using NaughtyAttributes;
using NavMeshPlus.Components;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBuilder : MonoBehaviour
{
    // for more Information on Runtime Baking Navmesh see also: https://github.com/h8man/NavMeshPlus/wiki/HOW-TO (last checked on 17.10.2023); JM

    [SerializeField, ReadOnly] private NavMeshSurface _navSurface;

    private void Awake()
    {
        _navSurface = GetComponent<NavMeshSurface>();
    }

    private void OnEnable()
    {
        Interactable_Door.OnDoorStatusChange += BakeNewNavMesh;
        Interactable_Console.OnDoorStatusChange += BakeNewNavMesh;
        CheatInput.OnResetDoors += BakeNewNavMesh;
    }

    private void OnDisable()
    {
        Interactable_Door.OnDoorStatusChange -= BakeNewNavMesh;
        Interactable_Console.OnDoorStatusChange -= BakeNewNavMesh;
        CheatInput.OnResetDoors -= BakeNewNavMesh;
    }

    private void Start()
    {
        // building the NavMesh on GameStart new (so if it was updated during runtime it is set to deafault (doesn't bake over doors))
        _navSurface.BuildNavMeshAsync();
    }

    private void BakeNewNavMesh()
    {
        // update the NavMesh Date for new runtime bakening the NavmeshSurface after Door-Kick-in or Door-Reset
        _navSurface.UpdateNavMesh(_navSurface.navMeshData);
    }
}
