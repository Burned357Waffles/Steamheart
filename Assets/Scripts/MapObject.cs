using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class will be used as a parent class for anything that is on a tile and can be attacked.
 * Tile improvements not included?
 * This includes:
 *   - Units
 *   - Cities
 * 
 * It will contain:
 *   - Hex grid coordinate of the object
 *   - Health
 *   - Damage
 *   - Selected
 *   - Create
 */
public abstract class MapObject : MonoBehaviour
{
    public abstract int[] coordinates = new int[2];
    public abstract int Health;
    public abstract int Damage;

    // checks if this object is selected. (this might be handled in unity instead)
    protected virtual void isSelected()
    {

    }

    // is called if certain button is pushed. 
    // set x and y pos to selected tile ()
    // init health to max
    // init damage 
    protected virtual void createObject()
    {
        
    }

    // this will set the x and y pos in the coordinate array
    protected virtual void setCoordinates(int x, int y)
    {
        this.coordinates[0] = x;
        this.coordinates[1] = y;
    }
    
    protected virtual int[] getCoordinates() { return coordinates; }
    protected virtual int getHealth() { return Health; }
    protected virtual int getDamage() { return Damage; }

    // to be implemented in child classes
    public abstract void setHealth();
    public abstract void setDamage();
}
