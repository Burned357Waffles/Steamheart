using System;
using System.Collections.Generic;
using System.Linq;
using Hex;
using Misc;
using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class handles the movement of units. It checks if the player
    /// selects a unit, then if they want to move it or attack with it. A
    /// player is only allowed to move their own units.
    /// </summary> ***********************************************************
    /// TODO: maybe transfer some of the combat related functions to the combat file
    public class UnitMovement : MonoBehaviour
    {
        private HexGrid _hexGrid;
        private int _currentHexIndex;
        private int _goalHexIndex;
        private Hex.Hex _currentHex;
        private Hex.Hex _goalHex;
        private Unit _selectedUnit;
        private GameObject _selectedUnitObject;

        private int _currentPlayer;
        private Camera _camera;
        private FMODUnity.StudioEventEmitter _selectEmitter;
        
        /* Heuristic was needed for A* algorithm
     https://www.redblobgames.com/pathfinding/a-star/introduction.html#greedy-best-first
    */
        /*
        private float Heuristic(Vector3 a, Vector3 b)
        {
            return Mathf.Abs(a.x - b.x ) + Mathf.Abs(a.y - b.y);
        }

        public Queue<Vector3> hex_reachable(Vector3 start, int movement)
        {
            Queue<Vector3> visited = new Queue<Vector3>();
            visited.Enqueue(start);

            List<List<Vector3>> fringes = new List<List<Vector3>>();
            fringes.Add(new List<Vector3> { start });

            for (int i = 1; i <= movement; i++)
            {
                fringes.Add(new List<Vector3>());
                foreach(Vector3 hex in fringes[i-1])
                {
                    for (int j = 0; j < 6; j++)
                    {
                        Vector3 neighbor = HexGrid.HexNeighbor(_hexGrid.GetHexAt(hex).GetVectorCoordinates(), j);
                        if (!visited.Contains(neighbor) && _hexGrid.GetHexAt(hex).IsBlocked())
                        {  
                            visited.Enqueue(neighbor);
                            fringes[i].Add(neighbor);
                        }
                    }
                }
            
            }
            return visited; 
        }
    */
        /********
    This is the beginning of an A* search according to https://www.redblobgames.com/pathfinding/a-star/introduction.html#breadth-first-search
    may have made hex_reachable redundant, but we'll see
    Queue needs to be replaced by PriorityQueue, but not sure how to do that, this should work but not efficiently
    
        public List<Hex.Hex> pathFind(Vector3 start, Vector3 goal)
        {
            Utils.PriorityQueue<Vector3, int> frontier = new Utils.PriorityQueue<Vector3, int>(); 
            frontier.Enqueue(start, 0);

            Dictionary<Vector3, Vector3> came_from = new Dictionary<Vector3, Vector3>();
            Dictionary<Vector3, int> cost = new Dictionary<Vector3, int>();

            came_from[start] = start;
            cost[start] = 0;

            List<Hex.Hex> path = new List<Hex.Hex>();

            while(frontier.Count > 0)
            {
                Vector3 current = frontier.Dequeue();

                if(current == goal)
                {
                    break;
                }
                // There should be a foreach, but couldn't figure it out.
                List<Vector3> neighborList = new List<Vector3>(); 
                for (int j = 0; j < 6; j++)
                {
                    Vector3 neighbor = HexGrid.HexNeighbor(_hexGrid.GetHexAt(current).GetVectorCoordinates(), j);
                    neighborList.Add(neighbor);     
                }
                foreach(Vector3 next in neighborList )
                {
                    int blockCost = 0;
                    int newCost = 0;
                    int priority = 0;
                    if(!_hexGrid.GetHexAt(next).IsBlocked())
                    {
                        blockCost = 1;
                    }
                    newCost = cost[current] + blockCost;
                    if(!cost.ContainsKey(next) || newCost < cost[next])
                    {
                        cost[next] = newCost;
                        priority = (int)(newCost + Heuristic(goal, next));
                        frontier.Enqueue(next, priority);
                        came_from[next] = current;
                    }

                }
            
            }
            return path;

        }
        */

        // use WorldPosition in Hex to get actual position

        public void SetCurrentIndex(int index) { _currentHexIndex = index; }
        
        private void Start()
        {
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            ResetIndices();
            _currentPlayer = 1;

            _selectEmitter = GameObject.Find("Select").GetComponent<FMODUnity.StudioEventEmitter>();
        }

        private void Update()
        {
            DetectClick();
        }

        /// <summary> ***********************************************
        /// This function handles user input and calls the necessary
        /// functions.
        /// </summary> **********************************************
        private void DetectClick()
        {
            // check if a unit is clicked
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;
                if (!hit.transform.CompareTag("Unit")) return;
                
                _currentHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
                if (_currentHexIndex < 0) return;
                _currentHex = _hexGrid.GetHexList()[_currentHexIndex];
                if (_hexGrid.GetUnitDictionary()[_currentHex].GetOwnerID() != _currentPlayer)
                {
                    _currentHexIndex = -1;
                    return;
                }
                
                _selectEmitter.Play();
                
                return;
            }


            // check if a tile is clicked

            if (Input.GetMouseButtonDown(1) && _currentHexIndex != -1)
            {
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;

                _goalHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
                if (_currentHexIndex < 0 || _goalHexIndex < 0) return;
                _currentHex = _hexGrid.GetHexList()[_currentHexIndex];
                _goalHex = _hexGrid.GetHexList()[_goalHexIndex];

                if (_hexGrid.GetUnitDictionary()[_currentHex].GetCurrentMovementPoints() <= 0)
                {
                    ResetIndices();
                    return;
                }

                bool doDeplete = false;

                if (_hexGrid.GetCityAt(_goalHex) != null)
                {
                    // if another player's city is clicked
                    Debug.Log("City Clicked");
                    if (_hexGrid.GetCityAt(_goalHex) != null &&
                        _hexGrid.GetUnitDictionary()[_currentHex].GetOwnerID() ==
                        _hexGrid.GetCityAt(_goalHex).GetOwnerID())
                    {
                        Debug.Log("first if return");
                        return;
                    }
                    
                    doDeplete = true;
                    // if target is still alive
                    if (DoCityCombat()) 
                    {
                        if (!_hexGrid.GetUnitDictionary().ContainsKey(_currentHex))
                        {
                            ResetIndices();
                            return;
                        }
                        _hexGrid.GetUnitDictionary()[_currentHex].DepleteMovementPoints();
                        ResetIndices();
                        return;
                    }
                }
                
                else if (_hexGrid.GetUnitDictionary().ContainsKey(_goalHex))
                {
                    // if there is already a unit at this hex
                    if (_hexGrid.GetUnitDictionary().ContainsKey(_goalHex))
                        if (_hexGrid.GetUnitDictionary()[_currentHex] == _hexGrid.GetUnitDictionary()[_goalHex])
                            return;
                    if (!IsTargetInRange(_currentHex, _goalHex, _hexGrid.GetUnitDictionary()[_currentHex].AttackRadius))
                        return;
                    
                    doDeplete = true;
                    // if target is still alive
                    if (DoCombat()) 
                    {
                        if (_hexGrid.GetUnitDictionary().ContainsKey(_currentHex))
                            _hexGrid.GetUnitDictionary()[_currentHex].DepleteMovementPoints();
                        ResetIndices();
                        return;
                    }

                    if (_hexGrid.GetUnitDictionary()[_currentHex].GetUnitType() != Unit.UnitType.Melee)
                    {
                        _hexGrid.GetUnitDictionary()[_currentHex].DepleteMovementPoints();
                        ResetIndices();
                        return;
                    }
                }

                if (!SelectedTileIsNeighbor()) return;
                if (_goalHex.IsBlocked() && 
                    _hexGrid.GetUnitDictionary()[_currentHex].GetUnitType() != Unit.UnitType.Airship)
                    return;
                
                if (_hexGrid.GetUnitDictionary().ContainsKey(_currentHex))
                {
                    if (_hexGrid.GetUnitDictionary()[_currentHex].GetOwnerID() != _currentPlayer)
                    {
                        ResetIndices();
                        return;
                    }
                }
                
                _selectedUnit = _hexGrid.GetUnitDictionary()[_currentHex];
                _selectedUnitObject = _hexGrid.GetUnitObjectDictionary()[_selectedUnit];
                
                if (_hexGrid.GetUnitDictionary().ContainsKey(_goalHex)) return;
                // TODO: add variables to network
                if (!MoveUnit()) return;
                
                if (doDeplete) _selectedUnit.DepleteMovementPoints();
                _currentHexIndex = _goalHexIndex;
                _goalHexIndex = -1;
            }
        }
        
        /// <summary> ***********************************************
        /// This function sets the current and goal Hex, then checks
        /// if the goal hex is a neighbor of current hex. If it is,
        /// then it will check if the goal hex is a blocked hex.
        /// If the hex is not blocked and goal hex is a neighbor
        /// return true, if hex is neighbor but is blocked or is not
        /// a neighbor, then return false.
        /// </summary> **********************************************
        private bool SelectedTileIsNeighbor()
        {
            for (int j = 0; j < 6; j++)
            {
                Vector3 neighbor = HexGrid.HexNeighbor(_currentHex.GetVectorCoordinates(), j);
                if (neighbor == _goalHex.GetVectorCoordinates())
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary> ***********************************************
        /// This function compares the distance from attacker to
        /// the target. Returns true if target is in range.
        /// </summary> **********************************************
        private bool IsTargetInRange(Hex.Hex current, Hex.Hex goal, int range)
        {
            Vector3 resultVector = current.GetVectorCoordinates() - goal.GetVectorCoordinates();
            int distance = (int)(Math.Abs(resultVector.x) + Math.Abs(resultVector.y) + Math.Abs(resultVector.z)) / 2;

            return distance <= range;
        }

        /// <summary> ***********************************************
        /// This function updates the Units' coordinates and updates
        /// the dictionary that holds the relation between the Hex
        /// and the Unit.
        /// </summary> **********************************************
        private bool MoveUnit()
        {
            if ((_currentHex.GetHexType() == Hex.Hex.HexType.Forest || _goalHex.GetHexType() == Hex.Hex.HexType.Forest)
                && _selectedUnit.GetUnitType() != Unit.UnitType.Airship)
            {
                if (_selectedUnit.GetCurrentMovementPoints() < 2)
                {
                    Debug.Log("returning on forest");
                    return false;
                }
                Debug.Log("using movement on forest");
                _selectedUnit.UseMovementPoints();
            }
                
            
            _selectedUnit.Q = _goalHex.Q;
            _selectedUnit.R = _goalHex.R;
            _selectedUnit.S = _goalHex.S;

            Vector3 goalVector = _goalHex.WorldPosition; 
            _selectedUnitObject.transform.position = goalVector;
        
            _hexGrid.GetUnitDictionary().Remove(_currentHex);
            _hexGrid.GetUnitDictionary().Add(_goalHex, _selectedUnit);
            _selectedUnit.UseMovementPoints();
            return true;
        }

        /// <summary> ***********************************************
        /// This function sets the playerID based on whose turn it
        /// currently is. 
        /// </summary> **********************************************
        public void SetPlayer(int currentPlayer)
        {
            _currentPlayer = currentPlayer;
        }

        private bool DoCityCombat()
        {
            if (_hexGrid.GetCityAt(_goalHex) == null) return true;
            
            Debug.Log("ATTACKING CITY");
            Unit attacker = _hexGrid.GetUnitDictionary()[_currentHex];
            City city = _hexGrid.GetCityAt(_goalHex);
            
            Debug.Log("Attacker health before attack: " + attacker.Health);
            Debug.Log("Defender health before attack: " + city.Health);
            
            if (_hexGrid.GetUnitDictionary().ContainsKey(_goalHex))
            {
                return DoCombat(); //TODO: return after attack
            }
            
            bool taken = Combat.InitiateCombat(attacker, city);
            if (!taken)
            {
                Debug.Log("Defender Not Dead");
                bool dead = false;
                if (IsTargetInRange(_goalHex, _currentHex, city.AttackRadius))
                {
                    Debug.Log("City retaliating");
                    dead = Combat.InitiateCombat(city, attacker);
                    Debug.Log(dead ? "dead" : "not dead");
                }
                
                Debug.Log("Attacker Health: " + attacker.Health);
                if (!dead) return true;
                
                Destroy(_hexGrid.GetUnitObjectDictionary()[attacker]);
                _hexGrid.GetUnitObjectDictionary().Remove(attacker);
                _hexGrid.GetUnitDictionary().Remove(_currentHex);
                return true;
            }
            
            Debug.Log("TAKEN");
            return RemovePlayer(_hexGrid.FindPlayerOfID(attacker.GetOwnerID()),
                _hexGrid.FindPlayerOfID(city.GetOwnerID()),
                city);
        }

        private bool DoCombat()
        {
            if (!_hexGrid.GetUnitDictionary().ContainsKey(_currentHex) ||
                !_hexGrid.GetUnitDictionary().ContainsKey(_goalHex))
                return true;

            Unit attacker = _hexGrid.GetUnitDictionary()[_currentHex];
            Unit defender = _hexGrid.GetUnitDictionary()[_goalHex];
            
            //if (_hexGrid.GetUnitDictionary()[_currentHex].GetOwnerID() != _currentPlayer) return true;

            Debug.Log("ATTACKING UNIT");
            bool dead = Combat.InitiateCombat(attacker, defender);
            if (!dead)
            {
                Debug.Log("Defender Not Dead");
                bool attackerDead = false;
                if (IsTargetInRange(_goalHex, _currentHex, defender.AttackRadius))
                    attackerDead = Combat.InitiateCombat(defender, attacker);
                
                Debug.Log("Attacker Health: " + attacker.Health);
                if (!attackerDead) return true;
                
                Destroy(_hexGrid.GetUnitObjectDictionary()[attacker]);
                _hexGrid.GetUnitObjectDictionary().Remove(attacker);
                _hexGrid.GetUnitDictionary().Remove(_currentHex);

                return true;
            }
            Destroy(_hexGrid.GetUnitObjectDictionary()[defender]);
            _hexGrid.GetUnitObjectDictionary().Remove(defender);
            _hexGrid.GetUnitDictionary().Remove(_goalHex);
            
            return false;
        }

        private bool RemovePlayer(Player attackerPlayer, Player defenderPlayer, City city)
        {
            defenderPlayer.RemoveCity(city);
            attackerPlayer.AssignCity(city);
            
            if (defenderPlayer.IsAlive) return true;
            Debug.Log("Player " + defenderPlayer.GetPlayerID() + " has been defeated");
            foreach (KeyValuePair<Hex.Hex, Unit> keyValue in _hexGrid.GetUnitDictionary()
                         .Where(pair => pair.Value.GetOwnerID() == defenderPlayer.GetPlayerID()).ToList())
            {
                Debug.Log("Removing Unit");
                Destroy(_hexGrid.GetUnitObjectDictionary()[keyValue.Value]);
                _hexGrid.GetUnitObjectDictionary().Remove(keyValue.Value);
                _hexGrid.GetUnitDictionary().Remove(keyValue.Key);
            }
            
            // set all owned tiles not in border to ownerID 0
            foreach (Hex.Hex hex in _hexGrid.GetHexList())
            {
                hex.SetOwnerID(0);
            }
            
            _hexGrid.GetPlayerList().Remove(defenderPlayer);
            // TODO: send player to end screen
            return false;
        }

        private void ResetIndices()
        {
            _currentHexIndex = -1;
            _goalHexIndex = -1;
        }
    }
}
