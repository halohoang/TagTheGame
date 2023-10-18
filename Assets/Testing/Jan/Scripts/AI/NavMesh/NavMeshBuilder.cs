using Interactables;
using NaughtyAttributes;
using NavMeshPlus.Components;
using System;
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
        Interactable.OnDoorStatusChange += BakeNewNavMesh;
        CheatInput.OnResetDoors += BakeNewNavMesh;
    }

    private void OnDisable()
    {
        Interactable.OnDoorStatusChange -= BakeNewNavMesh;
        CheatInput.OnResetDoors -= BakeNewNavMesh;
    }

    private void Start()
    {
        // buiklding the NavMesh on GameStart new (so if it was updated during runtime it is set to deafault (don't bake over doors))
        _navSurface.BuildNavMeshAsync();
    }

    private void BakeNewNavMesh()
    {
        // update the NavMesh Date for new runtime bakening the NavmeshSurface after Door-Kick-in or Door-Reset
        _navSurface.UpdateNavMesh(_navSurface.navMeshData);
    }
}
